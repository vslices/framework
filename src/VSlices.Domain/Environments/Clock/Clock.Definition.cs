namespace VSlices.Domain.Environments.Clock;

public interface ClockIO
{
    IO<DateTimeOffset> Now { get; }

    IO<Unit> SleepUntil(DateTimeOffset dt);

    IO<Unit> SleepFor(TimeSpan ts);
}

public static class ClockIOExtensions
{
    extension(ClockIO io)
    {
        public IO<DateTimeOffset> UtcNow => io.Now.Map(d => d.ToUniversalTime());
    }
}