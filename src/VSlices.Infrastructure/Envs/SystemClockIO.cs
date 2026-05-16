using LanguageExt;
using VSlices.Domain.Envs;

namespace VSlices.Infrastructure.Envs;

/// <summary>
/// A concrete implementation of the <see cref="ClockIO"/> interface, providing
/// clock-related operations such as retrieving the current time and managing delays
/// or sleep operations using a specified <see cref="TimeProvider"/>.
/// </summary>
/// <param name="timeProvider">The <see cref="TimeProvider"/> instance used to retrieve
/// the current time and manage time-related operations.</param>
public sealed class SystemClockIO(TimeProvider timeProvider) : ClockIO
{
    /// <inheritdoc />
    public IO<DateTimeOffset> Now => IO.lift(timeProvider.GetLocalNow);

    /// <inheritdoc />
    public IO<Unit> SleepFor(TimeSpan ts) =>
        Prelude.liftIO(async env => await Task.Delay(ts, env.Token).ConfigureAwait(false));

    /// <inheritdoc />
    public IO<Unit> SleepUntil(DateTimeOffset dt) => 
        from now in Now
        from res in dt <= now
            ? Prelude.unitIO
            : Prelude.liftIO(async env => await Task.Delay(dt - now, env.Token).ConfigureAwait(false))
        select res;

}
