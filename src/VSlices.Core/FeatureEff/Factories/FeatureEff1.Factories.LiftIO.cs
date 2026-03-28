using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial class FeatureEff<RT>
{
    // Constructores desde IO con runtime
    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, IO<A>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, IO<Pure<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, IO<FeatureError>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, IO<Fail<FeatureError>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, IO<Exceptional>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, IO<Fail<Exceptional>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, IO<Either<FeatureError, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, IO<Either<Exceptional, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, IO<Fin<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, IO<Fin<Either<FeatureError, A>>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    // Constructores desde funciones asincronas con runtime
    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, Task<A>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, Task<Pure<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, Task<FeatureError>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, Task<Fail<FeatureError>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, Task<Exceptional>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, Task<Fail<Exceptional>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, Task<Either<FeatureError, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, Task<Either<Exceptional, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, Task<Fin<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<RT, Task<Fin<Either<FeatureError, A>>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<IO<A>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<IO<Pure<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<IO<FeatureError>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<IO<Fail<FeatureError>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<IO<Exceptional>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<IO<Fail<Exceptional>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<IO<Either<FeatureError, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<IO<Either<Exceptional, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<IO<Fin<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<IO<Fin<Either<FeatureError, A>>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    // Constructores desde funciones asincronas sin runtime
    public static FeatureEff<RT, A> LiftIO<A>(Func<Task<A>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<Task<Pure<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<Task<FeatureError>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<Task<Fail<FeatureError>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<Task<Exceptional>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<Task<Fail<Exceptional>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<Task<Either<FeatureError, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<Task<Either<Exceptional, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<Task<Fin<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A>(Func<Task<Fin<Either<FeatureError, A>>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

}
