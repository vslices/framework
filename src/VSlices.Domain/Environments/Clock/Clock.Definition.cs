namespace VSlices.Domain.Environments.Clock;

public interface ClockIO
{
    IO<DateTimeOffset> Now { get; }

    IO<DateTimeOffset> UtcNow { get; }

    IO<Unit> SleepUntil(DateTimeOffset dt);

    IO<Unit> SleepFor(TimeSpan ts);
}
