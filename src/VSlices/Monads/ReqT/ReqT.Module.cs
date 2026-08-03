namespace VSlices.Monads;

public static partial class ReqT<M>
    where M : Monad<M>
{
    public static ReqT<M, IN, bool> Check<IN>(Func<IN, bool> fCheck) =>
        ReqT<M, IN>.Check(fCheck);

    public static ReqT<M, IN, Unit> Prescribe<IN>(Func<IN, Error> fError) =>
        ReqT<M, IN>.Prescribe(fError);

    public static ReqT<M, IN, Unit> Prescribe<IN>(Func<IN, string> fError) =>
        ReqT<M, IN>.Prescribe(i => Error.New(fError(i)));

    public static ReqT<M, IN, Unit> Ensure<IN>(
        Func<IN, bool> fCheck,
        Func<IN, Error> Fail) =>
        ReqT<M, IN>.Ensure(fCheck, Fail);

    public static ReqT<M, IN, OUT> Transform<IN, OUT>(Func<IN, OUT> fOut) =>
        ReqT<M, IN>.Transform(fOut);
}

public static partial class ReqT
{
    public static readonly Pure<Unit> Ok = Pure(unit);

}
