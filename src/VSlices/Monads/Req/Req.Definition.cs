namespace VSlices.Monads;

/// <summary>
///
/// </summary>
/// <typeparam name="IN">
///
/// </typeparam>
/// <typeparam name="A">
///
/// </typeparam>
/// <param name="input">
///
/// </param>
/// <param name="error">
///
/// </param>
/// <returns>
///
/// </returns>
public delegate (A Value, Error Error) ReqExecute<IN, A>(IN input, Error error);

/// <summary>
///
/// </summary>
/// <typeparam name="IN"></typeparam>
/// <typeparam name="OUT"></typeparam>
/// <param name="RawRun"></param>
public sealed record Req<IN, OUT>(ReqExecute<IN, OUT> RawRun) : K<Req<IN>, OUT>
{
    /// <summary>
    ///
    /// </summary>
    public static readonly Req<IN, IN> Input = Readable.ask<Req<IN>, IN>().As();

    /// <summary>
    ///
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Fin<OUT> Onto(IN input) =>
        RawRun(input, Error.Empty)
            .Map(Fin<OUT> (v) => v.Item2.IsEmpty ? v.Item1 : v.Item2);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="fb"></param>
    /// <returns></returns>
    public Req<IN, B> Map<B>(Func<OUT, B> fb) =>
        +Functor.map(fb, this);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="fb"></param>
    /// <returns></returns>
    public Req<IN, B> Bind<B>(Func<OUT, K<Req<IN>, B>> fb) =>
        +Monad.bind(this, fb);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="B"></typeparam>
    /// <param name="fb"></param>
    /// <returns></returns>
    public Req<IN, B> Bind<B>(Func<OUT, Req<IN, B>> fb) =>
        +Monad.bind(this, fb);

    /// <summary>
    ///
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public Req<IN, Unit> Tell(Error error) =>
        +Writable.tell<Req<IN>, Error>(error);

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Req<IN, OUT> Pure(OUT v) =>
        +Applicative.pure<Req<IN>, OUT>(v);

    /// <summary>
    ///
    /// </summary>
    /// <param name="mf"></param>
    public static implicit operator Req<IN, OUT>(Pure<OUT> mf) =>
        Pure(mf.Value);

}
