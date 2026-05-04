namespace VSlices.Domain.Environments.Clock;

public interface HasClock<RT> : Has<Eff<RT>, ClockIO>;

public record ClockEnv<RT>
    where RT : HasClock<RT>
{
    protected static Eff<RT, ClockIO> clockAccessIO =>
        Has<Eff<RT>, RT, ClockIO>.ask.As();

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
