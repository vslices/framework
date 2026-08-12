namespace VSlices.Monads;

/// <summary>
///
/// </summary>
public static partial class Req
{
    /// <summary>
    ///
    /// </summary>
    public static readonly Pure<Unit> Ok = Pure(unit);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <param name="fCheck"></param>
    /// <returns></returns>
    public static Req<IN, bool> Check<IN>(Func<IN, bool> fCheck) =>
        Req<IN>.Check(fCheck);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <param name="fError"></param>
    /// <returns></returns>
    public static Req<IN, Unit> Prescribe<IN>(Func<IN, Error> fError) =>
        Req<IN>.Prescribe(fError);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <param name="fError"></param>
    /// <returns></returns>
    public static Req<IN, Unit> Prescribe<IN>(Func<IN, string> fError) =>
        Req<IN>.Prescribe(i => Error.New(fError(i)));

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <param name="fCheck"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, Unit> Ensure<IN>(
        Func<IN, bool> fCheck,
        Func<IN, Error> Fail) =>
        Req<IN>.Ensure(fCheck, Fail);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <param name="fCheck"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, Unit> Ensure<IN>(
        Func<IN, bool> fCheck,
        Func<IN, string> Fail) =>
        Req<IN>.Ensure(fCheck, i => Error.New(Fail(i)));

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <param name="fCheck"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, Unit> Ensure<IN>(
        Func<IN, bool> fCheck,
        string Fail) =>
        Req<IN>.Ensure(fCheck, i => Error.New(Fail));

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <typeparam name="OUT"></typeparam>
    /// <param name="fOut"></param>
    /// <returns></returns>
    public static Req<IN, OUT> Transform<IN, OUT>(Func<IN, OUT> fOut) =>
        Req<IN>.Transform(fOut);
}
