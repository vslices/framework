using LanguageExt;
using VSlices;
using VSlices.Monads;
using Xunit;
using static VSlices.VSlicesPrelude;

namespace VSlices.Work.Tests;

public sealed class FlowDependencyCompositionTests
{
    [Fact]
    public async Task Derive_can_promote_to_Flow_from_the_return_type()
    {
        var result = await DeriveOnly()
            .RunFlow(new TestAlgebra(10), new TestRequest(5))
            .RunAsync();

        Assert.Equal(5, result);
    }

    [Fact]
    public async Task Compute_can_promote_to_Flow_from_the_return_type()
    {
        var result = await ComputeOnly()
            .RunFlow(new TestAlgebra(10), new TestRequest(5))
            .RunAsync();

        Assert.Equal(10, result);
    }

    [Fact]
    public async Task Derive_then_compute_joins_into_Flow()
    {
        var result = await DeriveThenCompute()
            .RunFlow(new TestAlgebra(10), new TestRequest(5))
            .RunAsync();

        Assert.Equal(15, result);
    }

    [Fact]
    public async Task Compute_then_derive_joins_into_Flow()
    {
        var result = await ComputeThenDerive()
            .RunFlow(new TestAlgebra(10), new TestRequest(5))
            .RunAsync();

        Assert.Equal(15, result);
    }

    [Fact]
    public async Task Same_dependency_composition_stays_partial_until_promoted()
    {
        var derived = await DeriveThenDerive()
            .RunFlow(new TestAlgebra(10), new TestRequest(5))
            .RunAsync();

        var computed = await ComputeThenCompute()
            .RunFlow(new TestAlgebra(10), new TestRequest(5))
            .RunAsync();

        Assert.Equal(15, derived);
        Assert.Equal(21, computed);
    }

    [Fact]
    public async Task Once_dependencies_join_following_partial_steps_remain_Flow()
    {
        var result = await JoinedThenDerivedThenComputed()
            .RunFlow(new TestAlgebra(10), new TestRequest(5))
            .RunAsync();

        Assert.Equal(30, result);
    }

    private static Flow<TestAlgebra, TestRequest, int> DeriveOnly() =>
        derive((TestRequest request) => request.Value);

    private static Flow<TestAlgebra, TestRequest, int> ComputeOnly() =>
        compute((TestAlgebra algebra) => IO.pure(algebra.Offset));

    private static Flow<TestAlgebra, TestRequest, int> DeriveThenCompute() =>
        from value in derive((TestRequest request) => request.Value)
        from result in compute((TestAlgebra algebra) => IO.pure(value + algebra.Offset))
        select result;

    private static Flow<TestAlgebra, TestRequest, int> ComputeThenDerive() =>
        from offset in compute((TestAlgebra algebra) => IO.pure(algebra.Offset))
        from value in derive((TestRequest request) => request.Value)
        select offset + value;

    private static Flow<TestAlgebra, TestRequest, int> DeriveThenDerive() =>
        from value in derive((TestRequest request) => request.Value)
        from doubled in derive((TestRequest request) => request.Value * 2)
        select value + doubled;

    private static Flow<TestAlgebra, TestRequest, int> ComputeThenCompute() =>
        from offset in compute((TestAlgebra algebra) => IO.pure(algebra.Offset))
        from next in compute((TestAlgebra algebra) => IO.pure(algebra.Offset + 1))
        select offset + next;

    private static Flow<TestAlgebra, TestRequest, int> JoinedThenDerivedThenComputed() =>
        from value in derive((TestRequest request) => request.Value)
        from offset in compute((TestAlgebra algebra) => IO.pure(algebra.Offset))
        from again in derive((TestRequest request) => request.Value)
        from final in compute((TestAlgebra algebra) => IO.pure(algebra.Offset))
        select value + offset + again + final;

    private sealed record TestRequest(int Value);
    private sealed record TestAlgebra(int Offset);
}
