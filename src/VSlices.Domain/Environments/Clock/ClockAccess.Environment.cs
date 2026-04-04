using VSlices.Domain.Interfaces;

namespace VSlices.Domain.Environments.Clock;

public interface HasClockAccess<TSelf> : Has<Eff<TSelf>, ClockAccessIO>;

public record ClockAccessEnv<RT>
    where RT : HasClockAccess<RT>
{
    protected static Eff<RT, ClockAccessIO> clockAccessIO =>
        Has<Eff<RT>, RT, ClockAccessIO>.ask.As();

    public static Eff<RT, DateTimeOffset> getNow =>
        clockAccessIO.Bind(io => io.Now);

    public static Eff<RT, DateTimeOffset> getUtcNow =>
        clockAccessIO.Bind(io => io.UtcNow);

    public static Eff<RT, DateOnly> getDate =>
        clockAccessIO.Bind(io => io.Now).Map(dt => DateOnly.FromDateTime(dt.DateTime));

    public static Eff<RT, TimeOnly> getTime =>
        clockAccessIO.Bind(io => io.Now).Map(dt => TimeOnly.FromDateTime(dt.DateTime));

    public static Eff<RT, DateOnly> getUtcDate =>
        clockAccessIO.Bind(io => io.UtcNow).Map(dt => DateOnly.FromDateTime(dt.DateTime));

    public static Eff<RT, TimeOnly> getUtcTime =>
        clockAccessIO.Bind(io => io.UtcNow).Map(dt => TimeOnly.FromDateTime(dt.DateTime));

    public static Eff<RT, Unit> sleepUntil(DateTimeOffset dt) =>
        clockAccessIO.Bind(io => io.SleepUntil(dt));

    public static Eff<RT, Unit> sleepFor(TimeSpan ts) =>
       clockAccessIO.Bind(io => io.SleepFor(ts));

}
