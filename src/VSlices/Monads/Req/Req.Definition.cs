namespace VSlices.Monads;

public delegate (A Value, Error Error) ReqExecute<IN, A>(IN input, Error error);

public sealed record Req<IN, A>(ReqExecute<IN, A> RawRun) : K<Req<IN>, A>
{
    public Fin<A> Run(IN input) =>
        RawRun(input, Error.Empty)
            .Map(Fin<A> (v) => v.Item2.IsEmpty ? v.Item1 : v.Item2);

    public Req<IN, B> Map<B>(Func<A, B> fb) =>
        +Functor.map(fb, this);

    public Req<IN, B> Bind<B>(Func<A, K<Req<IN>, B>> fb) =>
        +Monad.bind(this, fb);

    public Req<IN, B> Bind<B>(Func<A, Req<IN, B>> fb) =>
        +Monad.bind(this, fb);

    public Req<IN, Unit> Tell(Error error) =>
        +Writable.tell<Req<IN>, Error>(error);

    public Req<IN, (A Value, Error Error)> Listen() =>
        Listens(x => x);

    public Req<IN, (A Value, B Output)> Listens<B>(
        Func<Error, B> f) =>
        new((i, e) => RawRun(i, e).Map(ra => ((ra.Item1, f(ra.Item2)), ra.Item2)));

    public static Req<IN, A> Pure(A v) =>
        +Applicative.pure<Req<IN>, A>(v);

    public static implicit operator Req<IN, A>(Pure<A> mf) =>
        Pure(mf.Value);

}
