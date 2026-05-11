using System.Net.Http.Headers;

namespace VSlices.Monads;

// Contiene la implementación privada de los traits de Flow.Trait.Implementation,
// para mejorar la performance del monad, la base es trabajar acá
public partial class Flow<C, R>
    : MonadUnliftIO<Flow<C, R>>,
      Fallible<Error, Flow<C, R>>,
      Alternative<Flow<C, R>>,
      MonoidK<Flow<C, R>>,
      Final<Flow<C, R>>,
      Readable<Flow<C, R>, (C, R)>
{
    static K<Flow<C, R>, B> Functor<Flow<C, R>>.Map<A, B>(
        Func<A, B> f, K<Flow<C, R>, A> ma) =>
        new Flow<C, R, B>((s, r) => ma.RunFlow(s, r).Map(f));

    static K<Flow<C, R>, A> Applicative<Flow<C, R>>.Pure<A>(A value) =>
        new Flow<C, R, A>((_, _) => IO.pure(value));

    static K<Flow<C, R>, B> Applicative<Flow<C, R>>.Apply<A, B>(
        K<Flow<C, R>, Func<A, B>> mf,
        K<Flow<C, R>, A> ma) =>
        new Flow<C, R, B>(
            (s, r) => mf.As().RunFlow(s, r)
                        .Apply(ma.RunFlow(s, r)));

    static K<Flow<C, R>, B> Applicative<Flow<C, R>>.Apply<A, B>(
        K<Flow<C, R>, Func<A, B>> mf,
        Memo<Flow<C, R>, A> ma) =>
        new Flow<C, R, B>(
            (s, r) => mf.As().RunFlow(s, r)
                .Apply(ma.Value.RunFlow(s, r)));

    static K<Flow<C, R>, B> Monad<Flow<C, R>>.Bind<A, B>(
        K<Flow<C, R>, A> ma,
        Func<A, K<Flow<C, R>, B>> f) =>
        new Flow<C, R, B>(
            (s, r) => ma.RunFlow(s, r)
                        .Bind(a => f(a).RunFlow(s, r)));

    static K<Flow<C, R>, B> Monad<Flow<C, R>>.Recur<A, B>(
        A value,
        Func<A, K<Flow<C, R>, Next<A, B>>> f) =>
        liftFlow<C, R, B>(async (ctx, req, env) =>
        {
            while (true)
            {
                var mNext = await f(value).As().RunAsync(ctx, req, env);

                if (mNext is Fin<Next<A, B>>.Fail(var e))
                {
                    return Fin.Fail<B>(e);
                }

                var next = (Next<A, B>)mNext;

                if (next.IsDone) return Fin.Succ(next.Done);
                value = next.Loop;
            }
        });

    static K<Flow<C, R>, A> MonadIO<Flow<C, R>>.LiftIO<A>(IO<A> ma) =>
        new Flow<C, R, A>((_, _) => ma);

    static K<Flow<C, R>, IO<A>> MonadUnliftIO<Flow<C, R>>.ToIO<A>(K<Flow<C, R>, A> ma) =>
        new Flow<C, R, IO<A>>((c, r) => IO.pure(ma.RunFlow(c, r)));

    static K<Flow<C, R>, A> Fallible<Error, Flow<C, R>>.Fail<A>(Error error) =>
        new Flow<C, R, A>((_, _) => IO.fail<A>(error));

    static K<Flow<C, R>, A> Fallible<Error, Flow<C, R>>.Catch<A>(
        K<Flow<C, R>, A> fa,
        Func<Error, bool> Predicate,
        Func<Error, K<Flow<C, R>, A>> Fail) =>
        new Flow<C, R, A>(
            (s, r) => +fa.RunFlow(s, r)
                         .Catch(e => Predicate(e) ? Fail(e).RunFlow(s, r) : IO.fail<A>(e)));

    static K<Flow<C, R>, A> Choice<Flow<C, R>>.Choose<A>(K<Flow<C, R>, A> fa, K<Flow<C, R>, A> fb) =>
        new Flow<C, R, A>(
            (s, r) => +fa.RunFlow(s, r) | @catch(_ => fb.RunFlow(s, r)));

    static K<Flow<C, R>, A> Choice<Flow<C, R>>.Choose<A>(K<Flow<C, R>, A> fa, Memo<Flow<C, R>, A> fb) =>
        new Flow<C, R, A>(
            (s, r) => +fa.RunFlow(s, r) | @catch(_ => fb.Value.RunFlow(s, r)));

    static K<Flow<C, R>, A> Alternative<Flow<C, R>>.Empty<A>() =>
        Fail<A>(Error.Empty);

    static K<Flow<C, R>, A> SemigroupK<Flow<C, R>>.Combine<A>(
        K<Flow<C, R>, A> lhs, K<Flow<C, R>, A> rhs) =>
        lhs | @catch(e1 => rhs | @catch(e2 => Fail<A>(e1 + e2)));

    static K<Flow<C, R>, A> MonoidK<Flow<C, R>>.Empty<A>() =>
        Fail<A>(Error.Empty);

    static K<Flow<C, R>, A> Final<Flow<C, R>>.Finally<X, A>(
        K<Flow<C, R>, A> fa,
        K<Flow<C, R>, X> @finally) =>
        new Flow<C, R, A>(
            (c, r) => fa.RunFlow(c, r)
                        .Finally(@finally.RunFlow(c, r)));

    static K<Flow<C, R>, A> Readable<Flow<C, R>, (C, R)>.Asks<A>(
        Func<(C, R), A> f) =>
        new Flow<C, R, A>((s, r) => IO.pure(f((s, r))));

    static K<Flow<C, R>, A> Readable<Flow<C, R>, (C, R)>.Local<A>(
        Func<(C, R), (C, R)> f, 
        K<Flow<C, R>, A> ma) =>
        new Flow<C, R, A>((s, r) =>
        {
            var (newS, newR) = f((s, r));
            return ma.RunFlow(newS, newR);
        });
}
