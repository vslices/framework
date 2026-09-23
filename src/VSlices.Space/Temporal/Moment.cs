using VSlices.Space.Quantities;

namespace VSlices.Space.Temporal;

/// <summary>
/// A temporal point. Moment is not a duration and does not carry a scalar origin;
/// it inhabits an affine space translated by Duration&lt;double&gt;.
///
/// The semantic surface is intentionally pure: obtaining the current time belongs
/// to Grounding/Clock capabilities, not to Moment itself.
/// </summary>
public sealed record Moment(DateTimeOffset Value) :
    AffineSpace<Moment, Duration<double>, double>
{
    public static Moment operator +(Moment point, Duration<double> displacement) =>
        new(point.Value.AddSeconds(displacement.Value));

    public static Moment operator -(Moment point, Duration<double> displacement) =>
        new(point.Value.AddSeconds(-displacement.Value));

    public static Duration<double> operator -(Moment left, Moment right) =>
        new((left.Value - right.Value).TotalSeconds);

    public DateTimeOffset ToDateTimeOffset() => Value;
}
