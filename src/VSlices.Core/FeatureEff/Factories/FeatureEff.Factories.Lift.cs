using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial class FeatureEff
{
    public static FeatureEff<RT, A> Lift<RT, A>(Func<RT, A> func) => 
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<RT, Pure<A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<RT, FeatureError> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<RT, Fail<FeatureError>> func) => 
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<RT, Exceptional> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<RT, Fail<Exceptional>> func) => 
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<RT, Either<FeatureError, A>> func) => 
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<RT, Fin<A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<RT, Either<Exceptional, A>> func) => 
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<RT, Fin<Either<FeatureError, A>>> func) => 
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<A> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<Pure<A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<FeatureError> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<Fail<FeatureError>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<Exceptional> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<Fail<Exceptional>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<Either<FeatureError, A>> func) => 
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<Fin<A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<Either<Exceptional, A>> func) =>
        FeatureEff<RT, A>.Lift(func);

    public static FeatureEff<RT, A> Lift<RT, A>(Func<Fin<Either<FeatureError, A>>> func) => 
        FeatureEff<RT, A>.Lift(func);

}
