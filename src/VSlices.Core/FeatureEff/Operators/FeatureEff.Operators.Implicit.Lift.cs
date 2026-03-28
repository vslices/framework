using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    public static implicit operator FeatureEff<RT, A>(Lift<RT, A> func) => 
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Pure<A>> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, FeatureError> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Fail<FeatureError>> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Exceptional> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Fail<Exceptional>> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Either<FeatureError, A>> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Fin<A>> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Either<Exceptional, A>> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Fin<Either<FeatureError, A>>> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<A> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Pure<A>> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<FeatureError> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Fail<FeatureError>> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Exceptional> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Fail<Exceptional>> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Either<FeatureError, A>> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Fin<A>> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Either<Exceptional, A>> func) =>
        Lift(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Fin<Either<FeatureError, A>>> func) => 
        Lift(func.Function);

}
