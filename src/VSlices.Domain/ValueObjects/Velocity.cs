namespace VSlices.Domain.ValueObjects;

/// <summary>
///
/// </summary>
public readonly struct Velocity : Magnitude<Velocity, double>
{
    readonly double Value;

    internal Velocity(double value) =>
        Value = value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr"></param>
    /// <returns></returns>
    public Velocity New(double repr) => new(repr);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() =>
        $"{Value} m/s";

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Velocity other) =>
        Value.Equals(other.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    public bool Equals(Velocity other, double epsilon) =>
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
        obj is Velocity velocity && Equals(velocity);

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
            Velocity other => CompareTo(other),
            _ => throw new ArgumentException($"must be of type {nameof(Velocity)}")
        };

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Velocity other) =>
        Value.CompareTo(other.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Velocity Add(Velocity rhs) =>
        new(Value + rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Velocity Subtract(Velocity rhs) =>
        new(Value - rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Velocity Multiply(double rhs) =>
        new(Value * rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Velocity Divide(double rhs) =>
        new(Value / rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Velocity operator *(Velocity lhs, double rhs) =>
        lhs.Multiply(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Velocity operator *(double lhs, Velocity rhs) =>
        rhs.Multiply(lhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Length operator *(Velocity lhs, Time rhs) =>
        new(lhs.Value * rhs.Seconds);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Length operator *(Time lhs, Velocity rhs) =>
        new(lhs.Seconds * rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Velocity operator +(Velocity lhs, Velocity rhs) =>
        lhs.Add(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Velocity operator -(Velocity lhs, Velocity rhs) =>
        lhs.Subtract(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Velocity operator /(Velocity lhs, double rhs) =>
        lhs.Divide(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static double operator /(Velocity lhs, Velocity rhs) =>
        lhs.Value / rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Accel operator /(Velocity lhs, Time rhs) =>
        new(lhs.Value / rhs.Seconds);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Time operator /(Velocity lhs, Accel rhs) =>
        new(lhs.Value / rhs.MetresPerSecond2);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(Velocity lhs, Velocity rhs) =>
        lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(Velocity lhs, Velocity rhs) =>
        !lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(Velocity lhs, Velocity rhs) =>
        lhs.Value > rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(Velocity lhs, Velocity rhs) =>
        lhs.Value < rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(Velocity lhs, Velocity rhs) =>
        lhs.Value >= rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(Velocity lhs, Velocity rhs) =>
        lhs.Value <= rhs.Value;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Velocity Round() =>
        new(Math.Round(Value));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Velocity Abs() =>
        new(Math.Abs(Value));

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Velocity Min(Velocity rhs) =>
        new(Math.Min(Value, rhs.Value));

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Velocity Max(Velocity rhs) =>
        new(Math.Max(Value, rhs.Value));

    /// <summary>
    ///
    /// </summary>
    public double MetresPerSecond => Value;

    /// <summary>
    ///
    /// </summary>
    public double KilometresPerSecond => Value / 1000.0;

    /// <summary>
    ///
    /// </summary>
    public double KilometresPerHour => Value / 1000.0 * 3600.0;

    /// <summary>
    ///
    /// </summary>
    public double MilesPerSecond => Value / 1609.344000006437376000025749504;

    /// <summary>
    ///
    /// </summary>
    public double MilesPerHour => Value / 1609.344000006437376000025749504 * 3600.0;

    /// <summary>
    ///
    /// </summary>
    public double Knots => Value / 0.51444444444444;

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Velocity operator -(Velocity value) =>
        new(-value.Value);

    /// <summary>
    ///
    /// </summary>
    public static Velocity AdditiveIdentity { get; } = new(0);
}

/// <summary>
///
/// </summary>
public static class UnitsVelocityExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity MetresPerSecond(this int self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity MetresPerSecond(this float self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity MetresPerSecond(this double self) =>
        new(self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity KilometresPerSecond(this int self) =>
        new(self * 1000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity KilometresPerSecond(this float self) =>
        new(self * 1000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity KilometresPerSecond(this double self) =>
        new(self * 1000.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity KilometresPerHour(this int self) =>
        new(self * 1000.0 / 3600.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity KilometresPerHour(this float self) =>
        new(self * 1000.0 / 3600.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity KilometresPerHour(this double self) =>
        new(self * 1000.0 / 3600.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity MilesPerSecond(this int self) =>
        new(self * 1609.344000006437376000025749504);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity MilesPerSecond(this float self) =>
        new(self * 1609.344000006437376000025749504);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity MilesPerSecond(this double self) =>
        new(self * 1609.344000006437376000025749504);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity MilesPerHour(this int self) =>
        new(self * 1609.344000006437376000025749504 / 3600.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity MilesPerHour(this float self) =>
        new(self * 1609.344000006437376000025749504 / 3600.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity MilesPerHour(this double self) =>
        new(self * 1609.344000006437376000025749504 / 3600.0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity Knots(this int self) =>
        new(self * 0.51444444444444);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity Knots(this float self) =>
        new(self * 0.51444444444444);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Velocity Knots(this double self) =>
        new(self * 0.51444444444444);
}
