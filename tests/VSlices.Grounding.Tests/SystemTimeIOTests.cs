using VSlices.Space.Quantities;
using VSlices.Space.Temporal;
using Xunit;

namespace VSlices.Grounding.Tests;

public sealed class SystemTimeIOTests
{
    [Fact]
    public async Task Now_materializes_the_TimeProvider_value_as_a_Moment()
    {
        var expected =
            new DateTimeOffset(
                2026,
                9,
                23,
                12,
                34,
                56,
                TimeSpan.Zero);

        var time =
            new SystemTimeIO(
                new FixedTimeProvider(expected));

        var now = await time.Now.RunAsync();

        Assert.Equal(expected, now.Value);
    }

    [Fact]
    public async Task Non_positive_and_already_elapsed_delays_complete_without_waiting()
    {
        var now =
            new DateTimeOffset(
                2026,
                9,
                23,
                12,
                34,
                56,
                TimeSpan.Zero);

        var time =
            new SystemTimeIO(
                new FixedTimeProvider(now));

        await time
            .For(new Duration<double>(0))
            .RunAsync();

        await time
            .Until(new Moment(now.AddSeconds(-1)))
            .RunAsync();
    }
}

public sealed class FixedTimeProvider(DateTimeOffset now) :
    TimeProvider
{
    public override DateTimeOffset GetUtcNow() => now;

    public override TimeZoneInfo LocalTimeZone =>
        TimeZoneInfo.Utc;
}
