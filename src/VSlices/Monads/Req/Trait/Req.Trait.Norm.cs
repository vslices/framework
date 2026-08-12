namespace VSlices.Monads;

public partial class Req<IN>
{
    /// <summary>
    ///
    /// </summary>
    public static Req<IN, IN> Input =>
        Req<IN, IN>.Input;

    /// <summary>
    ///
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static Req<IN, Unit> Append(Error e) =>
        Input.Tell(e);

    /// <summary>
    ///
    /// </summary>
    /// <param name="fCheck"></param>
    /// <returns></returns>
    public static Req<IN, bool> Check(Func<IN, bool> fCheck) =>
        Input * fCheck;

    /// <summary>
    ///
    /// </summary>
    /// <param name="fError"></param>
    /// <returns></returns>
    public static Req<IN, Unit> Prescribe(Func<IN, Error> fError) =>
        Input * fError >> Append;

    /// <summary>
    ///
    /// </summary>
    /// <param name="fCheck"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, Unit> Ensure(
        Func<IN, bool> fCheck,
        Func<IN, Error> Fail) =>
        +iff(Check(fCheck), Then: Req.Ok, Else: Prescribe(Fail));

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="OUT"></typeparam>
    /// <param name="fOut"></param>
    /// <returns></returns>
    public static Req<IN, OUT> Transform<OUT>(Func<IN, OUT> fOut) =>
        Input * fOut;
}
