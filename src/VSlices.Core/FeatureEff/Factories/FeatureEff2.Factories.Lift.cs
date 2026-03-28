using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    public static FeatureEff<RT, A> Lift(Func<RT, A> func) => new(func);

    public static FeatureEff<RT, A> Lift(Func<RT, Pure<A>> func) => new(func);

    public static FeatureEff<RT, A> Lift(Func<RT, FeatureError> func) => new(func);

    public static FeatureEff<RT, A> Lift(Func<RT, Fail<FeatureError>> func) => 
        new(func);

    public static FeatureEff<RT, A> Lift(Func<RT, Exceptional> func) => new(func);

    public static FeatureEff<RT, A> Lift(Func<RT, Fail<Exceptional>> func) => 
        new(func);

    public static FeatureEff<RT, A> Lift(Func<RT, Either<FeatureError, A>> func) => 
        new(func);

    public static FeatureEff<RT, A> Lift(Func<RT, Fin<A>> func) => new(func);

    public static FeatureEff<RT, A> Lift(Func<RT, Either<Exceptional, A>> func) => 
        new(func);

    public static FeatureEff<RT, A> Lift(Func<RT, Fin<Either<FeatureError, A>>> func) => 
        new(func);

    public static FeatureEff<RT, A> Lift(Func<A> func) => new(func);

    public static FeatureEff<RT, A> Lift(Func<Pure<A>> func) => new(func);

    public static FeatureEff<RT, A> Lift(Func<FeatureError> func) => new(func);

    public static FeatureEff<RT, A> Lift(Func<Fail<FeatureError>> func) => new(func);

    public static FeatureEff<RT, A> Lift(Func<Exceptional> func) => new(func);

    public static FeatureEff<RT, A> Lift(Func<Fail<Exceptional>> func) => new(func);

    public static FeatureEff<RT, A> Lift(Func<Either<FeatureError, A>> func) => 
        new(func);

    public static FeatureEff<RT, A> Lift(Func<Fin<A>> func) => new(func);

    public static FeatureEff<RT, A> Lift(Func<Either<Exceptional, A>> func) => new(func);

    public static FeatureEff<RT, A> Lift(Func<Fin<Either<FeatureError, A>>> func) => 
        new(func);

}
