using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    // Constructores desde IO con runtime
    public static FeatureEff<RT, A> LiftIO(Func<RT, IO<A>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, IO<Pure<A>>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, IO<FeatureError>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, IO<Fail<FeatureError>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, IO<Exceptional>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, IO<Fail<Exceptional>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, IO<Either<FeatureError, A>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, IO<Either<Exceptional, A>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, IO<Fin<A>>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, IO<Fin<Either<FeatureError, A>>>> func) => 
        new(func);

    // Constructores desde funciones asincronas con runtime
    public static FeatureEff<RT, A> LiftIO(Func<RT, Task<A>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, Task<Pure<A>>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, Task<FeatureError>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, Task<Fail<FeatureError>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, Task<Exceptional>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, Task<Fail<Exceptional>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, Task<Either<FeatureError, A>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, Task<Either<Exceptional, A>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, Task<Fin<A>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<RT, Task<Fin<Either<FeatureError, A>>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<IO<A>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<IO<Pure<A>>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<IO<FeatureError>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<IO<Fail<FeatureError>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<IO<Exceptional>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<IO<Fail<Exceptional>>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<IO<Either<FeatureError, A>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<IO<Either<Exceptional, A>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<IO<Fin<A>>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<IO<Fin<Either<FeatureError, A>>>> func) => 
        new(func);

    // Constructores desde funciones asincronas sin runtime
    public static FeatureEff<RT, A> LiftIO(Func<Task<A>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<Task<Pure<A>>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<Task<FeatureError>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<Task<Fail<FeatureError>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<Task<Exceptional>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<Task<Fail<Exceptional>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<Task<Either<FeatureError, A>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<Task<Either<Exceptional, A>>> func) => 
        new(func);

    public static FeatureEff<RT, A> LiftIO(Func<Task<Fin<A>>> func) => new(func);

    public static FeatureEff<RT, A> LiftIO(Func<Task<Fin<Either<FeatureError, A>>>> func) => 
        new(func);

}
