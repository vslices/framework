namespace VSlices.Monads;

/// <summary>
///
/// </summary>
/// <typeparam name="M"></typeparam>
/// <typeparam name="IN"></typeparam>
public partial class ReqT<M, IN> :
    MonadT<ReqT<M, IN>, M>,
    Readable<ReqT<M, IN>, IN>,
    Writable<ReqT<M, IN>, Error>
    where M : Monad<M>
{
    static K<ReqT<M, IN>, B> Functor<ReqT<M, IN>>.Map<A, B>(
        Func<A, B> f,
        K<ReqT<M, IN>, A> ma) =>
        throw new NotImplementedException();

    static K<ReqT<M, IN>, B> Applicative<ReqT<M, IN>>.Apply<A, B>(
        K<ReqT<M, IN>, Func<A, B>> mf,
        K<ReqT<M, IN>, A> ma) =>
        throw new NotImplementedException();

    static K<ReqT<M, IN>, B> Applicative<ReqT<M, IN>>.Apply<A, B>(
        K<ReqT<M, IN>, Func<A, B>> mf,
        Memo<ReqT<M, IN>, A> ma) =>
        throw new NotImplementedException();

    static K<ReqT<M, IN>, A> Applicative<ReqT<M, IN>>.Pure<A>(
        A value) =>
        throw new NotImplementedException();

    static K<ReqT<M, IN>, B> Monad<ReqT<M, IN>>.Bind<A, B>(
        K<ReqT<M, IN>, A> ma,
        Func<A, K<ReqT<M, IN>, B>> f) =>
        throw new NotImplementedException();

    static K<ReqT<M, IN>, B> Monad<ReqT<M, IN>>.Recur<A, B>(
        A value,
        Func<A, K<ReqT<M, IN>, Next<A, B>>> f) =>
        throw new NotImplementedException();

    static K<ReqT<M, IN>, A> MonadT<ReqT<M, IN>, M>.Lift<A>(
        K<M, A> ma) =>
        throw new NotImplementedException();

    static K<ReqT<M, IN>, A> Readable<ReqT<M, IN>, IN>.Asks<A>(
        Func<IN, A> f) =>
        throw new NotImplementedException();

    static K<ReqT<M, IN>, A> Readable<ReqT<M, IN>, IN>.Local<A>(
        Func<IN, IN> f,
        K<ReqT<M, IN>, A> ma) =>
        throw new NotImplementedException();

    static K<ReqT<M, IN>, Unit> Writable<ReqT<M, IN>, Error>.Tell(
        Error item) =>
        throw new NotImplementedException();

    static K<ReqT<M, IN>, (A Value, Error Output)> Writable<ReqT<M, IN>, Error>.Listen<A>(
        K<ReqT<M, IN>, A> ma) =>
        throw new NotImplementedException();

    static K<ReqT<M, IN>, A> Writable<ReqT<M, IN>, Error>.Pass<A>(
        K<ReqT<M, IN>, (A Value, Func<Error, Error> Function)> action) =>
        throw new NotImplementedException();
}
