namespace VSlices.Monads;

/// <summary>
/// Represents a monadic flow that encapsulates computations with a specific runtime and request context.
/// </summary>
/// <typeparam name="RT">The type of the runtime context used in the flow.</typeparam>
/// <typeparam name="REQ">The type of the request context used in the flow.</typeparam>
public partial class Flow<RT, REQ>
    : MonadUnliftIO<Flow<RT, REQ>>,
      Fallible<Error, Flow<RT, REQ>>,
      Alternative<Flow<RT, REQ>>,
      MonoidK<Flow<RT, REQ>>,
      Final<Flow<RT, REQ>>,
      Readable<Flow<RT, REQ>, (RT, REQ)>
{
    static K<Flow<RT, REQ>, B> Functor<Flow<RT, REQ>>.Map<A, B>(
        Func<A, B> f, K<Flow<RT, REQ>, A> ma) =>
        new Flow<RT, REQ, B>((s, r) => ma.RunFlow(s, r).Map(f));

    static K<Flow<RT, REQ>, A> Applicative<Flow<RT, REQ>>.Pure<A>(A value) =>
        new Flow<RT, REQ, A>((_, _) => IO.pure(value));

    static K<Flow<RT, REQ>, B> Applicative<Flow<RT, REQ>>.Apply<A, B>(
        K<Flow<RT, REQ>, Func<A, B>> mf,
        K<Flow<RT, REQ>, A> ma) =>
        new Flow<RT, REQ, B>(
            (s, r) => mf.As().RunFlow(s, r)
                        .Apply(ma.RunFlow(s, r)));

    static K<Flow<RT, REQ>, B> Applicative<Flow<RT, REQ>>.Apply<A, B>(
        K<Flow<RT, REQ>, Func<A, B>> mf,
        Memo<Flow<RT, REQ>, A> ma) =>
        new Flow<RT, REQ, B>(
            (s, r) => mf.As().RunFlow(s, r)
                .Apply(ma.Value.RunFlow(s, r)));

    static K<Flow<RT, REQ>, B> Monad<Flow<RT, REQ>>.Bind<A, B>(
        K<Flow<RT, REQ>, A> ma,
        Func<A, K<Flow<RT, REQ>, B>> f) =>
        new Flow<RT, REQ, B>(
            (s, r) => ma.RunFlow(s, r)
                        .Bind(a => f(a).RunFlow(s, r)));

    static K<Flow<RT, REQ>, B> Monad<Flow<RT, REQ>>.Recur<A, B>(
        A value,
        Func<A, K<Flow<RT, REQ>, Next<A, B>>> f) =>
        liftFlow<RT, REQ, B>(async (ctx, req, env) =>
        {
            var current = value;
            
            while (true)
            {
                var mNext = await f(current).As().RunAsync(ctx, req, env);

                if (mNext is Fin<Next<A, B>>.Fail(var e))
                {
                    return Fin.Fail<B>(e);
                }

                var next = (Next<A, B>)mNext;

                if (next.IsDone) return Fin.Succ(next.Done);
                current = next.Loop;
            }
        });

    static K<Flow<RT, REQ>, A> MonadIO<Flow<RT, REQ>>.LiftIO<A>(IO<A> ma) =>
        new Flow<RT, REQ, A>((_, _) => ma);

    static K<Flow<RT, REQ>, IO<A>> MonadUnliftIO<Flow<RT, REQ>>.ToIO<A>(K<Flow<RT, REQ>, A> ma) =>
        new Flow<RT, REQ, IO<A>>((c, r) => IO.pure(ma.RunFlow(c, r)));

    static K<Flow<RT, REQ>, A> Fallible<Error, Flow<RT, REQ>>.Fail<A>(Error error) =>
        new Flow<RT, REQ, A>((_, _) => IO.fail<A>(error));

    static K<Flow<RT, REQ>, A> Fallible<Error, Flow<RT, REQ>>.Catch<A>(
        K<Flow<RT, REQ>, A> fa,
        Func<Error, bool> Predicate,
        Func<Error, K<Flow<RT, REQ>, A>> Fail) =>
        new Flow<RT, REQ, A>(
            (s, r) => +fa.RunFlow(s, r)
                         .Catch(e => Predicate(e) ? Fail(e).RunFlow(s, r) : IO.fail<A>(e)));

    static K<Flow<RT, REQ>, A> Choice<Flow<RT, REQ>>.Choose<A>(K<Flow<RT, REQ>, A> fa, K<Flow<RT, REQ>, A> fb) =>
        new Flow<RT, REQ, A>(
            (s, r) => +fa.RunFlow(s, r) | @catch(_ => fb.RunFlow(s, r)));

    static K<Flow<RT, REQ>, A> Choice<Flow<RT, REQ>>.Choose<A>(K<Flow<RT, REQ>, A> fa, Memo<Flow<RT, REQ>, A> fb) =>
        new Flow<RT, REQ, A>(
            (s, r) => +fa.RunFlow(s, r) | @catch(_ => fb.Value.RunFlow(s, r)));

    static K<Flow<RT, REQ>, A> Alternative<Flow<RT, REQ>>.Empty<A>() =>
        Fail<A>(Error.Empty);

    static K<Flow<RT, REQ>, A> SemigroupK<Flow<RT, REQ>>.Combine<A>(
        K<Flow<RT, REQ>, A> lhs, K<Flow<RT, REQ>, A> rhs) =>
        lhs | @catch(e1 => rhs | @catch(e2 => Fail<A>(e1 + e2)));

    static K<Flow<RT, REQ>, A> MonoidK<Flow<RT, REQ>>.Empty<A>() =>
        Fail<A>(Error.Empty);

    static K<Flow<RT, REQ>, A> Final<Flow<RT, REQ>>.Finally<X, A>(
        K<Flow<RT, REQ>, A> fa,
        K<Flow<RT, REQ>, X> @finally) =>
        new Flow<RT, REQ, A>(
            (c, r) => fa.RunFlow(c, r)
                        .Finally(@finally.RunFlow(c, r)));

    static K<Flow<RT, REQ>, A> Readable<Flow<RT, REQ>, (RT, REQ)>.Asks<A>(
        Func<(RT, REQ), A> f) =>
        new Flow<RT, REQ, A>((s, r) => IO.pure(f((s, r))));

    static K<Flow<RT, REQ>, A> Readable<Flow<RT, REQ>, (RT, REQ)>.Local<A>(
        Func<(RT, REQ), (RT, REQ)> f, 
        K<Flow<RT, REQ>, A> ma) =>
        new Flow<RT, REQ, A>((s, r) =>
        {
            var (newS, newR) = f((s, r));
            return ma.RunFlow(newS, newR);
        });
}
