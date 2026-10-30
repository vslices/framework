using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

var root = Path.GetFullPath(args.Length >= 1 ? args[0] : ".");
var outFile = Path.GetFullPath(args.Length >= 2 ? args[1] : "tools/forkext-extract/selection.txt");
var fork = Path.GetFullPath(Path.Combine(root, "src/VSlices/Fork"));
MSBuildLocator.RegisterDefaults();
using var workspace = MSBuildWorkspace.Create();
var loadWarnings = new List<string>();
workspace.WorkspaceFailed += (_, e) => loadWarnings.Add(e.Diagnostic.ToString());
var project = await workspace.OpenProjectAsync(Path.Combine(root, "src/VSlices/VSlices.csproj"));
var compilation = await project.GetCompilationAsync()
    ?? throw new InvalidOperationException("VSlices compilation unavailable");
var compileErrors = compilation.GetDiagnostics()
    .Where(e => e.Severity == DiagnosticSeverity.Error)
    .Select(e => e.ToString()).Take(30).ToList();
if (compileErrors.Count > 0)
    throw new InvalidOperationException("Upstream complete-source build has errors:\n" + string.Join("\n", compileErrors));

bool IsFork(string p) =>
    Path.GetFullPath(p).StartsWith(fork + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
string RelFork(string p) =>
    Path.GetRelativePath(fork, Path.GetFullPath(p)).Replace('\\', '/');
var sourceDocs = new Dictionary<string, Document>(StringComparer.OrdinalIgnoreCase);
var ownDocs = new List<Document>();
foreach (var doc in project.Documents)
{
    if (doc.FilePath is null || !doc.FilePath.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
        continue;
    if (IsFork(doc.FilePath)) sourceDocs[RelFork(doc.FilePath)] = doc;
    else ownDocs.Add(doc);
}
if (sourceDocs.Count < 500)
    throw new InvalidOperationException($"Expected complete source for closure, found {sourceDocs.Count} files.");

var selected = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
var queue = new Queue<string>();
var edges = new HashSet<string>(StringComparer.Ordinal);
int bindings = 0, analyzed = 0;

void Include(string relative, string from, string reason)
{
    if (!sourceDocs.ContainsKey(relative)) return;
    edges.Add($"{from}\t{relative}\t{reason.Replace('\t',' ')}");
    if (selected.Add(relative))
        queue.Enqueue(relative);
}

string? Canonical(INamedTypeSymbol symbol)
{
    var refs = symbol.DeclaringSyntaxReferences
        .Select(r => r.SyntaxTree.FilePath).Where(IsFork).Distinct().ToList();
    if (refs.Count == 0) return null;
    var matching = refs.Where(p =>
        string.Equals(Path.GetFileNameWithoutExtension(p), symbol.Name, StringComparison.OrdinalIgnoreCase))
        .OrderBy(p => p.Length).ToList();
    return RelFork(matching.Count > 0 ? matching.First() : refs.OrderBy(p => p.Length).First());
}

void Link(ISymbol? symbol, string from, string reason)
{
    if (symbol is null) return;
    if (symbol is IAliasSymbol alias) symbol = alias.Target;
    if (symbol is INamespaceSymbol) return;
    symbol = symbol.OriginalDefinition;
    if (symbol is IMethodSymbol reduced && reduced.ReducedFrom is { } original)
        symbol = original.OriginalDefinition;
    if (symbol is INamedTypeSymbol type)
    {
        var file = Canonical(type);
        if (file != null) Include(file, from, reason + " [type]");
    }
    else
    {
        foreach (var syntax in symbol.DeclaringSyntaxReferences)
        {
            var path = syntax.SyntaxTree.FilePath;
            if (IsFork(path)) Include(RelFork(path), from, reason);
        }
        if (symbol.ContainingType is { } container)
        {
            var file = Canonical(container);
            if (file != null) Include(file, from, reason + " [container]");
        }
    }
}

async Task Scan(Document document, string from)
{
    var tree = await document.GetSyntaxTreeAsync();
    if (tree is null) return;
    var model = compilation.GetSemanticModel(tree);
    var syntax = await tree.GetRootAsync();
    analyzed++;
    foreach (var node in syntax.DescendantNodesAndSelf())
    {
        bool symbolCandidate =
            node is SimpleNameSyntax or InvocationExpressionSyntax or
            ObjectCreationExpressionSyntax or BinaryExpressionSyntax or
            PrefixUnaryExpressionSyntax or PostfixUnaryExpressionSyntax or
            ElementAccessExpressionSyntax or AttributeSyntax or
            QueryExpressionSyntax;
        if (symbolCandidate)
        {
            var info = model.GetSymbolInfo(node);
            if (info.Symbol is { } bound)
            {
                bindings++;
                Link(bound, from, node.Kind().ToString());
            }
            else if (info.CandidateSymbols.Length > 0)
                foreach (var candidate in info.CandidateSymbols)
                    Link(candidate, from, "candidate " + node.Kind());
        }
        if (node is VariableDeclarationSyntax v && v.Type.IsVar)
            Link(model.GetTypeInfo(v.Type).Type, from, "inferred var");
        if (node is ExpressionSyntax expr &&
            node.Parent is (ArgumentSyntax or EqualsValueClauseSyntax or ReturnStatementSyntax
                or ArrowExpressionClauseSyntax or AssignmentExpressionSyntax
                or ConditionalExpressionSyntax or CastExpressionSyntax))
        {
            var conversion = model.GetConversion(expr);
            if (conversion.IsUserDefined)
                Link(conversion.MethodSymbol, from, "implicit conversion");
        }
    }
}

Console.WriteLine($"Loaded core compilation: {sourceDocs.Count} Fork files; {ownDocs.Count} VSlices authored files");
foreach (var doc in ownDocs)
    await Scan(doc, "VSlices/" + Path.GetFileName(doc.FilePath));
Console.WriteLine($"Directly used fork files: {selected.Count}; expanding dependency closure");
while (queue.Count > 0)
{
    var next = queue.Dequeue();
    await Scan(sourceDocs[next], next);
    if (analyzed % 60 == 0)
        Console.WriteLine($"Analyzed {analyzed} files; selected {selected.Count}; queued {queue.Count}");
}
if (selected.Count == 0) throw new InvalidOperationException("Closure empty: likely missing Roslyn symbol binding.");

Directory.CreateDirectory(Path.GetDirectoryName(outFile)!);
await File.WriteAllLinesAsync(outFile, selected.OrderBy(s => s, StringComparer.Ordinal));
var summary = new {
    upstreamFiles = sourceDocs.Count,
    neededFiles = selected.Count,
    filesExcluded = sourceDocs.Count - selected.Count,
    scannedDocuments = analyzed,
    symbolsResolved = bindings,
    sourceSnapshot = "HernanFAR/language-ext submodule pinned by VSlices",
    remainingWarnings = loadWarnings
};
await File.WriteAllTextAsync(Path.ChangeExtension(outFile, ".json"),
    JsonSerializer.Serialize(summary, new JsonSerializerOptions { WriteIndented = true }));
await File.WriteAllLinesAsync(Path.Combine(Path.GetDirectoryName(outFile)!, "source-dependency-edges.tsv"),
    new[] { "from\tto\treason" }.Concat(edges.OrderBy(s => s)));
Console.WriteLine(JsonSerializer.Serialize(summary));
