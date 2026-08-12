using System.Runtime.CompilerServices;

namespace VSlices.Domain.ValueObjects;

/// <summary>
///
/// </summary>
public readonly struct Temperature : Magnitude<Temperature, double>
{
    internal enum UnitType
    {
        K, C, F
    }

    readonly UnitType Type;
    readonly double Value;

    /// <summary>
    ///
    /// </summary>
    public static Temperature AbsoluteZero = default;

    /// <summary>
    ///
    /// </summary>
    public static Temperature ZeroCelsius = new(UnitType.C, 0.0);

    /// <summary>
    ///
    /// </summary>
    public static Temperature ZeroFahrenheit = new(UnitType.F, 0.0);

    internal Temperature(UnitType type, double value)
    {
        Type = type;
        Value = value;

        if (this < AbsoluteZero) throw new ArgumentOutOfRangeException(nameof(value), $"{value} [{type}]", "Less than absolute zero");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Temperature FromCelcius(double value) =>
        new(UnitType.C, value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Temperature FromFahrenheit(double value) =>
        new(UnitType.F, value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Temperature FromKelvin(double value) =>
        new(UnitType.K, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static double CtoK(double x) => x + 273.15;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static double KtoC(double x) => x - 273.15;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static double FtoK(double x) => (x + 459.67) * 5.0 / 9.0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static double KtoF(double x) => (x * 1.8) - 459.67;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static double CtoF(double x) => (x * 1.8) + 32.0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static double FtoC(double x) => (x - 32.0) * 5.0 / 9.0;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() =>
        Value.GetHashCode();

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
        obj is Temperature t && Equals(t);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public bool Equals(Temperature rhs) =>
        Value.Equals(rhs.Value);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public override string ToString() =>
        Type switch
        {
            UnitType.K => $"{Value} K",
            UnitType.C => $"{Value} °C",
            UnitType.F => $"{Value} °F",
            _ => throw new NotSupportedException(Type.ToString())
        };

    /// <summary>
    ///
    /// </summary>
    public Temperature Kelvin =>
        Type switch
        {
            UnitType.K => this,
            UnitType.C => new Temperature(UnitType.K, CtoK(Value)),
            UnitType.F => new Temperature(UnitType.K, FtoK(Value)),
            _ => throw new NotSupportedException(Type.ToString())
        };

    /// <summary>
    ///
    /// </summary>
    public double KValue =>
        Type switch
        {
            UnitType.K => Value,
            UnitType.C => CtoK(Value),
            UnitType.F => FtoK(Value),
            _ => throw new NotSupportedException(Type.ToString())
        };

    /// <summary>
    ///
    /// </summary>
    public Temperature Celsius =>
        Type switch
        {
            UnitType.K => new Temperature(UnitType.C, KtoC(Value)),
            UnitType.C => this,
            UnitType.F => new Temperature(UnitType.C, FtoC(Value)),
            _ => throw new NotSupportedException(Type.ToString())
        };

    /// <summary>
    ///
    /// </summary>
    public Temperature Fahrenheit =>
        Type switch
        {
            UnitType.K => new Temperature(UnitType.F, KtoF(Value)),
            UnitType.C => new Temperature(UnitType.F, CtoF(Value)),
            UnitType.F => this,
            _ => throw new NotSupportedException(Type.ToString())
        };

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public bool Equals(Temperature rhs, double epsilon) =>
        Type switch
        {
            UnitType.K => rhs.Type switch
            {
                UnitType.K => Math.Abs(rhs.Value - Value) < epsilon,
                UnitType.C => Math.Abs(CtoK(rhs.Value) - Value) < epsilon,
                UnitType.F => Math.Abs(FtoK(rhs.Value) - Value) < epsilon,
                _ => throw new NotSupportedException(Type.ToString())
            },
            UnitType.C => rhs.Type switch
            {
                UnitType.K => Math.Abs(KtoC(rhs.Value) - Value) < epsilon,
                UnitType.C => Math.Abs(rhs.Value - Value) < epsilon,
                UnitType.F => Math.Abs(FtoC(rhs.Value) - Value) < epsilon,
                _ => throw new NotSupportedException(Type.ToString())
            },
            UnitType.F => rhs.Type switch
            {
                UnitType.K => Math.Abs(KtoF(rhs.Value) - Value) < epsilon,
                UnitType.C => Math.Abs(CtoF(rhs.Value) - Value) < epsilon,
                UnitType.F => Math.Abs(rhs.Value - Value) < epsilon,
                _ => throw new NotSupportedException(Type.ToString())
            },
            _ => throw new NotSupportedException(Type.ToString())
        };

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
            Temperature other => CompareTo(other),
            _ => throw new ArgumentException($"must be of type {nameof(Temperature)}")
        };

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public int CompareTo(Temperature rhs) =>
        Type switch
        {
            UnitType.K => rhs.Type switch
            {
                UnitType.K => Value.CompareTo(rhs.Value),
                UnitType.C => Value.CompareTo(CtoK(rhs.Value)),
                UnitType.F => Value.CompareTo(FtoK(rhs.Value)),
                _ => throw new NotSupportedException(Type.ToString())
            },
            UnitType.C => rhs.Type switch
            {
                UnitType.K => Value.CompareTo(KtoC(rhs.Value)),
                UnitType.C => Value.CompareTo(rhs.Value),
                UnitType.F => Value.CompareTo(FtoC(rhs.Value)),
                _ => throw new NotSupportedException(Type.ToString())
            },
            UnitType.F => rhs.Type switch
            {
                UnitType.K => Value.CompareTo(KtoF(rhs.Value)),
                UnitType.C => Value.CompareTo(CtoF(rhs.Value)),
                UnitType.F => Value.CompareTo(rhs.Value),
                _ => throw new NotSupportedException(Type.ToString())
            },
            _ => throw new NotSupportedException(Type.ToString())
        };

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public Temperature Add(Temperature rhs) =>
        Type switch
        {
            UnitType.K => rhs.Type switch
            {
                UnitType.K => new Temperature(UnitType.K, Value + rhs.Value),
                UnitType.C => new Temperature(UnitType.K, Value + CtoK(rhs.Value)),
                UnitType.F => new Temperature(UnitType.K, Value + FtoK(rhs.Value)),
                _ => throw new NotSupportedException(Type.ToString())
            },
            UnitType.C => rhs.Type switch
            {
                UnitType.K => new Temperature(UnitType.C, Value + KtoC(rhs.Value)),
                UnitType.C => new Temperature(UnitType.C, Value + rhs.Value),
                UnitType.F => new Temperature(UnitType.C, Value + FtoC(rhs.Value)),
                _ => throw new NotSupportedException(Type.ToString())
            },
            UnitType.F => rhs.Type switch
            {
                UnitType.K => new Temperature(UnitType.F, Value + KtoF(rhs.Value)),
                UnitType.C => new Temperature(UnitType.F, Value + CtoF(rhs.Value)),
                UnitType.F => new Temperature(UnitType.F, Value + rhs.Value),
                _ => throw new NotSupportedException(Type.ToString())
            },
            _ => throw new NotSupportedException(Type.ToString())
        };

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Temperature Add(double rhs) =>
        new(Type, Value + rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public Temperature Subtract(Temperature rhs) =>
        Type switch
        {
            UnitType.K => rhs.Type switch
            {
                UnitType.K => new Temperature(UnitType.K, Value - rhs.Value),
                UnitType.C => new Temperature(UnitType.K, Value - CtoK(rhs.Value)),
                UnitType.F => new Temperature(UnitType.K, Value - FtoK(rhs.Value)),
                _ => throw new NotSupportedException(Type.ToString())
            },
            UnitType.C => rhs.Type switch
            {
                UnitType.K => new Temperature(UnitType.C, Value - KtoC(rhs.Value)),
                UnitType.C => new Temperature(UnitType.C, Value - rhs.Value),
                UnitType.F => new Temperature(UnitType.C, Value - FtoC(rhs.Value)),
                _ => throw new NotSupportedException(Type.ToString())
            },
            UnitType.F => rhs.Type switch
            {
                UnitType.K => new Temperature(UnitType.F, Value - KtoF(rhs.Value)),
                UnitType.C => new Temperature(UnitType.F, Value - CtoF(rhs.Value)),
                UnitType.F => new Temperature(UnitType.F, Value - rhs.Value),
                _ => throw new NotSupportedException(Type.ToString())
            },
            _ => throw new NotSupportedException(Type.ToString())
        };

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Temperature Subtract(double rhs) =>
        new(Type, Value + rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Temperature Multiply(double rhs) =>
        new(Type, Value * rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public Temperature Divide(double rhs) =>
        new(Type, Value / rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Temperature operator *(Temperature lhs, double rhs) =>
        lhs.Multiply(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Temperature operator *(double lhs, Temperature rhs) =>
        rhs.Multiply(lhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Temperature operator +(Temperature lhs, Temperature rhs) =>
        lhs.Add(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Temperature operator +(Temperature lhs, double rhs) =>
        lhs.Add(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Temperature operator -(Temperature lhs, Temperature rhs) =>
        lhs.Subtract(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Temperature operator -(Temperature lhs, double rhs) =>
        lhs.Subtract(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Temperature operator /(Temperature lhs, double rhs) =>
        lhs.Divide(rhs);
    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>

    public static bool operator ==(Temperature lhs, Temperature rhs) =>
        lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(Temperature lhs, Temperature rhs) =>
        !lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(Temperature lhs, Temperature rhs) =>
        lhs.CompareTo(rhs) > 0;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(Temperature lhs, Temperature rhs) =>
        lhs.CompareTo(rhs) < 0;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(Temperature lhs, Temperature rhs) =>
        lhs.CompareTo(rhs) >= 0;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(Temperature lhs, Temperature rhs) =>
        lhs.CompareTo(rhs) <= 0;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Temperature Round() =>
        new(Type, Math.Round(Value));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Temperature Abs() =>
        new(Type, Math.Abs(Value));

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public Temperature Min(Temperature rhs) =>
        Type switch
        {
            UnitType.K => rhs.Type switch
            {
                UnitType.K => new Temperature(UnitType.K, Math.Min(Value, rhs.Value)),
                UnitType.C => new Temperature(UnitType.K, Math.Min(Value, CtoK(rhs.Value))),
                UnitType.F => new Temperature(UnitType.K, Math.Min(Value, FtoK(rhs.Value))),
                _ => throw new NotSupportedException(Type.ToString())
            },
            UnitType.C => rhs.Type switch
            {
                UnitType.K => new Temperature(UnitType.C, Math.Min(Value, KtoC(rhs.Value))),
                UnitType.C => new Temperature(UnitType.C, Math.Min(Value, rhs.Value)),
                UnitType.F => new Temperature(UnitType.C, Math.Min(Value, FtoC(rhs.Value))),
                _ => throw new NotSupportedException(Type.ToString())
            },
            UnitType.F => rhs.Type switch
            {
                UnitType.K => new Temperature(UnitType.F, Math.Min(Value, KtoF(rhs.Value))),
                UnitType.C => new Temperature(UnitType.F, Math.Min(Value, CtoF(rhs.Value))),
                UnitType.F => new Temperature(UnitType.F, Math.Min(Value, rhs.Value)),
                _ => throw new NotSupportedException(Type.ToString())
            },
            _ => throw new NotSupportedException(Type.ToString())
        };

    /// <summary>
    ///
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public Temperature Max(Temperature rhs) =>
        Type switch
        {
            UnitType.K => rhs.Type switch
            {
                UnitType.K => new Temperature(UnitType.K, Math.Max(Value, rhs.Value)),
                UnitType.C => new Temperature(UnitType.K, Math.Max(Value, CtoK(rhs.Value))),
                UnitType.F => new Temperature(UnitType.K, Math.Max(Value, FtoK(rhs.Value))),
                _ => throw new NotSupportedException(Type.ToString())
            },
            UnitType.C => rhs.Type switch
            {
                UnitType.K => new Temperature(UnitType.C, Math.Max(Value, KtoC(rhs.Value))),
                UnitType.C => new Temperature(UnitType.C, Math.Max(Value, rhs.Value)),
                UnitType.F => new Temperature(UnitType.C, Math.Max(Value, FtoC(rhs.Value))),
                _ => throw new NotSupportedException(Type.ToString())
            },
            UnitType.F => rhs.Type switch
            {
                UnitType.K => new Temperature(UnitType.F, Math.Max(Value, KtoF(rhs.Value))),
                UnitType.C => new Temperature(UnitType.F, Math.Max(Value, CtoF(rhs.Value))),
                UnitType.F => new Temperature(UnitType.F, Math.Max(Value, rhs.Value)),
                _ => throw new NotSupportedException(Type.ToString())
            },
            _ => throw new NotSupportedException(Type.ToString())
        };

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Temperature operator -(Temperature value) =>
        new(value.Type, -value.Value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr"></param>
    /// <returns></returns>
    public static Temperature From(double repr) =>
        new(UnitType.C, repr);

    /// <summary>
    ///
    /// </summary>
    public static Temperature AdditiveIdentity { get; } = new(UnitType.C, 0);

}

/// <summary>
///
/// </summary>
public static class UnitsTemperatureExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Temperature Celsius(this int self) =>
        new(Temperature.UnitType.C, self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Temperature Celsius(this float self) =>
        new(Temperature.UnitType.C, self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Temperature Celsius(this double self) =>
        new(Temperature.UnitType.C, self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Temperature Fahrenheit(this int self) =>
        new(Temperature.UnitType.F, self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Temperature Fahrenheit(this float self) =>
        new(Temperature.UnitType.F, self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Temperature Fahrenheit(this double self) =>
        new(Temperature.UnitType.F, self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Temperature Kelvin(this int self) =>
        new(Temperature.UnitType.K, self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Temperature Kelvin(this float self) =>
        new(Temperature.UnitType.K, self);

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Temperature Kelvin(this double self) =>
        new(Temperature.UnitType.K, self);
}
