namespace VSlices.Monads;

// Contiene la implementación publica de los traits de Flow.Trait.Implementation
public partial class Flow<RT, REQ>
{
    private Flow() { }

    public static Flow<RT, REQ, O> Map<T, O>(
        Func<T, O> f, K<Flow<RT, REQ>, T> ma) =>
        +Functor.map(f, ma);

    public static Flow<RT, REQ, O> ConstMap<T, O>(
        O b, K<Flow<RT, REQ>, T> ma) =>
        Map(_ => b, ma);

    public static Flow<RT, REQ, A> Pure<A>(A value) =>
        +Applicative.pure<Flow<RT, REQ>, A>(value);

    public static Flow<RT, REQ, A> Pure<A>(Pure<A> pa) =>
        Pure(pa.Value);

    public static Flow<RT, REQ, Unit> Unit { get; } = Pure(unit);

    public static Flow<RT, REQ, Option<A>> Some<A>(A v) =>
        Pure<Option<A>>(v);

    public static Flow<RT, REQ, Option<A>> None<A>() =>
        Pure<Option<A>>(Option.None);

    public static Flow<RT, REQ, O> Action<A, O>(
        K<Flow<RT, REQ>, A> ma, 
        K<Flow<RT, REQ>, O> mb) =>
        +Applicative.action(ma, mb);
    
    public static Flow<RT, REQ, O> Apply<T, O>(
        K<Flow<RT, REQ>, Func<T, O>> mf,
        K<Flow<RT, REQ>, T> ma) =>
        +Applicative.apply(mf, ma);
    
    public static Flow<RT, REQ, O> Bind<T, O>(
        K<Flow<RT, REQ>, T> ma,
        Func<T, K<Flow<RT, REQ>, O>> fb) =>
        +Monad.bind(ma, fb);

    public static Flow<RT, REQ, O> Recur<T, O>(
        T value,
        Func<T, K<Flow<RT, REQ>, Next<T, O>>> f) =>
        +Monad.recur(value, f);

    public static Flow<RT, REQ, A> LiftIO<A>(IO<A> ma) =>
        +MonadIO.liftIO<Flow<RT, REQ>, A>(ma);

    public static Flow<RT, REQ, IO<A>> ToIO<A>(K<Flow<RT, REQ>, A> ma) =>
        +MonadUnliftIO.toIO(ma);

    public static Flow<RT, REQ, A> Fail<A>(Error e) =>
        +Fallible.error<Flow<RT, REQ>, A>(e);

    public static Flow<RT, REQ, A> Fail<A>(string msg) =>
        Fail<A>(Error.New(msg));

    public static Flow<RT, REQ, A> Fail<A>(Fail<Error> fe) =>
        Fail<A>(fe.Value);

    public static Flow<RT, REQ, A> Fail<A>(Fail<string> fe) =>
        Fail<A>(Error.New(fe.Value));

    public static Flow<RT, REQ, A> Catch<A>(
        K<Flow<RT, REQ>, A> fa,
        Func<Error, bool> Predicate,
        Func<Error, K<Flow<RT, REQ>, A>> Fail) =>
        +fa.Catch(Predicate, Fail);

    public static Flow<RT, REQ, A> Choose<A>(
        K<Flow<RT, REQ>, A> fa,
        K<Flow<RT, REQ>, A> fb) =>
        +Choice.choose(fa, fb);

    public static Flow<RT, REQ, A> Choose<A>(
        K<Flow<RT, REQ>, A> fa,
        Memo<Flow<RT, REQ>, A> fb) =>
        +Choice.choose(fa, fb.Value);

    public static Flow<RT, REQ, A> Empty<A>() =>
        +Alternative.empty<Flow<RT, REQ>, A>();

    public static Flow<RT, REQ, A> Combine<A>(
        K<Flow<RT, REQ>, A> mx, 
        K<Flow<RT, REQ>, A> my) =>
        +SemigroupK.combine(mx, my);

    public static Flow<RT, REQ, A> Finally<A, X>(
        K<Flow<RT, REQ>, A> fa,
        K<Flow<RT, REQ>, X> fx) =>
        +(fa | Final.final(fx));
    
    public static Flow<RT, REQ, A> Asks<A>(Func<RT, REQ, A> f) =>
        +Readable.asks<Flow<RT, REQ>, (RT, REQ), A>(cr => f(cr.Item1, cr.Item2));

    public static Flow<RT, REQ, A> Local<A>(K<Flow<RT, REQ>, A> ma) =>
        +Readable.local<Flow<RT, REQ>, (RT, REQ), A>(cr => cr, ma);
}
