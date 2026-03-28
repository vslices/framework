using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial class FeatureEff<RT>
{
    public static FeatureEff<RT, A> Lift<A>(Eff<RT, A> m) =>
        FeatureEff<RT, A>.Lift(m);

    public static FeatureEff<RT, A> Lift<A>(Eff<A> m) =>
        FeatureEff<RT, A>.Lift(m);

    public static FeatureEff<RT, A> LiftIO<A>(IO<A> m) =>
        FeatureEff<RT, A>.LiftIO(m);

    public static FeatureEff<RT, A> Lift<A>(Fin<A> m) =>
        FeatureEff<RT, A>.Lift(m);

    public static FeatureEff<RT, A> Lift<A>(Either<FeatureError, A> m) =>
        FeatureEff<RT, A>.Lift(m);

    public static FeatureEff<RT, A> Lift<A>(Either<Exceptional, A> m) =>
        FeatureEff<RT, A>.Lift(m);

    public static FeatureEff<RT, A> Lift<A>(Eff<RT, Either<FeatureError, A>> m) =>
        FeatureEff<RT, A>.Lift(m);

    public static FeatureEff<RT, A> Lift<A>(Fin<Either<FeatureError, A>> m) =>
        FeatureEff<RT, A>.Lift(m);

    public static FeatureEff<RT, A> Lift<A>(Eff<RT, K<Either<FeatureError>, A>> m) =>
        FeatureEff<RT, A>.Lift(m);

    public static FeatureEff<RT, A> Lift<A>(Fin<K<Either<FeatureError>, A>> m) =>
        FeatureEff<RT, A>.Lift(m);
}
