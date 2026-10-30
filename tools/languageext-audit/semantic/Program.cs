using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

var argsList = args.ToList();
string ReadArg(string key, string defaultValue) =>
    argsList.IndexOf(key) is var i && i >= 0 && i + 1 < argsList.Count ? argsList[i + 1] : defaultValue;
var root = Path.GetFullPath(ReadArg("--root", "."));
var output = Path.GetFullPath(ReadArg("--out", "docs/audits/languageext-2026-10/semantic"));
Directory.CreateDirectory(output);
MSBuildLocator.RegisterDefaults();
using var workspace = MSBuildWorkspace.Create();
var loadErrors = new List<string>();
workspace.WorkspaceFailed += (_, e) => loadErrors.Add(e.Diagnostic.ToString());

var projects = Directory.EnumerateFiles(root, "*.csproj", SearchOption.AllDirectories)
    .Where(p => !p.Split(Path.DirectorySeparatorChar).Any(x => x is "obj" or "bin" or ".git" or "language-ext"))
    .Where(p => !p.Contains("/tools/languageext-audit/semantic/") &&
                !p.Contains("\\tools\\languageext-audit\\semantic\\"))
    .OrderBy(p => p).ToArray();
var results = new List<Reference>();
var compilationErrors = new List<string>();
var analyzed = new List<string>();
var projectFailures = new List<string>();
foreach (var path in projects)
{
    Project project;
    try {
        project = workspace.CurrentSolution.Projects.FirstOrDefault(p =>
            string.Equals(Path.GetFullPath(p.FilePath ?? ""), Path.GetFullPath(path),
                StringComparison.OrdinalIgnoreCase))
            ?? await workspace.OpenProjectAsync(path);
    }
    catch (Exception e) { projectFailures.Add(path + ": " + e); continue; }
    var compilation = await project.GetCompilationAsync();
    if (compilation is null) { projectFailures.Add(path + ": null compilation"); continue; }
    if (!project.Name.Contains("Invalid", StringComparison.OrdinalIgnoreCase))
        compilationErrors.AddRange(compilation.GetDiagnostics()
        .Where(d => d.Severity == DiagnosticSeverity.Error).Take(80)
        .Select(d => project.Name + ": " + d.ToString()));
    Console.WriteLine($"Analyzing {project.Name} ({project.Documents.Count()} documents)");
    foreach (var doc in project.Documents.Where(d => d.SourceCodeKind == SourceCodeKind.Regular))
    {
        var tree = await doc.GetSyntaxTreeAsync();
        if (tree is null) continue;
        var model = compilation.GetSemanticModel(tree);
        var rootNode = await tree.GetRootAsync();
        var filepath = Path.GetRelativePath(root, doc.FilePath ?? doc.Name).Replace('\\','/');
        analyzed.Add(project.Name + ":" + filepath);
        foreach (var node in rootNode.DescendantNodesAndSelf())
        {
            if (node is not (NameSyntax or InvocationExpressionSyntax or
                ObjectCreationExpressionSyntax or MemberAccessExpressionSyntax or
                OperatorDeclarationSyntax or ConversionOperatorDeclarationSyntax or
                BinaryExpressionSyntax or AssignmentExpressionSyntax or PrefixUnaryExpressionSyntax or
                PostfixUnaryExpressionSyntax or ElementAccessExpressionSyntax or QueryExpressionSyntax or
                AttributeSyntax or VariableDeclaratorSyntax)) continue;
            if (node is NameSyntax n && n.Parent is NameSyntax) continue;
            var info = model.GetSymbolInfo(node);
            var symbol = info.Symbol ?? info.CandidateSymbols.FirstOrDefault();
            if (symbol is not null) Add(symbol, node, project.Name, filepath);
            if (node is InvocationExpressionSyntax call)
            {
                var inferred = model.GetTypeInfo(call).Type;
                if (inferred is not null && symbol is null) Add(inferred, call, project.Name, filepath);
            }
            if (node is VariableDeclaratorSyntax variable && variable.Parent?.Parent is VariableDeclarationSyntax declaration &&
                declaration.Type.IsVar)
            {
                var inferred = model.GetTypeInfo(declaration.Type).Type;
                if (inferred is not null) Add(inferred, declaration.Type, project.Name, filepath);
            }
            if (node is ExpressionSyntax expr && node is not InvocationExpressionSyntax)
            {
                var conversion = model.GetConversion(expr);
                if (conversion.IsUserDefined && conversion.MethodSymbol is { } method)
                    Add(method, expr, project.Name, filepath, "conversion");
            }
        }
        void Add(ISymbol symbol, SyntaxNode node, string projectName, string file, string? kindOverride = null)
        {
            var original = symbol.OriginalDefinition;
            var assembly = original.ContainingAssembly?.Identity.Name ?? "";
            if (!assembly.StartsWith("LanguageExt", StringComparison.OrdinalIgnoreCase)) return;
            var loc = tree.GetLineSpan(node.Span).StartLinePosition;
            results.Add(new Reference(projectName, file, loc.Line+1, loc.Character+1,
                kindOverride ?? symbol.Kind.ToString(), symbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat),
                original.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), assembly,
                node.ToString().Replace('\n',' ').Replace('\r',' ').Trim()[..Math.Min(240,node.ToString().Length)]));
        }
    }
}
results = results.Where(r => !r.File.StartsWith("language-ext/", StringComparison.OrdinalIgnoreCase)).ToList();
results = results.Distinct().OrderBy(x => x.Project).ThenBy(x => x.File)
    .ThenBy(x => x.Line).ThenBy(x => x.Column).ThenBy(x => x.Symbol).ToList();
static string Escape(string? s) => "\"" + (s ?? "").Replace("\"","\"\"") + "\"";
var sb = new StringBuilder("project,file,line,column,kind,symbol,definition,assembly,expression\n");
foreach (var r in results) sb.AppendLine(string.Join(",", new[]{r.Project,r.File,r.Line.ToString(),
    r.Column.ToString(),r.Kind,r.Symbol,r.Definition,r.Assembly,r.Expression}.Select(Escape)));
await File.WriteAllTextAsync(Path.Combine(output,"symbols.csv"),sb.ToString());
var summary = new {
    scannedProjectFiles=projects.Length, loadedProjects=projects.Length-projectFailures.Count,
    analyzedDocuments=analyzed.Count, attributedReferences=results.Count,
    assemblies=results.GroupBy(x=>x.Assembly).ToDictionary(g=>g.Key,g=>g.Count()),
    topSymbols=results.GroupBy(x=>x.Definition).OrderByDescending(g=>g.Count()).Take(80)
        .Select(g=>new {symbol=g.Key,locations=g.Count()}).ToList(),
    projects=results.GroupBy(x=>x.Project).ToDictionary(g=>g.Key,g=>g.Count()),
    loadErrors, projectFailures, compilationErrors,
    analyzedDocumentsList=analyzed
};
await File.WriteAllTextAsync(Path.Combine(output,"summary.json"),
    JsonSerializer.Serialize(summary,new JsonSerializerOptions {WriteIndented=true}));
Console.WriteLine($"Projects: {projects.Length}; documents: {analyzed.Count}; references: {results.Count}; failures: {projectFailures.Count}; compilation errors: {compilationErrors.Count}");
if (projectFailures.Count>0 || compilationErrors.Count>0 || loadErrors.Any(x=>x.Contains("Failure",StringComparison.OrdinalIgnoreCase)))
    Environment.ExitCode=2;
record Reference(string Project,string File,int Line,int Column,string Kind,string Symbol,string Definition,string Assembly,string Expression);
