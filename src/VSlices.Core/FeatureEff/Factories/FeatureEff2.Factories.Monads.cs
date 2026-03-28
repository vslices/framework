using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    public static FeatureEff<RT, A> Lift(Eff<RT, A> m) => new(m);

    public static FeatureEff<RT, A> Lift(Eff<A> m) => new(m);

    public static FeatureEff<RT, A> LiftIO(IO<A> m) => new(m);

    public static FeatureEff<RT, A> Lift(Fin<A> m) => new(m);

    public static FeatureEff<RT, A> Lift(Either<FeatureError, A> m) => 
        new(m);

    public static FeatureEff<RT, A> Lift(Either<Exceptional, A> m) =>
        new(m);

    public static FeatureEff<RT, A> Lift(Eff<RT, Either<FeatureError, A>> m) =>
        new(m);

    public static FeatureEff<RT, A> Lift(Fin<Either<FeatureError, A>> m) =>
        new(m);

    public static FeatureEff<RT, A> Lift(Eff<RT, K<Either<FeatureError>, A>> m) =>
        new(m);

    public static FeatureEff<RT, A> Lift(Fin<K<Either<FeatureError>, A>> m) =>
        new(m);
}
