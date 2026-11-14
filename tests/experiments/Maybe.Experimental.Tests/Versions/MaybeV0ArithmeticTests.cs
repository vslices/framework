using System.Numerics;
using Maybe.Experimental.MaybeV0;
using Maybe.Experimental.Tests.Contracts;

namespace Maybe.Experimental.Tests.Versions;

public abstract class MaybeV0ArithmeticContractTests<T>
    : MaybeArithmeticContractTests<MaybeV0<T>, T>
    where T : INumber<T>
{
    protected override MaybeV0<T> Some(T value) => new SomeV0<T>(value);
    protected override MaybeV0<T> None() => new NoneV0<T>();
    protected override bool IsNone(MaybeV0<T> value) => value is NoneV0<T>;

    protected override T ReadSome(MaybeV0<T> value) => value switch
    {
        SomeV0<T> some => some.Value,
        _ => throw new InvalidOperationException("Expected Some in a successful operation")
    };

    protected override MaybeV0<T> Combine(Func<T, T, T> operation, params MaybeV0<T>[] operands)
        => MaybeV0Arithmetic.Combine(operation, operands);
}

public sealed class MaybeV0IntArithmeticTests : MaybeV0ArithmeticContractTests<int>;

public sealed class MaybeV0DecimalArithmeticTests : MaybeV0ArithmeticContractTests<decimal>;

public sealed class MaybeV0LongArithmeticTests : MaybeV0ArithmeticContractTests<long>;
