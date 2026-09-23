using LanguageExt;
using LanguageExt.Traits;
using VSlices.Work;
using Xunit;

namespace VSlices.Work.Tests;

public sealed class AlgebraSumArityTests
{
    [Fact]
    public async Task Seven_way_sum_hoists_and_interprets_A_through_G()
    {
        var interpreter =
            new AlgebraSumIO<
                ValueAlgebra<ATag>,
                ValueAlgebra<BTag>,
                ValueAlgebra<CTag>,
                ValueAlgebra<DTag>,
                ValueAlgebra<ETag>,
                ValueAlgebra<FTag>,
                ValueAlgebra<GTag>>(
                    new ValueInterpreter<ATag>(),
                    new ValueInterpreter<BTag>(),
                    new ValueInterpreter<CTag>(),
                    new ValueInterpreter<DTag>(),
                    new ValueInterpreter<ETag>(),
                    new ValueInterpreter<FTag>(),
                    new ValueInterpreter<GTag>());

        var program =
            from a in AlgebraSum<
                ValueAlgebra<ATag>,
                ValueAlgebra<BTag>,
                ValueAlgebra<CTag>,
                ValueAlgebra<DTag>,
                ValueAlgebra<ETag>,
                ValueAlgebra<FTag>,
                ValueAlgebra<GTag>>
                .FromA(ValueAlgebra<ATag>.Value(1))
            from b in AlgebraSum<
                ValueAlgebra<ATag>,
                ValueAlgebra<BTag>,
                ValueAlgebra<CTag>,
                ValueAlgebra<DTag>,
                ValueAlgebra<ETag>,
                ValueAlgebra<FTag>,
                ValueAlgebra<GTag>>
                .FromB(ValueAlgebra<BTag>.Value(2))
            from c in AlgebraSum<
                ValueAlgebra<ATag>,
                ValueAlgebra<BTag>,
                ValueAlgebra<CTag>,
                ValueAlgebra<DTag>,
                ValueAlgebra<ETag>,
                ValueAlgebra<FTag>,
                ValueAlgebra<GTag>>
                .FromC(ValueAlgebra<CTag>.Value(3))
            from d in AlgebraSum<
                ValueAlgebra<ATag>,
                ValueAlgebra<BTag>,
                ValueAlgebra<CTag>,
                ValueAlgebra<DTag>,
                ValueAlgebra<ETag>,
                ValueAlgebra<FTag>,
                ValueAlgebra<GTag>>
                .FromD(ValueAlgebra<DTag>.Value(4))
            from e in AlgebraSum<
                ValueAlgebra<ATag>,
                ValueAlgebra<BTag>,
                ValueAlgebra<CTag>,
                ValueAlgebra<DTag>,
                ValueAlgebra<ETag>,
                ValueAlgebra<FTag>,
                ValueAlgebra<GTag>>
                .FromE(ValueAlgebra<ETag>.Value(5))
            from f in AlgebraSum<
                ValueAlgebra<ATag>,
                ValueAlgebra<BTag>,
                ValueAlgebra<CTag>,
                ValueAlgebra<DTag>,
                ValueAlgebra<ETag>,
                ValueAlgebra<FTag>,
                ValueAlgebra<GTag>>
                .FromF(ValueAlgebra<FTag>.Value(6))
            from g in AlgebraSum<
                ValueAlgebra<ATag>,
                ValueAlgebra<BTag>,
                ValueAlgebra<CTag>,
                ValueAlgebra<DTag>,
                ValueAlgebra<ETag>,
                ValueAlgebra<FTag>,
                ValueAlgebra<GTag>>
                .FromG(ValueAlgebra<GTag>.Value(7))
            select a + b + c + d + e + f + g;

        var result = await FreeAlgebra
            .interpret(program, interpreter)
            .RunAsync();

        Assert.Equal(28, result);
    }
}

public sealed record ValueOperation<TTag, T>(
    int Value,
    Func<int, T> Next)
    : K<ValueAlgebra<TTag>, T>;

public sealed class ValueAlgebra<TTag> :
    Functor<ValueAlgebra<TTag>>
{
    public static Free<ValueAlgebra<TTag>, int> Value(int value) =>
        Free.lift<ValueAlgebra<TTag>, int>(
            new ValueOperation<TTag, int>(
                value,
                static current => current));

    static K<ValueAlgebra<TTag>, Y>
        Functor<ValueAlgebra<TTag>>.Map<X, Y>(
            Func<X, Y> f,
            K<ValueAlgebra<TTag>, X> ma) =>
        ma switch
        {
            ValueOperation<TTag, X>(var value, var next) =>
                new ValueOperation<TTag, Y>(
                    value,
                    current => f(next(current))),
            _ => throw new NotSupportedException()
        };
}

public sealed class ValueInterpreter<TTag> :
    AlgebraIO<ValueAlgebra<TTag>>
{
    public IO<T> Interpret<T>(
        K<ValueAlgebra<TTag>, T> operation) =>
        operation switch
        {
            ValueOperation<TTag, T>(var value, var next) =>
                IO.pure(next(value)),
            _ => throw new NotSupportedException()
        };
}

public sealed class ATag;
public sealed class BTag;
public sealed class CTag;
public sealed class DTag;
public sealed class ETag;
public sealed class FTag;
public sealed class GTag;
