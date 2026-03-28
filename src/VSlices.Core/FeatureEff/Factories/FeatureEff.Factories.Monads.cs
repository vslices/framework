using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial class FeatureEff
{
    public static FeatureEff<RT, A> Lift<RT, A>(Eff<RT, A> m) =>
        FeatureEff<RT>.Lift(m);

    public static FeatureEff<RT, A> Lift<RT, A>(Eff<A> m) =>
        FeatureEff<RT>.Lift(m);

    public static FeatureEff<RT, A> LiftIO<RT, A>(IO<A> m) =>
        FeatureEff<RT>.LiftIO(m);

    public static FeatureEff<RT, A> Lift<RT, A>(Fin<A> m) =>
        FeatureEff<RT>.Lift(m);

    public static FeatureEff<RT, A> Lift<RT, A>(Either<FeatureError, A> m) =>
        FeatureEff<RT>.Lift(m);

    public static FeatureEff<RT, A> Lift<RT, A>(Either<Exceptional, A> m) =>
        FeatureEff<RT>.Lift(m);

    public static FeatureEff<RT, A> Lift<RT, A>(Eff<RT, Either<FeatureError, A>> m) =>
        FeatureEff<RT>.Lift(m);

    public static FeatureEff<RT, A> Lift<RT, A>(Fin<Either<FeatureError, A>> m) =>
        FeatureEff<RT>.Lift(m);

    public static FeatureEff<RT, A> Lift<RT, A>(Eff<RT, K<Either<FeatureError>, A>> m) =>
        FeatureEff<RT>.Lift(m);

    public static FeatureEff<RT, A> Lift<RT, A>(Fin<K<Either<FeatureError>, A>> m) =>
        FeatureEff<RT>.Lift(m);
}
