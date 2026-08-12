using System.Globalization;

namespace VSlices.Domain.ValueObjects;

/// <summary>
///
/// </summary>
/// <typeparam name="C"></typeparam>
/// <remarks>
///
/// </remarks>
public readonly struct Money<C> :
    Magnitude<Money<C>, decimal, decimal>,
    Transform<Money<C>, decimal>
    where C : Currency, new()
{
    private readonly decimal _value;

    private Money(decimal value) =>
        _value = value;

    /// <summary>
    ///
    /// </summary>
    public C Currency =>
        Currency<C>.Value;

    /// <summary>
    ///
    /// </summary>
    public decimal Amount =>
        _value;

    /// <summary>
    ///
    /// </summary>
    public decimal To() =>
        _value;

    /// <summary>
    ///
    /// </summary>
    static Req<decimal, Money<C>> Transform<Money<C>, Money<C>, decimal>.Invariants { get; } =
        Req.Transform(Money<C> (decimal v) => new Money<C>(v));

    /// <summary>
    ///
    /// </summary>
    internal static Money<C> New(decimal value) =>
        new(value);

    /// <summary>
    ///
    /// </summary>
    public static Money<C> AdditiveIdentity { get; } =
        new(0m);

    /// <summary>
    ///
    /// </summary>
    public static Money<C> Zero =>
        AdditiveIdentity;

    /// <summary>
    ///
    /// </summary>
    public static Money<C> One { get; } =
        new(1m);

    /// <summary>
    ///
    /// </summary>
    public Money<C> Add(Money<C> rhs) =>
        new(_value + rhs._value);

    /// <summary>
    ///
    /// </summary>
    public Money<C> Subtract(Money<C> rhs) =>
        new(_value - rhs._value);

    /// <summary>
    ///
    /// </summary>
    public Money<C> Multiply(decimal rhs) =>
        new(_value * rhs);

    /// <summary>
    ///
    /// </summary>
    public Money<C> Divide(decimal rhs) =>
        new(_value / rhs);

    /// <summary>
    ///
    /// </summary>
    public Fin<Money<C>> DivideSafe(decimal rhs) =>
        rhs > 0
            ? new Money<C>(_value / rhs)
            : Error.New($"{nameof(Money<>)} cannot be divided by zero.");

    /// <summary>
    ///
    /// </summary>
    public decimal RatioTo(Money<C> rhs) =>
        _value / rhs._value;

    /// <summary>
    ///
    /// </summary>
    public Fin<decimal> RatioToSafe(Money<C> rhs) =>
        rhs._value > 0
            ? _value / rhs._value
            : Error.New($"{nameof(Money<>)} ratio cannot divide by zero money.");

    /// <summary>
    ///
    /// </summary>
    public Money<C> Abs() =>
        new(Math.Abs(_value));

    /// <summary>
    ///
    /// </summary>
    public Money<C> Round(MidpointRounding mode = MidpointRounding.ToEven)
    {
        var currency = Currency;

        return new Money<C>(
            decimal.Round(_value, currency.Decimals, mode));
    }

    /// <summary>
    ///
    /// </summary>
    public Money<TO> Convert<TO>(ExchangeRate<C, TO> rate)
        where TO : Currency, new() =>
        Money<TO>.New(_value * rate.To());

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Money<C> operator +(Money<C> lhs, Money<C> rhs) => lhs.Add(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Money<C> operator -(Money<C> lhs, Money<C> rhs) => lhs.Subtract(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Money<C> operator -(Money<C> value) => new(-value._value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Money<C> operator *(Money<C> lhs, decimal rhs) => lhs.Multiply(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Money<C> operator *(decimal lhs, Money<C> rhs) => rhs.Multiply(lhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Money<C> operator /(Money<C> lhs, decimal rhs) => lhs.Divide(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static decimal operator /(Money<C> lhs, Money<C> rhs) => lhs.RatioTo(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(Money<C> lhs, Money<C> rhs) => lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(Money<C> lhs, Money<C> rhs) => !lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(Money<C> lhs, Money<C> rhs) => lhs._value > rhs._value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(Money<C> lhs, Money<C> rhs) => lhs._value < rhs._value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(Money<C> lhs, Money<C> rhs) => lhs._value >= rhs._value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(Money<C> lhs, Money<C> rhs) => lhs._value <= rhs._value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Money<C> other) => _value == other._value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Money<C> other) =>
        _value.CompareTo(other._value);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var code = Currency.Code;
        var symbol = Currency.Symbol;
        var decimals = Currency.Decimals;

        return $"{symbol}{_value.ToString($"N{decimals}", CultureInfo.InvariantCulture)} {code}";
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => obj is Money<C> money && Equals(money);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() =>
        HashCode.Combine(Currency, _value);
}

