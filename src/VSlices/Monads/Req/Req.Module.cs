using System.ComponentModel.Design.Serialization;

namespace VSlices.Monads;

public static partial class Req
{
    public static readonly Pure<Unit> Ok = Pure(unit);

    public static Req<IN, bool> Check<IN>(Func<IN, bool> fCheck) =>
        Req<IN>.Check(fCheck);

    public static Req<IN, Unit> Prescribe<IN>(Func<IN, Error> fError) =>
        Req<IN>.Prescribe(fError);

    public static Req<IN, Unit> Prescribe<IN>(Func<IN, string> fError) =>
        Req<IN>.Prescribe(i => Error.New(fError(i)));

    public static Req<IN, Unit> Ensure<IN>(
        Func<IN, bool> fCheck,
        Func<IN, Error> Fail) =>
        Req<IN>.Ensure(fCheck, Fail);

    public static Req<IN, OUT> Transform<IN, OUT>(Func<IN, OUT> fOut) =>
        Req<IN>.Transform(fOut);
}
