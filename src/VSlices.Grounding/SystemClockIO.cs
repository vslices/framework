using LanguageExt;
using VSlices.Work;

namespace VSlices.Grounding;

/// <summary>
/// Grounds the <see cref="ClockIO"/> capability in a concrete <see cref="TimeProvider"/>.
/// </summary>
public sealed class SystemClockIO(TimeProvider timeProvider) : ClockIO
{
    public IO<DateTimeOffset> Now => IO.lift(timeProvider.GetLocalNow);

    public IO<Unit> SleepFor(TimeSpan ts) =>
        Prelude.liftIO(async env => await Task.Delay(ts, env.Token).ConfigureAwait(false));

    public IO<Unit> SleepUntil(DateTimeOffset dt) =>
        from now in Now
        from res in dt <= now
            ? Prelude.unitIO
            : Prelude.liftIO(async env => await Task.Delay(dt - now, env.Token).ConfigureAwait(false))
        select res;
}
