namespace VSlices.Domain.ValueObjects;

/// <summary>
///
/// </summary>
/// <remarks>
///
/// </remarks>
public readonly struct Accel : Magnitude<Accel, double>
{
    readonly double Value;

    internal Accel(double value) =>
        Value = value;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() =>
        Value + " m/s²";

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Accel other) =>
        Value.Equals(other.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    public bool Equals(Accel other, double epsilon) =>
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
        obj is Accel accel && Equals(accel);

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
            Accel other => CompareTo(other),
            _ => throw new ArgumentException($"must be of type {nameof(Accel)}")
        };

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Accel other) =>
        Value.CompareTo(other.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Accel Add(Accel rhs) =>
        new(Value + rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Accel Subtract(Accel rhs) =>
        new(Value - rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Accel Multiply(double rhs) =>
        new(Value * rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Accel Divide(double rhs) =>
        new(Value / rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Accel operator *(Accel lhs, double rhs) =>
        lhs.Multiply(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Accel operator *(double lhs, Accel rhs) =>
        rhs.Multiply(lhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Velocity operator *(Accel lhs, Time rhs) =>
        new(lhs.Value * rhs.Seconds);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Velocity operator *(Time lhs, Accel rhs) =>
        new(lhs.Seconds * rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Accel operator +(Accel lhs, Accel rhs) =>
        lhs.Add(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Accel operator -(Accel lhs, Accel rhs) =>
        lhs.Subtract(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Accel operator /(Accel lhs, double rhs) =>
        lhs.Divide(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static double operator /(Accel lhs, Accel rhs) =>
        lhs.Value / rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(Accel lhs, Accel rhs) =>
        lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(Accel lhs, Accel rhs) =>
        !lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(Accel lhs, Accel rhs) =>
        lhs.Value > rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(Accel lhs, Accel rhs) =>
        lhs.Value < rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(Accel lhs, Accel rhs) =>
        lhs.Value >= rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(Accel lhs, Accel rhs) =>
        lhs.Value <= rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="power"></param>
    /// <returns></returns>
    public Accel Pow(double power) =>
        new(Math.Pow(Value, power));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Accel Round() =>
        new(Math.Round(Value));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Accel Sqrt() =>
        new(Math.Sqrt(Value));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Accel Abs() =>
        new(Math.Abs(Value));

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Accel Min(Accel rhs) =>
        new(Math.Min(Value, rhs.Value));

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Accel Max(Accel rhs) =>
        new(Math.Max(Value, rhs.Value));

    /// <summary>
    ///
    /// </summary>
    public double MetresPerSecond2 => Value;


    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Accel operator -(Accel value) =>
        new(-value.Value);

    /// <summary>
    ///
    /// </summary>
    public static Accel AdditiveIdentity { get; } = new(0);
}

/// <summary>
///
/// </summary>
public static class UnitsAccelExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Accel MetresPerSecond2(this int self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Accel MetresPerSecond2(this float self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Accel MetresPerSecond2(this double self) =>
        new(self);
}
