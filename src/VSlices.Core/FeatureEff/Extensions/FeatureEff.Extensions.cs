using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    public Eff<RT, Either<FeatureError, A>> RunEff() =>
        Effect.Run().As();

    public IO<Either<FeatureError, A>> RunIO(RT rt) =>
        RunEff().RunIO(rt);

    public Fin<Either<FeatureError, A>> RunFeature(RT rt, EnvIO envIO) =>
        RunEff().Run(rt, envIO);

    public Task<Fin<Either<FeatureError, A>>> RunFeatureAsync(RT rt, EnvIO envIO) =>
        RunEff().RunAsync(rt, envIO);

    public Either<FeatureError, A> RunUnsafeFeature(RT rt, EnvIO envIO) =>
        RunEff().RunUnsafe(rt, envIO);

    public ValueTask<Either<FeatureError, A>> RunUnsafeFeatureAsync(RT rt, EnvIO envIO) =>
        RunEff().RunUnsafeAsync(rt, envIO);

    public QueryMatching<RT, A> MatchQueryResult() =>
        new(this);

    public ActionMatching<RT, A> MatchActionResult() =>
        new(this);
}

public static partial class FeatureEffExtensions
{
    extension<RT, A>(K<FeatureEff<RT>, A> ma)
    {
        public Eff<RT, Either<FeatureError, A>> RunEff() =>
            +ma.As().RunEff();

        public FeatureEff<RT, A> As() =>
            (FeatureEff<RT, A>)ma;
    }

    extension<RT, A>(FeatureEff<RT, FeatureEff<RT, A>> ma)
    {
        public FeatureEff<RT, A> Flatten() =>
            ma.Bind(identity);
    }
}
