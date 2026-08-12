namespace VSlices.Domain.ValueObjects;

/// <summary>
///
/// </summary>
public readonly struct Time :
    Magnitude<Time, double>,
    Transform<Time, double>
{
    readonly double Value;

    internal Time(double value) =>
        Value = value;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() =>
        Value + " s";

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Time other) =>
        Value.Equals(other.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    public bool Equals(Time other, double epsilon) =>
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
        obj is Time time && Equals(time);

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
            Time other => CompareTo(other),
            _ => throw new ArgumentException($"must be of type {nameof(Time)}")
        };

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Time other) =>
        Value.CompareTo(other.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Time Add(Time rhs) =>
        new(Value + rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Time Subtract(Time rhs) =>
        new(Value - rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Time Multiply(double rhs) =>
        new(Value * rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Time Divide(double rhs) =>
        new(Value / rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Time operator *(Time lhs, double rhs) =>
        lhs.Multiply(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Time operator *(double lhs, Time rhs) =>
        rhs.Multiply(lhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static TimeSq operator *(Time lhs, Time rhs) =>
        new(lhs.Value * rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="power"></param>
    /// <returns></returns>
    public static TimeSq operator ^(Time lhs, int power) =>
        power == 2
            ? new TimeSq(lhs.Value * lhs.Value)
            : raise<TimeSq>(new NotSupportedException("Time can only be raised to the power of 2"));

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Time operator /(Time lhs, double rhs) =>
        lhs.Divide(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Time operator +(Time lhs, Time rhs) =>
        lhs.Add(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static DateTime operator +(DateTime lhs, Time rhs) =>
        lhs.AddSeconds(rhs.Seconds);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Time operator -(Time lhs, Time rhs) =>
        lhs.Subtract(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static DateTime operator -(DateTime lhs, Time rhs) =>
        lhs.AddSeconds(-rhs.Seconds);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static double operator /(Time lhs, Time rhs) =>
        lhs.Value / rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(Time lhs, Time rhs) =>
        lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(Time lhs, Time rhs) =>
        !lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(Time lhs, Time rhs) =>
        lhs.Value > rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(Time lhs, Time rhs) =>
        lhs.Value < rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(Time lhs, Time rhs) =>
        lhs.Value >= rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(Time lhs, Time rhs) =>
        lhs.Value <= rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="power"></param>
    /// <returns></returns>
    public Time Pow(double power) =>
        new(Math.Pow(Value, power));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Time Round() =>
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
    public Time Abs() =>
        new(Math.Abs(Value));

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Time Min(Time rhs) =>
        new(Math.Min(Value, rhs.Value));

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Time Max(Time rhs) =>
        new(Math.Max(Value, rhs.Value));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public TimeSpan ToTimeSpan() =>
        TimeSpan.FromSeconds(Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator TimeSpan(Time value) =>
        value.ToTimeSpan();

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator Time(TimeSpan value) =>
        new(value.TotalSeconds);

    /// <summary>
    ///
    /// </summary>
    public double Seconds => Value;

    /// <summary>
    ///
    /// </summary>
    public double Milliseconds => Value * 1000.0;

    /// <summary>
    ///
    /// </summary>
    public double Minutes => Value / 60.0;

    /// <summary>
    ///
    /// </summary>
    public double Hours => Value / 3600.0;

    /// <summary>
    ///
    /// </summary>
    public double Days => Value / 86400.0;

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Time operator -(Time value) =>
        new(-value.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr"></param>
    /// <returns></returns>
    public Time From(double repr) => new(repr);

    /// <summary>
    ///
    /// </summary>
    static Req<double, Time> Transform<Time, Time, double>.Invariants { get; } =
        Req.Transform(Time (double v) => new Time(v));

    /// <summary>
    ///
    /// </summary>
    public static Time AdditiveIdentity { get; } = new(0);

}

/// <summary>
///
/// </summary>
public static class UnitsTimeExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Time Milliseconds(this int self) =>
        new(self / 1000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Time Milliseconds(this float self) =>
        new(self / 1000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Time Milliseconds(this double self) =>
        new(self / 1000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Time Seconds(this int self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Time Seconds(this float self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Time Seconds(this double self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Time Minutes(this int self) =>
        new(self * 60.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Time Minutes(this float self) =>
        new(self * 60.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Time Minutes(this double self) =>
        new(self * 60.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Time Hours(this int self) =>
        new(self * 3600.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Time Hours(this float self) =>
        new(self * 3600.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Time Hours(this double self) =>
        new(self * 3600.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Time Days(this int self) =>
        new(self * 86400.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Time Days(this float self) =>
        new(self * 86400.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Time Days(this double self) =>
        new(self * 86400.0);
}
