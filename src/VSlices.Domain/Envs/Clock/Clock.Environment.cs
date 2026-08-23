using LanguageExt;

namespace VSlices.Domain.Envs;

/// <summary>
/// Represents an interface that provides access to clock-related operations 
/// within a specific runtime environment.
/// </summary>
/// <typeparam name="RT">
/// The type of the runtime environment implementing this interface, 
/// which must also satisfy the <see cref="HasClock{RT}"/> constraint.
/// </typeparam>
public interface HasClock<RT> : Has<Eff<RT>, ClockIO>;

/// <summary>
/// Represents a runtime environment that provides clock-related operations,
/// such as retrieving the current time and managing delays or sleep operations.
/// </summary>
/// <typeparam name="RT">
/// The type of the runtime environment implementing this record, 
/// which must satisfy the <see cref="HasClock{RT}"/> constraint.
/// </typeparam>
public sealed record ClockEnv<RT>
    where RT : HasClock<RT>
{
    private static Eff<RT, ClockIO> clockAccessIO =>
        Has<Eff<RT>, RT, ClockIO>.ask.As();

    /// <summary>
    /// Gets an effect that retrieves the current date and time as a <see cref="DateTimeOffset"/> 
    /// within the runtime environment.
    /// </summary>
    public static Eff<RT, DateTimeOffset> getNow =>
        clockAccessIO.Bind(io => io.Now);

    /// <summary>
    /// Gets an effect that retrieves the current UTC date and time from the clock
    /// within the runtime environment.
    /// </summary>
    public static Eff<RT, DateTimeOffset> getUtcNow =>
        clockAccessIO.Bind(io => io.UtcNow);

    /// <summary>
    /// Gets an effect that retrieves the current date from the runtime environment's clock.
    /// </summary>
    public static Eff<RT, DateOnly> getDate =>
        clockAccessIO.Bind(io => io.Now).Map(dt => DateOnly.FromDateTime(dt.DateTime));

    /// <summary>
    /// Gets an effect that retrieves the current time as a <see cref="TimeOnly"/> value 
    /// from the runtime environment.
    /// </summary>
    public static Eff<RT, TimeOnly> getTime =>
        clockAccessIO.Bind(io => io.Now).Map(dt => TimeOnly.FromDateTime(dt.DateTime));

    /// <summary>
    /// Gets the current UTC date as a <see cref="DateOnly"/> value.
    /// </summary>
    public static Eff<RT, DateOnly> getUtcDate =>
        clockAccessIO.Bind(io => io.UtcNow).Map(dt => DateOnly.FromDateTime(dt.DateTime));

    /// <summary>
    /// Gets an effect that retrieves the current UTC time as a <see cref="TimeOnly"/> value
    /// within the runtime environment.
    /// </summary>
    /// <remarks>
    /// This property accesses the clock-related operations of the runtime environment
    /// to obtain the current UTC time and maps it to a <see cref="TimeOnly"/> representation.
    /// </remarks>
    /// <value>
    /// An <see cref="Eff{RT, TimeOnly}"/> representing the effect of retrieving the current UTC time.
    /// </value>
    public static Eff<RT, TimeOnly> getUtcTime =>
        clockAccessIO.Bind(io => io.UtcNow).Map(dt => TimeOnly.FromDateTime(dt.DateTime));

    /// <summary>
    /// Suspends the execution of the current operation until the specified date and time.
    /// </summary>
    /// <param name="dt">
    /// The <see cref="DateTimeOffset"/> representing the date and time to sleep until.
    /// </param>
    /// <returns>
    /// An <see cref="Eff{RT, Unit}"/> representing the result of the sleep operation.
    /// </returns>
    /// <remarks>
    /// This method delegates the sleep operation to the underlying <see cref="ClockIO"/> implementation
    /// within the runtime environment.
    /// </remarks>
    public static Eff<RT, Unit> sleepUntil(DateTimeOffset dt) =>
        clockAccessIO.Bind(io => io.SleepUntil(dt));

    /// <summary>
    /// Suspends the execution of the current operation for the specified duration.
    /// </summary>
    /// <param name="ts">
    /// The <see cref="TimeSpan"/> representing the duration to sleep for.
    /// </param>
    /// <returns>
    /// An <see cref="Eff{RT, Unit}"/> representing the result of the sleep operation.
    /// </returns>
    /// <remarks>
    /// This method delegates the sleep operation to the underlying <see cref="ClockIO"/> implementation
    /// within the runtime environment.
    /// </remarks>
    public static Eff<RT, Unit> sleepFor(TimeSpan ts) =>
       clockAccessIO.Bind(io => io.SleepFor(ts));

}
