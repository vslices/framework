using VSlices;
using VSlices.Monads;
using VSlices.Work;
using Xunit;

namespace VSlices.Work.Tests;

public sealed class AlgebraSumArityTests
{
    [Fact]
    public async Task Seven_way_sum_projects_each_child_runtime_into_Flow()
    {
        var algebra = new AlgebraSum<
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
            ValueFlow<ATag>().MapRuntime<AlgebraSum<ValueAlgebra<ATag>, ValueAlgebra<BTag>, ValueAlgebra<CTag>, ValueAlgebra<DTag>, ValueAlgebra<ETag>, ValueAlgebra<FTag>, ValueAlgebra<GTag>>>(x => x.A)
            .Bind(a => ValueFlow<BTag>().MapRuntime<AlgebraSum<ValueAlgebra<ATag>, ValueAlgebra<BTag>, ValueAlgebra<CTag>, ValueAlgebra<DTag>, ValueAlgebra<ETag>, ValueAlgebra<FTag>, ValueAlgebra<GTag>>>(x => x.B)
            .Bind(b => ValueFlow<CTag>().MapRuntime<AlgebraSum<ValueAlgebra<ATag>, ValueAlgebra<BTag>, ValueAlgebra<CTag>, ValueAlgebra<DTag>, ValueAlgebra<ETag>, ValueAlgebra<FTag>, ValueAlgebra<GTag>>>(x => x.C)
            .Bind(c => ValueFlow<DTag>().MapRuntime<AlgebraSum<ValueAlgebra<ATag>, ValueAlgebra<BTag>, ValueAlgebra<CTag>, ValueAlgebra<DTag>, ValueAlgebra<ETag>, ValueAlgebra<FTag>, ValueAlgebra<GTag>>>(x => x.D)
            .Bind(d => ValueFlow<ETag>().MapRuntime<AlgebraSum<ValueAlgebra<ATag>, ValueAlgebra<BTag>, ValueAlgebra<CTag>, ValueAlgebra<DTag>, ValueAlgebra<ETag>, ValueAlgebra<FTag>, ValueAlgebra<GTag>>>(x => x.E)
            .Bind(e => ValueFlow<FTag>().MapRuntime<AlgebraSum<ValueAlgebra<ATag>, ValueAlgebra<BTag>, ValueAlgebra<CTag>, ValueAlgebra<DTag>, ValueAlgebra<ETag>, ValueAlgebra<FTag>, ValueAlgebra<GTag>>>(x => x.F)
            .Bind(f => ValueFlow<GTag>().MapRuntime<AlgebraSum<ValueAlgebra<ATag>, ValueAlgebra<BTag>, ValueAlgebra<CTag>, ValueAlgebra<DTag>, ValueAlgebra<ETag>, ValueAlgebra<FTag>, ValueAlgebra<GTag>>>(x => x.G)
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
