using Maybe.Experimental.MaybeV0;
using Maybe.Experimental.Tests.Contracts;

namespace Maybe.Experimental.Tests.Versions;

public sealed class MaybeV0Tests : MaybeContractTests<MaybeV0<int>>
{
    protected override MaybeV0<int> Some(int value) => new SomeV0<int>(value);

    protected override MaybeV0<int> None() => new NoneV0<int>();

    protected override bool IsSome(MaybeV0<int> value) => value is SomeV0<int>;

    protected override bool IsNone(MaybeV0<int> value) => value is NoneV0<int>;

    protected override int ReadSome(MaybeV0<int> value) => value switch
    {
        SomeV0<int> some => some.Value,
        _ => throw new InvalidOperationException("Expected Some for this contract assertion")
    };
}
