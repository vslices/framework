namespace VSlices.Monads;

// Contiene la implementación publica de los traits de Flow.Trait.Implementation
public partial class Flow<C, R>
{
    private Flow()
    {
    }

    public static Flow<C, R, O> Map<T, O>(
        Func<T, O> f, K<Flow<C, R>, T> ma) =>
        +Functor.map(f, ma);

    public static Flow<C, R, O> ConstMap<T, O>(
        O b, K<Flow<C, R>, T> ma) =>
        Map(_ => b, ma);

    public static Flow<C, R, A> Pure<A>(A value) =>
        +Applicative.pure<Flow<C, R>, A>(value);

    public static Flow<C, R, A> Pure<A>(Pure<A> pa) =>
        Pure(pa.Value);

    public static Flow<C, R, Unit> Unit { get; } = Pure(unit);

    public static Flow<C, R, Option<A>> Some<A>(A v) =>
        Pure<Option<A>>(v);

    public static Flow<C, R, Option<A>> None<A>() =>
        Pure<Option<A>>(Option.None);

    public static Flow<C, R, O> Action<A, O>(
        K<Flow<C, R>, A> ma, 
        K<Flow<C, R>, O> mb) =>
        +Applicative.action(ma, mb);
    
    public static Flow<C, R, O> Apply<T, O>(
        K<Flow<C, R>, Func<T, O>> mf,
        K<Flow<C, R>, T> ma) =>
        +Applicative.apply(mf, ma);
    
    public static Flow<C, R, O> Bind<T, O>(
        K<Flow<C, R>, T> ma,
        Func<T, K<Flow<C, R>, O>> fb) =>
        +Monad.bind(ma, fb);

    public static Flow<C, R, O> Recur<T, O>(
        T value,
        Func<T, K<Flow<C, R>, Next<T, O>>> f) =>
        +Monad.recur(value, f);

    public static Flow<C, R, A> LiftIO<A>(IO<A> ma) =>
        +MonadIO.liftIO<Flow<C, R>, A>(ma);

    public static Flow<C, R, IO<A>> ToIO<A>(K<Flow<C, R>, A> ma) =>
        +MonadUnliftIO.toIO(ma);

    public static Flow<C, R, A> Fail<A>(Error e) =>
        +Fallible.error<Flow<C, R>, A>(e);

    public static Flow<C, R, A> Fail<A>(string msg) =>
        Fail<A>(Error.New(msg));

    public static Flow<C, R, A> Fail<A>(Fail<Error> fe) =>
        Fail<A>(fe.Value);

    public static Flow<C, R, A> Fail<A>(Fail<string> fe) =>
        Fail<A>(Error.New(fe.Value));

    public static Flow<C, R, A> Catch<A>(
        K<Flow<C, R>, A> fa,
        Func<Error, bool> Predicate,
        Func<Error, K<Flow<C, R>, A>> Fail) =>
        +fa.Catch(Predicate, Fail);

    public static Flow<C, R, A> Choose<A>(
        K<Flow<C, R>, A> fa,
        K<Flow<C, R>, A> fb) =>
        +Choice.choose(fa, fb);

    public static Flow<C, R, A> Choose<A>(
        K<Flow<C, R>, A> fa,
        Memo<Flow<C, R>, A> fb) =>
        +Choice.choose(fa, fb.Value);

    public static Flow<C, R, A> Empty<A>() =>
        +Alternative.empty<Flow<C, R>, A>();

    public static Flow<C, R, A> Combine<A>(
        K<Flow<C, R>, A> mx, 
        K<Flow<C, R>, A> my) =>
        +SemigroupK.combine(mx, my);

    public static Flow<C, R, A> Finally<A, X>(
        K<Flow<C, R>, A> fa,
        K<Flow<C, R>, X> fx) =>
        +(fa | Final.final(fx));
    
    public static Flow<C, R, A> Asks<A>(Func<C, R, A> f) =>
        +Readable.asks<Flow<C, R>, (C, R), A>(cr => f(cr.Item1, cr.Item2));

    public static Flow<C, R, A> Local<A>(K<Flow<C, R>, A> ma) =>
        +Readable.local<Flow<C, R>, (C, R), A>(cr => cr, ma);
}
