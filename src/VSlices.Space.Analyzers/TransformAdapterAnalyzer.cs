using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace VSlices.Space.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class TransformAdapterAnalyzer : DiagnosticAnalyzer
{
    public const string MissingInputId = "VST001";
    public const string InvalidInputConstructorId = "VST002";
    public const string MissingTransformationId = "VST003";

    private const string TransformAdapterNamespace = "VSlices.Space.Traits";
    private const string ViewsNamespace = "VSlices.Views.Abstract.Forms";

    private static readonly DiagnosticDescriptor MissingInput = new(
        MissingInputId,
        "TransformAdapter target requires canonical Input",
        "Type '{0}' must declare a nested type named 'Input' to be adapted from '{1}'",
        "VSlices.Transforms",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor InvalidInputConstructor = new(
        InvalidInputConstructorId,
        "TransformAdapter Input constructor is incompatible",
        "Type '{0}.Input' must expose exactly one non-empty public instance constructor whose only parameter is '{1}'",
        "VSlices.Transforms",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor MissingTransformation = new(
        MissingTransformationId,
        "TransformAdapter target does not own the canonical Input transformation",
        "Type '{0}' must implement Transformable<{0}.Input, {0}>",
        "VSlices.Transforms",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        [MissingInput, InvalidInputConstructor, MissingTransformation];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(
            GeneratedCodeAnalysisFlags.Analyze |
            GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(
            AnalyzeGenericName,
            SyntaxKind.GenericName);
    }

    private static void AnalyzeGenericName(
        SyntaxNodeAnalysisContext context)
    {
        var syntax = (GenericNameSyntax)context.Node;
        var type = context.SemanticModel.GetTypeInfo(syntax).Type as INamedTypeSymbol;

        if (type is null)
            return;

        var definition = type.OriginalDefinition;

        if (IsType(
                definition,
                TransformAdapterNamespace,
                "TransformAdapter",
                arity: 2))
        {
            Validate(
                context,
                syntax.GetLocation(),
                source: type.TypeArguments[0],
                target: type.TypeArguments[1]);

            return;
        }

        if (IsType(
                definition,
                ViewsNamespace,
                "vTextInput",
                arity: 1))
        {
            Validate(
                context,
                syntax.GetLocation(),
                source: context.Compilation.GetSpecialType(
                    SpecialType.System_String),
                target: type.TypeArguments[0]);
        }
    }

    private static void Validate(
        SyntaxNodeAnalysisContext context,
        Location location,
        ITypeSymbol source,
        ITypeSymbol target)
    {
        if (source.TypeKind == TypeKind.TypeParameter ||
            target.TypeKind == TypeKind.TypeParameter ||
            target is not INamedTypeSymbol targetType)
        {
            return;
        }

        var inputTypes = targetType
            .GetTypeMembers("Input")
            .Where(static type => type.Arity == 0)
            .ToArray();

        if (inputTypes.Length != 1)
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    MissingInput,
                    location,
                    targetType.ToDisplayString(),
                    source.ToDisplayString()));

            return;
        }

        var input = inputTypes[0];

        var publicConstructors = input.InstanceConstructors
            .Where(static constructor =>
                constructor.DeclaredAccessibility ==
                    Accessibility.Public &&
                constructor.Parameters.Length > 0)
            .ToArray();

        if (publicConstructors.Length != 1 ||
            publicConstructors[0].Parameters.Length != 1 ||
            !SymbolEqualityComparer.Default.Equals(
                publicConstructors[0].Parameters[0].Type,
                source))
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    InvalidInputConstructor,
                    location,
                    targetType.ToDisplayString(),
                    source.ToDisplayString()));

            return;
        }

        var transformable = context.Compilation.GetTypeByMetadataName(
            "VSlices.Space.Traits.Transformable`2");

        if (transformable is null)
            return;

        var expected = transformable.Construct(
            input,
            targetType);

        if (!targetType.AllInterfaces.Any(
                implemented =>
                    SymbolEqualityComparer.Default.Equals(
                        implemented,
                        expected)))
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    MissingTransformation,
                    location,
                    targetType.ToDisplayString()));
        }
    }

    private static bool IsType(
        INamedTypeSymbol type,
        string @namespace,
        string name,
        int arity) =>
        type.Arity == arity &&
        type.Name == name &&
        type.ContainingNamespace.ToDisplayString() == @namespace;
}
