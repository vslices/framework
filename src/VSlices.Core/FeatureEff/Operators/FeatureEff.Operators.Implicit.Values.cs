using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    public static implicit operator FeatureEff<RT, A>(A value) =>
        Pure(value);

    public static implicit operator FeatureEff<RT, A>(Pure<A> value) =>
        Pure(value);

    public static implicit operator FeatureEff<RT, A>(Exceptional value) =>
        Fail(value);

    public static implicit operator FeatureEff<RT, A>(Fail<Exceptional> value) =>
        Fail(value.Value);

    public static implicit operator FeatureEff<RT, A>(FeatureError value) =>
        Fail(value);

    public static implicit operator FeatureEff<RT, A>(Fail<FeatureError> value) =>
        Fail(value.Value);
}
