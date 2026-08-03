using System.Net.Cache;

namespace VSlices.Monads;

/// <summary>
///
/// </summary>
/// <typeparam name="IN"></typeparam>
public partial class Req<IN>
{
    public static Req<IN, IN> Input =>
        Readable.ask<Req<IN>, IN>().As();

    public static Req<IN, Unit> Append(Error e) =>
        +Writable.tell<Req<IN>, Error>(e);

    public static Req<IN, bool> Check(Func<IN, bool> fCheck) =>
        Input * fCheck;

    public static Req<IN, Unit> Prescribe(Func<IN, Error> fError) =>
        Input * fError >> Append;

    public static Req<IN, Unit> Ensure(
        Func<IN, bool> fCheck,
        Func<IN, Error> Fail) =>
        +iff(Check(fCheck), Then: Req.Ok, Else: Prescribe(Fail));

    public static Req<IN, OUT> Transform<OUT>(Func<IN, OUT> fOut) =>
        Input * fOut;
}
