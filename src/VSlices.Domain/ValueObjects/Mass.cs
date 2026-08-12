namespace VSlices.Domain.ValueObjects;

/// <summary>
///
/// </summary>
public readonly struct Mass :
    Magnitude<Mass, double>,
    Transform<Mass, double>
{
    readonly double Value;

    internal Mass(double value) =>
        Value = value;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() =>
        Kilograms + " kg";

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public int CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Mass other ? CompareTo(other)
                              : throw new ArgumentException($"must be of type {nameof(Mass)}");

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Mass other) =>
        Kilograms.CompareTo(other.Kilograms);

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Mass other) =>
        Kilograms.Equals(other.Kilograms);

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    public bool Equals(Mass other, double epsilon) =>
        Math.Abs(other.Kilograms - Kilograms) < epsilon;

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
        obj is Mass m && Equals(m);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() =>
        Kilograms.GetHashCode();

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Mass Add(Mass rhs) =>
        new(Kilograms + rhs.Kilograms);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Mass Subtract(Mass rhs) =>
        new(Kilograms - rhs.Kilograms);


    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Mass Multiply(double rhs) =>
        new(Kilograms * rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Mass Divide(double rhs) =>
        new(Kilograms / rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Mass operator *(Mass lhs, double rhs) =>
        lhs.Multiply(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Mass operator *(double lhs, Mass rhs) =>
        rhs.Multiply(lhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Mass operator +(Mass lhs, Mass rhs) =>
        lhs.Add(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Mass operator -(Mass lhs, Mass rhs) =>
        lhs.Subtract(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Mass operator /(Mass lhs, double rhs) =>
        lhs.Divide(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static double operator /(Mass lhs, Mass rhs) =>
        lhs.Kilograms / rhs.Kilograms;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(Mass lhs, Mass rhs) =>
        lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(Mass lhs, Mass rhs) =>
        !lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(Mass lhs, Mass rhs) =>
        lhs.Kilograms > rhs.Kilograms;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(Mass lhs, Mass rhs) =>
        lhs.Kilograms < rhs.Kilograms;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(Mass lhs, Mass rhs) =>
        lhs.Kilograms >= rhs.Kilograms;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(Mass lhs, Mass rhs) =>
        lhs.Kilograms <= rhs.Kilograms;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Mass Round() =>
        new(Math.Round(Kilograms));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Mass Sqrt() =>
        new(Math.Sqrt(Kilograms));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Mass Abs() =>
        new(Math.Abs(Kilograms));

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Mass Min(Mass rhs) =>
        new(Math.Min(Kilograms, rhs.Kilograms));

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Mass Max(Mass rhs) =>
        new(Math.Max(Kilograms, rhs.Kilograms));

    /// <summary>
    ///
    /// </summary>
    public double Grams => Value * 1000.0;

    /// <summary>
    ///
    /// </summary>
    public double Kilograms => Value;

    /// <summary>
    ///
    /// </summary>
    public double Tonnes => Value / 1000.0;

    /// <summary>
    ///
    /// </summary>
    public double Ounces => Pounds * 16.0;

    /// <summary>
    ///
    /// </summary>
    public double Pounds => Value * 2.2046226;

    /// <summary>
    ///
    /// </summary>
    public double Stones => Pounds / 14.0;

    /// <summary>
    ///
    /// </summary>
    public double ImperialTons => Value / 0.000984207;

    /// <summary>
    ///
    /// </summary>
    public double ShortTons => Value / 0.00110231;

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Mass operator -(Mass value) =>
        new(-value.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr"></param>
    /// <returns></returns>
    public Mass From(double repr) => new(repr);

    /// <summary>
    ///
    /// </summary>
    static Req<double, Mass> Transform<Mass, Mass, double>.Invariants { get; } =
        Req.Transform(Mass (double v) => new Mass(v));

    /// <summary>
    ///
    /// </summary>
    public static Mass AdditiveIdentity { get; } = new(0);
}

/// <summary>
///
/// </summary>
public static class UnitsMassExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Grams(this int self) =>
        new(self / 1000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Grams(this double self) =>
        new(self / 1000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Grams(this float self) =>
        new(self / 1000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Kilograms(this int self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Kilograms(this double self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Kilograms(this float self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Tonnes(this int self) =>
        new(self * 1000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Tonnes(this double self) =>
        new(self * 1000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Tonnes(this float self) =>
        new(self * 1000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Ounces(this int self) =>
        new(self / 35.273961949);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Ounces(this double self) =>
        new(self / 35.273961949);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Ounces(this float self) =>
        new(self / 35.273961949);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Pounds(this int self) =>
        new(self / 2.2046226219);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Pounds(this double self) =>
        new(self / 2.2046226219);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Pounds(this float self) =>
        new(self / 2.2046226219);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Stones(this int self) =>
        new(self / 0.157473044418);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Stones(this double self) =>
        new(self / 0.157473044418);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass Stones(this float self) =>
        new(self / 0.157473044418);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass ImperialTons(this int self) =>
        new(self / 0.0009842065277);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass ImperialTons(this double self) =>
        new(self / 0.0009842065277);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass ImperialTons(this float self) =>
        new(self / 0.0009842065277);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass ShortTon(this int self) =>
        new(self / 0.00110231131093);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass ShortTon(this double self) =>
        new(self / 0.00110231131093);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Mass ShortTon(this float self) =>
        new(self / 0.00110231131093);
}
