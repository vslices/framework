namespace VSlices.Monads;

public sealed record ReqT<M, IN, A> : K<ReqT<M, IN>, A>
    where M : Monad<M>
{
    public static readonly ReqT<M, IN, IN> Input =
        Readable.ask<ReqT<M, IN>, IN>().As();

    public Fin<A> Run(IN input) =>
        throw new NotImplementedException();

    public ReqT<M, IN, B> Map<B>(Func<A, B> fb) =>
        +Functor.map(fb, this);

    public ReqT<M, IN, B> Bind<B>(Func<A, K<ReqT<M, IN>, B>> fb) =>
        +Monad.bind(this, fb);

    public ReqT<M, IN, B> Bind<B>(Func<A, ReqT<M, IN, B>> fb) =>
        +Monad.bind(this, fb);

    public ReqT<M, IN, Unit> Tell(Error error) =>
        +Writable.tell<ReqT<M, IN>, Error>(error);

    public ReqT<M, IN, (A Value, Error Error)> Listen() =>
        throw new NotImplementedException();

    public static ReqT<M, IN, A> Pure(A v) =>
        +Applicative.pure<ReqT<M, IN>, A>(v);

    public static implicit operator ReqT<M, IN, A>(Pure<A> mf) =>
        Pure(mf.Value);

}
