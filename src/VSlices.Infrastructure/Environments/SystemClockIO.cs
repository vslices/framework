using LanguageExt;
using VSlices.Domain.Envs.Clock;

namespace VSlices.Infrastructure.Environments;

public sealed class SystemClockIO(TimeProvider timeProvider) : ClockIO
{
    public IO<DateTimeOffset> Now => IO.lift(timeProvider.GetLocalNow);

    public IO<DateTimeOffset> UtcNow => IO.lift(timeProvider.GetUtcNow);

    public IO<Unit> SleepFor(TimeSpan ts) =>
        Prelude.liftIO(async env => await Task.Delay(ts, env.Token).ConfigureAwait(false));

    public IO<Unit> SleepUntil(DateTimeOffset dt) => 
        from now in Now
        from res in dt <= now
            ? Prelude.unitIO
            : Prelude.liftIO(async env => await Task.Delay(dt - now, env.Token).ConfigureAwait(false))
        select res;

}
