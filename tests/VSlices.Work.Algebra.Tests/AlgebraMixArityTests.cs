using LanguageExt;
using VSlices;
using VSlices.Monads;
using VSlices.Work;
using Xunit;

namespace VSlices.Work.Tests;

public sealed class AlgebraMixArityTests
{
    [Fact]
    public async Task Seven_way_mix_projects_each_child_runtime_into_Flow()
    {
        var algebra = new AlgebraMix<
            ValueAlgebra<ATag>,
            ValueAlgebra<BTag>,
            ValueAlgebra<CTag>,
            ValueAlgebra<DTag>,
            ValueAlgebra<ETag>,
            ValueAlgebra<FTag>,
            ValueAlgebra<GTag>>(
                new(1),
                new(2),
                new(3),
                new(4),
                new(5),
                new(6),
                new(7));

        var flow =
            ValueFlow<ATag>().MapRuntime((AlgebraMix<ValueAlgebra<ATag>, ValueAlgebra<BTag>, ValueAlgebra<CTag>, ValueAlgebra<DTag>, ValueAlgebra<ETag>, ValueAlgebra<FTag>, ValueAlgebra<GTag>> mix) => mix.A)
            .Bind(a => ValueFlow<BTag>().MapRuntime((AlgebraMix<ValueAlgebra<ATag>, ValueAlgebra<BTag>, ValueAlgebra<CTag>, ValueAlgebra<DTag>, ValueAlgebra<ETag>, ValueAlgebra<FTag>, ValueAlgebra<GTag>> mix) => mix.B)
            .Bind(b => ValueFlow<CTag>().MapRuntime((AlgebraMix<ValueAlgebra<ATag>, ValueAlgebra<BTag>, ValueAlgebra<CTag>, ValueAlgebra<DTag>, ValueAlgebra<ETag>, ValueAlgebra<FTag>, ValueAlgebra<GTag>> mix) => mix.C)
            .Bind(c => ValueFlow<DTag>().MapRuntime((AlgebraMix<ValueAlgebra<ATag>, ValueAlgebra<BTag>, ValueAlgebra<CTag>, ValueAlgebra<DTag>, ValueAlgebra<ETag>, ValueAlgebra<FTag>, ValueAlgebra<GTag>> mix) => mix.D)
            .Bind(d => ValueFlow<ETag>().MapRuntime((AlgebraMix<ValueAlgebra<ATag>, ValueAlgebra<BTag>, ValueAlgebra<CTag>, ValueAlgebra<DTag>, ValueAlgebra<ETag>, ValueAlgebra<FTag>, ValueAlgebra<GTag>> mix) => mix.E)
            .Bind(e => ValueFlow<FTag>().MapRuntime((AlgebraMix<ValueAlgebra<ATag>, ValueAlgebra<BTag>, ValueAlgebra<CTag>, ValueAlgebra<DTag>, ValueAlgebra<ETag>, ValueAlgebra<FTag>, ValueAlgebra<GTag>> mix) => mix.F)
            .Bind(f => ValueFlow<GTag>().MapRuntime((AlgebraMix<ValueAlgebra<ATag>, ValueAlgebra<BTag>, ValueAlgebra<CTag>, ValueAlgebra<DTag>, ValueAlgebra<ETag>, ValueAlgebra<FTag>, ValueAlgebra<GTag>> mix) => mix.G)
                .Map(g => a + b + c + d + e + f + g)))))));

        var result = await flow.RunFlow(algebra, default(Unit)).RunAsync();

        Assert.Equal(28, result);
    }

    private static Flow<ValueAlgebra<TTag>, Unit, int> ValueFlow<TTag>() =>
        new((algebra, _) => IO.pure(algebra.Value));
}

public sealed record ValueAlgebra<TTag>(int Value);

public sealed class ATag;
public sealed class BTag;
public sealed class CTag;
public sealed class DTag;
public sealed class ETag;
public sealed class FTag;
public sealed class GTag;
