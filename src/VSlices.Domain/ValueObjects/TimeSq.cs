namespace VSlices.Domain.ValueObjects;

/// <summary>
///
/// </summary>
public readonly struct TimeSq :
    Magnitude<TimeSq, double>,
    Transform<TimeSq, double>
{
    readonly double Value;

    internal TimeSq(double value) =>
        Value = value;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() =>
        Value + " s²";

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(TimeSq other) =>
        Value.Equals(other.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    public bool Equals(TimeSq other, double epsilon) =>
        Math.Abs(other.Value - Value) < epsilon;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public double To() => Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) =>
        obj is TimeSq sq && Equals(sq);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() =>
        Value.GetHashCode();

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public int CompareTo(object? obj) =>
        obj switch
        {
            null => 1,
            TimeSq other => CompareTo(other),
            _ => throw new ArgumentException($"must be of type {nameof(TimeSq)}")
        };

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(TimeSq other) =>
        Value.CompareTo(other.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public TimeSq Add(TimeSq rhs) =>
        new(Value + rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public TimeSq Subtract(TimeSq rhs) =>
        new(Value - rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public TimeSq Multiply(double rhs) =>
        new(Value * rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public TimeSq Divide(double rhs) =>
        new(Value / rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static TimeSq operator *(TimeSq lhs, double rhs) =>
        lhs.Multiply(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static TimeSq operator *(double lhs, TimeSq rhs) =>
        rhs.Multiply(lhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static TimeSq operator /(TimeSq lhs, double rhs) =>
        lhs.Divide(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static TimeSq operator +(TimeSq lhs, TimeSq rhs) =>
        lhs.Add(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static TimeSq operator -(TimeSq lhs, TimeSq rhs) =>
        lhs.Subtract(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static double operator /(TimeSq lhs, TimeSq rhs) =>
        lhs.Value / rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(TimeSq lhs, TimeSq rhs) =>
        lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(TimeSq lhs, TimeSq rhs) =>
        !lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(TimeSq lhs, TimeSq rhs) =>
        lhs.Value > rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(TimeSq lhs, TimeSq rhs) =>
        lhs.Value < rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(TimeSq lhs, TimeSq rhs) =>
        lhs.Value >= rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(TimeSq lhs, TimeSq rhs) =>
        lhs.Value <= rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public TimeSq Round() =>
        new(Math.Round(Value));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Time Sqrt() =>
        new(Math.Sqrt(Value));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public TimeSq Abs() =>
        new(Math.Abs(Value));

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public TimeSq Min(TimeSq rhs) =>
        new(Math.Min(Value, rhs.Value));

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public TimeSq Max(TimeSq rhs) =>
        new(Math.Max(Value, rhs.Value));

    /// <summary>
    ///
    /// </summary>
    public double Seconds2 => Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TimeSq operator -(TimeSq value) =>
        new(-value.Value);


    /// <summary>
    ///
    /// </summary>
    /// <param name="repr"></param>
    /// <returns></returns>
    public static TimeSq From(double repr) =>
        new(repr);

    /// <summary>
    ///
    /// </summary>
    static Req<double, TimeSq> Transform<TimeSq, TimeSq, double>.Invariants { get; } =
        Req.Transform(TimeSq (double v) => new TimeSq(v));

    /// <summary>
    ///
    /// </summary>
    public static TimeSq AdditiveIdentity { get; } = new(0);
}
