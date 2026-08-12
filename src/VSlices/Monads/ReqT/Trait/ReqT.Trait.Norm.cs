namespace VSlices.Monads;

public partial class ReqT<M, IN>
{
    /// <summary>
    ///
    /// </summary>
    public static readonly ReqT<M, IN, IN> Input =
        ReqT<M, IN, IN>.Input;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <param name="ma"></param>
    /// <returns></returns>
    public static ReqT<M, IN, A> Lift<A>(K<M, A> ma) =>
        +MonadT.lift<ReqT<M, IN>, M, A>(ma);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <param name="ma"></param>
    /// <returns></returns>
    public static ReqT<M, IN, A> LiftIO<A>(IO<A> ma) =>
        +MonadIO.liftIO<ReqT<M, IN>, A>(ma);

    /// <summary>
    ///
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static ReqT<M, IN, Unit> Append(Error e) =>
        Input.Tell(e);

    /// <summary>
    ///
    /// </summary>
    /// <param name="fCheck"></param>
    /// <returns></returns>
    public static ReqT<M, IN, bool> Check(Func<IN, K<M, bool>> fCheck) =>
        Input >> (i => Lift(fCheck(i)));

    /// <summary>
    ///
    /// </summary>
    /// <param name="fCheck"></param>
    /// <returns></returns>
    public static ReqT<M, IN, bool> Check(Func<IN, bool> fCheck) =>
        Input * fCheck;

    /// <summary>
    ///
    /// </summary>
    /// <param name="fError"></param>
    /// <returns></returns>
    public static ReqT<M, IN, Unit> Prescribe(Func<IN, Error> fError) =>
        Input * fError >> Append;

    /// <summary>
    ///
    /// </summary>
    /// <param name="fCheck"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static ReqT<M, IN, Unit> Ensure(
        Func<IN, bool> fCheck,
        Func<IN, Error> Fail) =>
        +iff(Check(fCheck), Then: ReqT.Ok, Else: Prescribe(Fail));

    /// <summary>
    ///
    /// </summary>
    /// <param name="fCheck"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static ReqT<M, IN, Unit> Ensure(
        Func<IN, K<M, bool>> fCheck,
        Func<IN, Error> Fail) =>
        +iff(Check(fCheck), Then: ReqT.Ok, Else: Prescribe(Fail));

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="OUT"></typeparam>
    /// <param name="fOut"></param>
    /// <returns></returns>
    public static ReqT<M, IN, OUT> Transform<OUT>(Func<IN, OUT> fOut) =>
        Input * fOut;
}
