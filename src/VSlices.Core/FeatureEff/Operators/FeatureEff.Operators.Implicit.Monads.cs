using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    public static implicit operator FeatureEff<RT, A>(Eff<A> value) =>
        new(value);

    public static implicit operator FeatureEff<RT, A>(Eff<RT, A> value) =>
        new(value);

    public static implicit operator FeatureEff<RT, A>(Fin<A> value) =>
        new(value);

    public static implicit operator FeatureEff<RT, A>(Either<FeatureError, A> value) =>
        new(value);

    public static implicit operator FeatureEff<RT, A>(Either<Exception, A> value) =>
        new(value);

    public static implicit operator FeatureEff<RT, A>(EitherT<FeatureError, Eff<RT>, A> value) =>
        new(value);

    public static implicit operator FeatureEff<RT, A>(EitherT<FeatureError, Eff, A> value) =>
        new(value);

}
