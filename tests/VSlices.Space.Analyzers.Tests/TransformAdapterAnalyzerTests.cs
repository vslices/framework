using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using VSlices.Space.Analyzers;
using Xunit;

namespace VSlices.Space.Analyzers.Tests;

public sealed class TransformAdapterAnalyzerTests
{
    [Fact]
    public async Task Valid_adapter_convention_has_no_diagnostics()
    {
        var diagnostics = await Analyze(
            """
            using VSlices.Space.Traits;

            namespace VSlices.Space.Traits
            {
                public sealed class TransformAdapter<A, B> { }
                public interface Transformable<A, B> { }
            }

            namespace Probe
            {
                public sealed class EmailAddress :
                    Transformable<EmailAddress.Input, EmailAddress>
                {
                    public readonly record struct Input(string Value);
                }

                public sealed class Usage
                {
                    private TransformAdapter<string, EmailAddress>? adapter;
                }
            }
            """);

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task Missing_Input_is_reported()
    {
        var diagnostics = await Analyze(
            """
            using VSlices.Space.Traits;

            namespace VSlices.Space.Traits
            {
                public sealed class TransformAdapter<A, B> { }
                public interface Transformable<A, B> { }
            }

            namespace Probe
            {
                public sealed class EmailAddress { }

                public sealed class Usage
                {
                    private TransformAdapter<string, EmailAddress>? adapter;
                }
            }
            """);

        var diagnostic = Assert.Single(diagnostics);
        Assert.Equal(
            TransformAdapterAnalyzer.MissingInputId,
            diagnostic.Id);
    }

    [Fact]
    public async Task Input_constructor_must_accept_exact_source_type()
    {
        var diagnostics = await Analyze(
            """
            using VSlices.Space.Traits;

            namespace VSlices.Space.Traits
            {
                public sealed class TransformAdapter<A, B> { }
                public interface Transformable<A, B> { }
            }

            namespace Probe
            {
                public sealed class EmailAddress :
                    Transformable<EmailAddress.Input, EmailAddress>
                {
                    public readonly record struct Input(int Value);
                }

                public sealed class Usage
                {
                    private TransformAdapter<string, EmailAddress>? adapter;
                }
            }
            """);

        var diagnostic = Assert.Single(diagnostics);
        Assert.Equal(
            TransformAdapterAnalyzer.InvalidInputConstructorId,
            diagnostic.Id);
    }

    [Fact]
    public async Task Target_must_own_the_canonical_Input_transformation()
    {
        var diagnostics = await Analyze(
            """
            using VSlices.Space.Traits;

            namespace VSlices.Space.Traits
            {
                public sealed class TransformAdapter<A, B> { }
                public interface Transformable<A, B> { }
            }

            namespace Probe
            {
                public sealed class EmailAddress
                {
                    public readonly record struct Input(string Value);
                }

                public sealed class Usage
                {
                    private TransformAdapter<string, EmailAddress>? adapter;
                }
            }
            """);

        var diagnostic = Assert.Single(diagnostics);
        Assert.Equal(
            TransformAdapterAnalyzer.MissingTransformationId,
            diagnostic.Id);
    }

    [Fact]
    public async Task Closed_vTextInput_is_validated_with_string_as_source()
    {
        var diagnostics = await Analyze(
            """
            namespace VSlices.Space.Traits
            {
                public sealed class TransformAdapter<A, B> { }
                public interface Transformable<A, B> { }
            }

            namespace VSlices.Views.Abstract.Forms
            {
                public abstract class vTextInput<T> { }
            }

            namespace Probe
            {
                using VSlices.Views.Abstract.Forms;

                public sealed class EmailAddress
                {
                    public readonly record struct Input(int Value);
                }

                public abstract class EmailInput :
                    vTextInput<EmailAddress>
                {
                }
            }
            """);

        var diagnostic = Assert.Single(diagnostics);
        Assert.Equal(
            TransformAdapterAnalyzer.InvalidInputConstructorId,
            diagnostic.Id);
    }

    private static async Task<ImmutableArray<Diagnostic>> Analyze(
        string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(
            source,
            new CSharpParseOptions(LanguageVersion.Preview));

        var references = ((string?)AppContext.GetData(
                "TRUSTED_PLATFORM_ASSEMBLIES")
            ?? string.Empty)
            .Split(
                Path.PathSeparator,
                StringSplitOptions.RemoveEmptyEntries)
            .Select(static path =>
                MetadataReference.CreateFromFile(path));

        var compilation = CSharpCompilation.Create(
            assemblyName: "TransformAdapterAnalyzerProbe",
            syntaxTrees: [syntaxTree],
            references: references,
            options: new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary));

        var analyzers = ImmutableArray.Create<DiagnosticAnalyzer>(
            new TransformAdapterAnalyzer());

        return await compilation
            .WithAnalyzers(analyzers)
            .GetAnalyzerDiagnosticsAsync();
    }
}
