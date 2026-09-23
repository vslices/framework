using VSlices.Space.Quantities;
using VSlices.Space.Temporal;
using VSlices.Work;

namespace VSlices.Grounding;

/// <summary>
/// Grounds clock observation and temporal delay in a concrete <see cref="TimeProvider"/>.
/// </summary>
public sealed class SystemTimeIO(TimeProvider timeProvider) :
    ClockIO,
    DelayIO
{
    public IO<Moment> Now =>
        IO.lift(() => new Moment(timeProvider.GetLocalNow()));

    public IO<Unit> For(Duration<double> duration) =>
        duration.Value <= 0
            ? Prelude.unitIO
            : Prelude.liftIO(
                async env =>
                    await Task
                        .Delay(
                            TimeSpan.FromSeconds(duration.Value),
                            timeProvider,
                            env.Token)
                        .ConfigureAwait(false));

    public IO<Unit> Until(Moment moment) =>
        from now in Now
        from _ in moment.Value <= now.Value
            ? Prelude.unitIO
            : For(moment - now)
        select _;
}
