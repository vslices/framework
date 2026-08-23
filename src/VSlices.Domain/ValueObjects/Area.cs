using VSlices.Monads;

namespace VSlices.Domain.ValueObjects;

/// <summary>
///
/// </summary>
/// <remarks>
///
/// </remarks>
public readonly struct Area : Magnitude<Area, double>
{
    readonly double Value;

    internal Area(double value) =>
        Value = value;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Area New(double value) =>
        new(value);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() =>
        Value + " m²";

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Area other) =>
        Value.Equals(other.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    public bool Equals(Area other, double epsilon) =>
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
        obj is Area area && Equals(area);

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
            Area other => CompareTo(other),
            _ => throw new ArgumentException($"must be of type {nameof(Area)}")
        };

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Area other) =>
        Value.CompareTo(other.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Area Add(Area rhs) =>
        new(Value + rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Area Subtract(Area rhs) =>
        new(Value - rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Area Multiply(double rhs) =>
        new(Value * rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Area Divide(double rhs) =>
        new(Value / rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Area operator *(Area lhs, double rhs) =>
        lhs.Multiply(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Area operator *(double lhs, Area rhs) =>
        rhs.Multiply(lhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Area operator /(Area lhs, double rhs) =>
        lhs.Divide(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Area operator +(Area lhs, Area rhs) =>
        lhs.Add(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Area operator -(Area lhs, Area rhs) =>
        lhs.Subtract(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Length operator /(Area lhs, Length rhs) =>
        new Length(lhs.Value / rhs.Metres);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static double operator /(Area lhs, Area rhs) =>
        lhs.Value / rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(Area lhs, Area rhs) =>
        lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(Area lhs, Area rhs) =>
        !lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(Area lhs, Area rhs) =>
        lhs.Value > rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(Area lhs, Area rhs) =>
        lhs.Value < rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(Area lhs, Area rhs) =>
        lhs.Value >= rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(Area lhs, Area rhs) =>
        lhs.Value <= rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="power"></param>
    /// <returns></returns>
    public Area Pow(double power) =>
        new Area(Math.Pow(Value, power));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Area Round() =>
        new Area(Math.Round(Value));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Area Sqrt() =>
        new Area(Math.Sqrt(Value));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Length Abs() =>
        new Length(Math.Abs(Value));

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Area Min(Area rhs) =>
        new Area(Math.Min(Value, rhs.Value));

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Area Max(Area rhs) =>
        new Area(Math.Max(Value, rhs.Value));

    /// <summary>
    ///
    /// </summary>
    public double SqKilometres => Value * 0.000001;

    /// <summary>
    ///
    /// </summary>
    public double SqMetres => Value;

    /// <summary>
    ///
    /// </summary>
    public double SqCentimetres => Value * 10000.0;

    /// <summary>
    ///
    /// </summary>
    public double SqMillimetres => Value * 1000000.0;

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Area operator -(Area value) =>
        new Area(-value.Value);

    /// <summary>
    ///
    /// </summary>
    public static Area AdditiveIdentity { get; } = new(0);
}

/// <summary>
///
/// </summary>
public static class UnitsAreaExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Area SqKilometres(this int self) =>
        new(self / 0.000001);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Area SqKilometres(this float self) =>
        new(self / 0.000001);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Area SqKilometres(this double self) =>
        new(self / 0.000001);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Area SqMetres(this int self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Area SqMetres(this float self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Area SqMetres(this double self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Area SqCentimetres(this int self) =>
        new(self / 10000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Area SqCentimetres(this float self) =>
        new(self / 10000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Area SqCentimetres(this double self) =>
        new(self / 10000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Area SqMillimetres(this int self) =>
        new(self / 1000000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Area SqMillimetres(this float self) =>
        new(self / 1000000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Area SqMillimetres(this double self) =>
        new(self / 1000000.0);
}
