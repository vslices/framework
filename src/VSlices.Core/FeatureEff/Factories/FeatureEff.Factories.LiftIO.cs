using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial class FeatureEff
{
    // Constructores desde IO con runtime
    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, IO<A>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, IO<Pure<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, IO<FeatureError>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, IO<Fail<FeatureError>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, IO<Exceptional>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, IO<Fail<Exceptional>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, IO<Either<FeatureError, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, IO<Either<Exceptional, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, IO<Fin<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, IO<Fin<Either<FeatureError, A>>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    // Constructores desde funciones asincronas con runtime
    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, Task<A>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, Task<Pure<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, Task<FeatureError>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, Task<Fail<FeatureError>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, Task<Exceptional>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, Task<Fail<Exceptional>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, Task<Either<FeatureError, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, Task<Either<Exceptional, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, Task<Fin<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<RT, Task<Fin<Either<FeatureError, A>>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<IO<A>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<IO<Pure<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<IO<FeatureError>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<IO<Fail<FeatureError>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<IO<Exceptional>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<IO<Fail<Exceptional>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<IO<Either<FeatureError, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<IO<Either<Exceptional, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<IO<Fin<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<IO<Fin<Either<FeatureError, A>>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    // Constructores desde funciones asincronas sin runtime
    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<Task<A>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<Task<Pure<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<Task<FeatureError>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<Task<Fail<FeatureError>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<Task<Exceptional>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<Task<Fail<Exceptional>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<Task<Either<FeatureError, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<Task<Either<Exceptional, A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<Task<Fin<A>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

    public static FeatureEff<RT, A> LiftIO<A, RT>(Func<Task<Fin<Either<FeatureError, A>>>> func) => 
        FeatureEff<RT, A>.LiftIO(func);

}
