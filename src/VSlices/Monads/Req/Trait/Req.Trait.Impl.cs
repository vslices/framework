namespace VSlices.Monads;

/// <summary>
///
/// </summary>
/// <remarks>
///
/// </remarks>
/// <typeparam name="IN"></typeparam>
public partial class Req<IN> :
    Monad<Req<IN>>,
    Readable<Req<IN>, IN>,
    Writable<Req<IN>, Error>
{
    static K<Req<IN>, B> Functor<Req<IN>>.Map<A, B>(
        Func<A, B> f,
        K<Req<IN>, A> ma) =>
        new Req<IN, B>(
            (i, e) => ma.RawRun(i, e).MapFirst(i => f(i)));

    static K<Req<IN>, A> Applicative<Req<IN>>.Pure<A>(A value) =>
        new Req<IN, A>((_, e) => (value, e));

    static K<Req<IN>, B> Applicative<Req<IN>>.Apply<A, B>(
        K<Req<IN>, Func<A, B>> mf, K<Req<IN>, A> ma) =>
        mf.As().Bind(x => ma.As().Map(x));

    static K<Req<IN>, B> Applicative<Req<IN>>.Apply<A, B>(
        K<Req<IN>, Func<A, B>> mf, Memo<Req<IN>, A> ma) =>
        mf.As().Bind(x => ma.Value.As().Map(x));

    static K<Req<IN>, B> Monad<Req<IN>>.Bind<A, B>(
        K<Req<IN>, A> ma,
        Func<A, K<Req<IN>, B>> f) =>
        new Req<IN, B>(
            (i, e) => ma.RawRun(i, e)
                        .Map(ra => f(ra.Item1).RawRun(i, ra.Item2)));

    static K<Req<IN>, B> Monad<Req<IN>>.Recur<A, B>(
        A value,
        Func<A, K<Req<IN>, Next<A, B>>> f) =>
        new Req<IN, B>((i, e) =>
        {
            while (true)
            {
                (Next<A, B> Value, Error Output) mr = f(value).RawRun(i, e);

                if (mr.Value.IsDone) return mr.MapFirst(v => v.Done);

                value = mr.Value.Loop;
            }
        });

    static K<Req<IN>, A> Readable<Req<IN>, IN>.Asks<A>(
        Func<IN, A> f) =>
        new Req<IN, A>((i, e) => (f(i), e));

    static K<Req<IN>, A> Readable<Req<IN>, IN>.Local<A>(
        Func<IN, IN> f, K<Req<IN>, A> ma) =>
        new Req<IN, A>((i, e) => ma.RawRun(f(i), e));

    static K<Req<IN>, Unit> Writable<Req<IN>, Error>.Tell(
        Error item) =>
        new Req<IN, Unit>((i, e) => (unit, e + item));

    static K<Req<IN>, (A Value, Error Output)> Writable<Req<IN>, Error>.Listen<A>(
        K<Req<IN>, A> ma) =>
        ma.As().Listen();

    static K<Req<IN>, A> Writable<Req<IN>, Error>.Pass<A>(
        K<Req<IN>, (A Value, Func<Error, Error> Function)> action) =>
        new Req<IN, A>((i, e) =>
        {
            var ((va, fa), ea) = action.RawRun(i, e);

            return (va, fa(ea));
        });
}
