namespace VSlices.Domain.Envs;

/// <summary>
/// Represents an interface for clock-related operations, providing functionality
/// to retrieve the current time and manage delays or sleep operations.
/// </summary>
public interface ClockIO
{
    /// <summary>
    /// Gets an <see cref="IO{T}"/> instance that represents the current date and time.
    /// </summary>
    /// <remarks>
    /// The returned value is typically used to retrieve the current time in a functional programming context.
    /// </remarks>
    IO<DateTimeOffset> Now { get; }

    /// <summary>
    /// Suspends the execution of the current operation until the specified date and time.
    /// </summary>
    /// <param name="dt">The <see cref="DateTimeOffset"/> representing the date and time to sleep until.</param>
    /// <returns>An <see cref="IO{Unit}"/> representing the result of the sleep operation.</returns>
    IO<Unit> SleepUntil(DateTimeOffset dt);

    /// <summary>
    /// Suspends the execution of the current operation for the specified duration.
    /// </summary>
    /// <param name="ts">The <see cref="TimeSpan"/> representing the duration to sleep for.</param>
    /// <returns>An <see cref="IO{Unit}"/> representing the result of the sleep operation.</returns>
    IO<Unit> SleepFor(TimeSpan ts);
}

/// <summary>
/// Provides extension methods for the <see cref="ClockIO"/> interface, enabling additional
/// functionality for clock-related operations.
/// </summary>
public static class ClockIOExtensions
{
    extension(ClockIO io)
    {
        /// <summary>
        /// Gets the current time in Coordinated Universal Time (UTC).
        /// </summary>
        /// <remarks>
        /// This property retrieves the current time from the underlying <see cref="ClockIO.Now"/> 
        /// and converts it to UTC using <see cref="DateTimeOffset.ToUniversalTime"/>.
        /// </remarks>
        public IO<DateTimeOffset> UtcNow => io.Now.Map(d => d.ToUniversalTime());
    }
}