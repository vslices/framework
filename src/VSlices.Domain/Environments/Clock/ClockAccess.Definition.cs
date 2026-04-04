namespace VSlices.Domain.Environments.Clock;

public interface ClockAccessIO
{
    IO<DateTimeOffset> Now { get; }

    IO<DateTimeOffset> UtcNow { get; }

    IO<Unit> SleepUntil(DateTimeOffset dt);

    IO<Unit> SleepFor(TimeSpan ts);
}
