using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial class FeatureEff<RT>
{
    public static FeatureEff<RT, A> Lift<A>(Func<RT, A> func) => 
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<RT, Pure<A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<RT, FeatureError> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<RT, Fail<FeatureError>> func) => 
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<RT, Exceptional> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<RT, Fail<Exceptional>> func) => 
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<RT, Either<FeatureError, A>> func) => 
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<RT, Fin<A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<RT, Either<Exceptional, A>> func) => 
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<RT, Fin<Either<FeatureError, A>>> func) => 
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<A> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<Pure<A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<FeatureError> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<Fail<FeatureError>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<Exceptional> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<Fail<Exceptional>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<Either<FeatureError, A>> func) => 
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<Fin<A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<Either<Exceptional, A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<A>(Func<Fin<Either<FeatureError, A>>> func) => 
        FeatureEff<RT, A>.Lift(func);

}
