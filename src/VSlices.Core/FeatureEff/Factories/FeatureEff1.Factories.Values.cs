using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial class FeatureEff<RT>
{
    public static FeatureEff<RT, A> Success<A>(A value) =>
        FeatureEff<RT, A>.Success(value);

    public static FeatureEff<RT, A> Success<A>(Pure<A> value) =>
        FeatureEff<RT, A>.Success(value);

    public static FeatureEff<RT, A> Pure<A>(A value) =>
        Success(value);

    public static FeatureEff<RT, A> Pure<A>(Pure<A> value) =>
        Success(value);

    public static FeatureEff<RT, A> Fail<A>(Exceptional error) =>
        FeatureEff<RT, A>.Fail(error);

    public static FeatureEff<RT, A> Fail<A>(Fail<Exceptional> error) =>
        FeatureEff<RT, A>.Fail(error);

    public static FeatureEff<RT, A> Fail<A>(FeatureError error) =>
        FeatureEff<RT, A>.Fail(error);

    public static FeatureEff<RT, A> Fail<A>(Fail<FeatureError> error) =>
        FeatureEff<RT, A>.Fail(error);
}
