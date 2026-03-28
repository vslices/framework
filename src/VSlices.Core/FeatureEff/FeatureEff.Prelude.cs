using VSlices.Core;
using VSlices.Core.Errors;

namespace VSlices;

public static partial class VSlicesCorePrelude
{
    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, A> func) =>
        FeatureEff < RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Pure<A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, FeatureError> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Fail<FeatureError>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Exceptional> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Fail<Exceptional>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Either<FeatureError, A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Fin<A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Either<Exceptional, A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Fin<Either<FeatureError, A>>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<A> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<Pure<A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<FeatureError> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<Fail<FeatureError>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<Exceptional> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<Fail<Exceptional>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<Either<FeatureError, A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<Fin<A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<Either<Exceptional, A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<Fin<Either<FeatureError, A>>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Task<A>> func) =>
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Task<Pure<A>>> func) =>
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Task<FeatureError>> func) =>
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Task<Fail<FeatureError>>> func) =>
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Task<Exceptional>> func) =>
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Task<Fail<Exceptional>>> func) =>
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Task<Either<FeatureError, A>>> func) =>
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Task<Either<Exceptional, A>>> func) =>
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Task<Fin<A>>> func) =>
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> liftFeat<RT, A>(Func<RT, Task<Fin<Either<FeatureError, A>>>> func) =>
        FeatureEff<RT, A>.LiftIO(func);
}
