using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    public static FeatureEff<RT, A> Success(A value) =>
        new(value);

    public static FeatureEff<RT, A> Success(Pure<A> value) =>
        new(value);

    public static FeatureEff<RT, A> Pure(A value) =>
        Success(value);

    public static FeatureEff<RT, A> Pure(Pure<A> value) =>
        Success(value);

    public static FeatureEff<RT, A> Fail(Exceptional error) =>
        new(error);

    public static FeatureEff<RT, A> Fail(Fail<Exceptional> error) =>
        new(error);

    public static FeatureEff<RT, A> Fail(FeatureError error) =>
        new(error);

    public static FeatureEff<RT, A> Fail(Fail<FeatureError> error) =>
        new(error);
}
