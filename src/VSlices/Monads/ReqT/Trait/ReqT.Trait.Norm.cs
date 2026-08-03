namespace VSlices.Monads;

/// <summary>
///
/// </summary>
/// <typeparam name="IN"></typeparam>
public partial class ReqT<M, IN>
{
    public static readonly ReqT<M, IN, IN> Input =
        ReqT<M, IN, IN>.Input;

    public static ReqT<M, IN, A> Lift<A>(K<M, A> ma) =>
        +MonadT.lift<ReqT<M, IN>, M, A>(ma);

    public static ReqT<M, IN, A> LiftIO<A>(IO<A> ma) =>
        +MonadIO.liftIO<ReqT<M, IN>, A>(ma);

    public static ReqT<M, IN, Unit> Append(Error e) =>
        Input.Tell(e);

    public static ReqT<M, IN, bool> Check(Func<IN, K<M, bool>> fCheck) =>
        Input >> (i => Lift(fCheck(i)));

    public static ReqT<M, IN, bool> Check(Func<IN, bool> fCheck) =>
        Input * fCheck;

    public static ReqT<M, IN, Unit> Prescribe(Func<IN, Error> fError) =>
        Input * fError >> Append;

    public static ReqT<M, IN, Unit> Ensure(
        Func<IN, bool> fCheck,
        Func<IN, Error> Fail) =>
        +iff(Check(fCheck), Then: ReqT.Ok, Else: Prescribe(Fail));

    public static ReqT<M, IN, Unit> Ensure(
        Func<IN, K<M, bool>> fCheck,
        Func<IN, Error> Fail) =>
        +iff(Check(fCheck), Then: ReqT.Ok, Else: Prescribe(Fail));

    public static ReqT<M, IN, OUT> Transform<OUT>(Func<IN, OUT> fOut) =>
        Input * fOut;
}
