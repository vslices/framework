using VSlices.Space.Quantities;
using VSlices.Space.Temporal;

namespace VSlices.Space.Tests;

public class MomentTests
{
    [Fact]
    public void default_duration_is_a_vector_space_displacement()
    {
        Assert.True(typeof(VectorSpace<Duration<double>, double>).IsAssignableFrom(typeof(Duration<double>)));

        var value = new Duration<double>(90d);

        Assert.Equal(value, value + Duration<double>.AdditiveIdentity);
        Assert.Equal(Duration<double>.AdditiveIdentity, value + (-value));
        Assert.Equal(new Duration<double>(180d), value * 2d);
        Assert.Equal(value, (value * 2d) / 2d);
    }

    [Fact]
    public void moment_is_an_affine_point_translated_by_duration()
    {
        var origin = new Moment(new DateTimeOffset(2026, 9, 15, 12, 0, 0, TimeSpan.Zero));
        var displacement = new Duration<double>(90d);

        var moved = origin + displacement;
        var measured = moved - origin;

        Assert.Equal(new DateTimeOffset(2026, 9, 15, 12, 1, 30, TimeSpan.Zero), moved.Value);
        Assert.Equal(displacement, measured);
        Assert.Equal(origin, moved - displacement);
    }

    [Fact]
    public void affine_translation_and_difference_round_trip()
    {
        var point = new Moment(new DateTimeOffset(2026, 9, 15, 12, 0, 0, TimeSpan.FromHours(-3)));
        var displacement = new Duration<double>(3600d);

        Assert.Equal(displacement, (point + displacement) - point);
        Assert.Equal(point, (point + displacement) - displacement);
    }

    [Fact]
    public void moment_equality_follows_instant_equality_not_display_offset()
    {
        var utc = new Moment(new DateTimeOffset(2026, 9, 15, 15, 0, 0, TimeSpan.Zero));
        var chile = new Moment(new DateTimeOffset(2026, 9, 15, 12, 0, 0, TimeSpan.FromHours(-3)));

        Assert.Equal(utc, chile);
        Assert.Equal(Duration<double>.AdditiveIdentity, utc - chile);
    }
}
