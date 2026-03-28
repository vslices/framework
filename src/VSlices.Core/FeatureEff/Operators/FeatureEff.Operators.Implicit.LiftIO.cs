using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    // Constructores desde IO con runtime
    public static implicit operator FeatureEff<RT, A>(Func<RT, IO<A>> func) => 
        LiftIO(func);

    public static implicit operator FeatureEff<RT, A>(Func<RT, IO<Pure<A>>> func) =>
        LiftIO(func);

    public static implicit operator FeatureEff<RT, A>(Func<RT, IO<FeatureError>> func) =>
        LiftIO(func);

    public static implicit operator FeatureEff<RT, A>(Func<RT, IO<Fail<FeatureError>>> func)  => 
        LiftIO(func);

    public static implicit operator FeatureEff<RT, A>(Func<RT, IO<Exceptional>> func)  => 
        LiftIO(func);

    public static implicit operator FeatureEff<RT, A>(Func<RT, IO<Fail<Exceptional>>> func)  => 
        LiftIO(func);

    public static implicit operator FeatureEff<RT, A>(Func<RT, IO<Either<FeatureError, A>>> func)  => 
        LiftIO(func);

    public static implicit operator FeatureEff<RT, A>(Func<RT, IO<Either<Exceptional, A>>> func)  => 
        LiftIO(func);

    public static implicit operator FeatureEff<RT, A>(Func<RT, IO<Fin<A>>> func) =>
        LiftIO(func);

    public static implicit operator FeatureEff<RT, A>(Func<RT, IO<Fin<Either<FeatureError, A>>>> func) => 
        LiftIO(func);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Task<A>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Task<Pure<A>>> func) =>
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Task<FeatureError>> func) =>
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Task<Fail<FeatureError>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Task<Exceptional>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Task<Fail<Exceptional>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Task<Either<FeatureError, A>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Task<Either<Exceptional, A>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Task<Fin<A>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<RT, Task<Fin<Either<FeatureError, A>>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<IO<A>> func) =>
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<IO<Pure<A>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<IO<FeatureError>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<IO<Fail<FeatureError>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<IO<Exceptional>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<IO<Fail<Exceptional>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<IO<Either<FeatureError, A>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<IO<Either<Exceptional, A>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<IO<Fin<A>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<IO<Fin<Either<FeatureError, A>>>> func) => 
        new(func);

    // Constructores desde funciones asincronas sin runtime
    public static implicit operator FeatureEff<RT, A>(Lift<Task<A>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Task<Pure<A>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Task<FeatureError>> func) =>
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Task<Fail<FeatureError>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Task<Exceptional>> func) =>
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Task<Fail<Exceptional>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Task<Either<FeatureError, A>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Task<Either<Exceptional, A>>> func) => 
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Task<Fin<A>>> func) =>
        LiftIO(func.Function);

    public static implicit operator FeatureEff<RT, A>(Lift<Task<Fin<Either<FeatureError, A>>>> func) => 
        LiftIO(func.Function);

}
