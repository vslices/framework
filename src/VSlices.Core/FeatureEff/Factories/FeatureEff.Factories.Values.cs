using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial class FeatureEff
{
    public static FeatureEff<RT, A> Success<RT, A>(A value) =>
        FeatureEff<RT, A>.Success(value);

    public static FeatureEff<RT, A> Success<RT, A>(Pure<A> value) =>
        FeatureEff<RT, A>.Success(value);

    public static FeatureEff<RT, A> Pure<RT, A>(A value) =>
        Success<RT, A>(value);

    public static FeatureEff<RT, A> Pure<RT, A>(Pure<A> value) =>
        Success<RT, A>(value);

    public static FeatureEff<RT, A> Fail<RT, A>(Exceptional error) =>
        FeatureEff<RT, A>.Fail(error);

    public static FeatureEff<RT, A> Fail<RT, A>(Fail<Exceptional> error) =>
        FeatureEff<RT, A>.Fail(error);

    public static FeatureEff<RT, A> Fail<RT, A>(FeatureError error) =>
        FeatureEff<RT, A>.Fail(error);

    public static FeatureEff<RT, A> Fail<RT, A>(Fail<FeatureError> error) =>
        FeatureEff<RT, A>.Fail(error);
}
