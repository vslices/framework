namespace VSlices.Domain.ValueObjects;

/// <summary>
///
/// </summary>
public readonly struct Length : Magnitude<Length, double>
{
    readonly double Value;

    internal Length(double value) =>
        Value = value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr"></param>
    /// <returns></returns>
    public Length New(double repr) => new(repr);

    /// <summary>
    ///
    /// </summary>
    /// <returns>
    ///
    /// </returns>
    public override string ToString() =>
        Value + " m";

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Length other) =>
        Value.Equals(other.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    public bool Equals(Length other, double epsilon) =>
        Math.Abs(other.Value - Value) < epsilon;

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) =>
        obj is Length length && Equals(length);
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
            Length other => CompareTo(other),
            _ => throw new ArgumentException($"must be of type {nameof(Length)}")
        };

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Length other) =>
        Value.CompareTo(other.Value);
    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Length Add(Length rhs) =>
        new(Value + rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Length Subtract(Length rhs) =>
        new(Value - rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Length Multiply(double rhs) =>
        new(Value * rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Length Divide(double rhs) =>
        new(Value / rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Area operator *(Length lhs, Length rhs) =>
        new(lhs.Value * rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Length operator *(Length lhs, double rhs) =>
        lhs.Multiply(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Length operator *(double lhs, Length rhs) =>
        rhs.Multiply(lhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length operator -(Length self) =>
        new(-self.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Length operator +(Length lhs, Length rhs) =>
        lhs.Add(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Length operator -(Length lhs, Length rhs) =>
        lhs.Subtract(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Length operator /(Length lhs, double rhs) =>
        lhs.Divide(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static double operator /(Length lhs, Length rhs) =>
        lhs.Value / rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Time operator /(Length lhs, Velocity rhs) =>
        new Time(lhs.Metres / rhs.MetresPerSecond);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Velocity operator /(Length lhs, Time rhs) =>
        new Velocity(lhs.Value / rhs.Seconds);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(Length lhs, Length rhs) =>
        lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(Length lhs, Length rhs) =>
        !lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(Length lhs, Length rhs) =>
        lhs.Value > rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(Length lhs, Length rhs) =>
        lhs.Value < rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(Length lhs, Length rhs) =>
        lhs.Value >= rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(Length lhs, Length rhs) =>
        lhs.Value <= rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="power"></param>
    /// <returns></returns>
    public Length Pow(double power) =>
        new(Math.Pow(Value, power));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Length Round() =>
        new(Math.Round(Value));
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Length Sqrt() =>
        new(Math.Sqrt(Value));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Length Abs() =>
        new(Math.Abs(Value));

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Length Min(Length rhs) =>
        new(Math.Min(Value, rhs.Value));

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Length Max(Length rhs) =>
        new(Math.Max(Value, rhs.Value));

    /// <summary>
    ///
    /// </summary>
    public double Miles => Value * 6.2137119223484848484848484848485e-4;

    /// <summary>
    ///
    /// </summary>
    public double NauticalMiles => Value * 1852.0;

    /// <summary>
    ///
    /// </summary>
    public double Yards => Value * 1.0936132983333333333333333333333;

    /// <summary>
    ///
    /// </summary>
    public double Feet => Value * 3.280839895;

    /// <summary>
    ///
    /// </summary>
    public double Inches => Value * 39.37007874;

    /// <summary>
    ///
    /// </summary>
    public double Kilometres => Value / 1000.0;

    /// <summary>
    ///
    /// </summary>
    public double Hectometres => Value / 100.0;

    /// <summary>
    ///
    /// </summary>
    public double Decametres => Value / 10.0;

    /// <summary>
    ///
    /// </summary>
    public double Metres => Value;

    /// <summary>
    ///
    /// </summary>
    public double Centimetres => Value * 100.0;

    /// <summary>
    ///
    /// </summary>
    public double Millimetres => Value * 1000.0;

    /// <summary>
    ///
    /// </summary>
    public double Micrometres => Value * 1000000.0;

    /// <summary>
    ///
    /// </summary>
    public double Nanometres => Value * 1000000000.0;

    /// <summary>
    ///
    /// </summary>
    public double Angstroms => Value * 10000000000.0;

    /// <summary>
    ///
    /// </summary>
    public static Length AdditiveIdentity { get; } = new Length(0);
}

/// <summary>
///
/// </summary>
public static class UnitsLengthExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Miles(this int self) =>
        new(1609.344000006437376000025749504 * self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Miles(this float self) =>
        new(1609.344000006437376000025749504 * self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Miles(this double self) =>
        new(1609.344000006437376000025749504 * self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length NauticalMiles(this int self) =>
        new(self / 1852.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length NauticalMiles(this float self) =>
        new(self / 1852.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length NauticalMiles(this double self) =>
        new(self / 1852.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Yards(this int self) =>
        new(0.9144000000036576000000146304 * self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Yards(this float self) =>
        new(0.9144000000036576000000146304 * self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Yards(this double self) =>
        new(0.9144000000036576000000146304 * self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Feet(this int self) =>
        new(0.3048000000012192000000048768 * self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Feet(this float self) =>
        new(0.3048000000012192000000048768 * self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Feet(this double self) =>
        new(0.3048000000012192000000048768 * self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Inches(this int self) =>
        new(0.0254000000001016000000004064 * self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Inches(this float self) =>
        new(0.0254000000001016000000004064 * self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Inches(this double self) =>
        new(0.0254000000001016000000004064 * self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Kilometres(this int self) =>
        new(1000.0 * self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Kilometres(this float self) =>
        new(1000.0 * self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Kilometres(this double self) =>
        new(1000.0 * self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Metres(this int self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Metres(this float self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Metres(this double self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Centimetres(this int self) =>
        new(self / 100.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Centimetres(this float self) =>
        new(self / 100.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Centimetres(this double self) =>
        new(self / 100.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Millimetres(this int self) =>
        new(self / 1000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Millimetres(this float self) =>
        new(self / 1000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Millimetres(this double self) =>
        new(self / 1000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Micrometres(this int self) =>
        new(self / 1000000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Micrometres(this float self) =>
        new(self / 1000000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Micrometres(this double self) =>
        new(self / 1000000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Nanometres(this int self) =>
        new(self / 1000000000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Nanometres(this float self) =>
        new(self / 1000000000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Nanometres(this double self) =>
        new(self / 1000000000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Angstroms(this int self) =>
        new(self / 10000000000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Angstroms(this float self) =>
        new(self / 10000000000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length Angstroms(this double self) =>
        new(self / 10000000000.0);
}
