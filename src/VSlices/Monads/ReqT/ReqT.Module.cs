namespace VSlices.Monads;

/// <summary>
///
/// </summary>
/// <typeparam name="M"></typeparam>
public static partial class ReqT<M>
    where M : Monad<M>
{
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="IN"></typeparam>
        /// <param name="fCheck"></param>
        /// <returns></returns>
    public static ReqT<M, IN, bool> Check<IN>(Func<IN, bool> fCheck) =>
        ReqT<M, IN>.Check(fCheck);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <param name="fError"></param>
    /// <returns></returns>
    public static ReqT<M, IN, Unit> Prescribe<IN>(Func<IN, Error> fError) =>
        ReqT<M, IN>.Prescribe(fError);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <param name="fError"></param>
    /// <returns></returns>
    public static ReqT<M, IN, Unit> Prescribe<IN>(Func<IN, string> fError) =>
        ReqT<M, IN>.Prescribe(i => Error.New(fError(i)));

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <param name="fCheck"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static ReqT<M, IN, Unit> Ensure<IN>(
        Func<IN, bool> fCheck,
        Func<IN, Error> Fail) =>
        ReqT<M, IN>.Ensure(fCheck, Fail);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <typeparam name="OUT"></typeparam>
    /// <param name="fOut"></param>
    /// <returns></returns>
    public static ReqT<M, IN, OUT> Transform<IN, OUT>(Func<IN, OUT> fOut) =>
        ReqT<M, IN>.Transform(fOut);
}

/// <summary>
///
/// </summary>
public static partial class ReqT
{
    /// <summary>
    ///
    /// </summary>
    public static readonly Pure<Unit> Ok = Pure(unit);

}
