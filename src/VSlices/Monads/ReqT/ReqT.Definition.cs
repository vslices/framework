namespace VSlices.Monads;

/// <summary>
///
/// </summary>
/// <typeparam name="M"></typeparam>
/// <typeparam name="IN"></typeparam>
/// <typeparam name="A"></typeparam>
public sealed record ReqT<M, IN, A> : K<ReqT<M, IN>, A>
    where M : Monad<M>
{
    /// <summary>
    ///
    /// </summary>
    public static readonly ReqT<M, IN, IN> Input =
        Readable.ask<ReqT<M, IN>, IN>().As();

    /// <summary>
    ///
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public FinT<M, A> Onto(IN input) =>
        throw new NotImplementedException();

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="fb"></param>
    /// <returns></returns>
    public ReqT<M, IN, B> Map<B>(Func<A, B> fb) =>
        +Functor.map(fb, this);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="fb"></param>
    /// <returns></returns>
    public ReqT<M, IN, B> Bind<B>(Func<A, K<ReqT<M, IN>, B>> fb) =>
        +Monad.bind(this, fb);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="fb"></param>
    /// <returns></returns>
    public ReqT<M, IN, B> Bind<B>(Func<A, ReqT<M, IN, B>> fb) =>
        +Monad.bind(this, fb);

    /// <summary>
    ///
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public ReqT<M, IN, Unit> Tell(Error error) =>
        +Writable.tell<ReqT<M, IN>, Error>(error);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ReqT<M, IN, (A Value, Error Error)> Listen() =>
        +Writable.listen<Error, ReqT<M, IN>, A>(this);

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static ReqT<M, IN, A> Pure(A v) =>
        +Applicative.pure<ReqT<M, IN>, A>(v);

    /// <summary>
    ///
    /// </summary>
    /// <param name="mf"></param>
    public static implicit operator ReqT<M, IN, A>(Pure<A> mf) =>
        Pure(mf.Value);

}
