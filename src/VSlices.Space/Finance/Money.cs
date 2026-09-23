using System.Numerics;

namespace VSlices.Space.Finance;

/// <summary>
/// A monetary amount expressed in one nominal currency.
///
/// Money is additive within the same currency and supports scalar multiplication
/// and division. It is intentionally not modeled as Q&lt;F,C,T&gt; because Currency is
/// not a measurement Coordinate and no static reference scale exists between currencies.
/// </summary>
public sealed record Money<C, T>(T Amount) : VectorSpace<Money<C, T>, T>
    where C : Currency
    where T : IFloatingPoint<T>
{
    public static Money<C, T> AdditiveIdentity => new(T.Zero);

    public static Money<C, T> operator +(
        Money<C, T> left,
        Money<C, T> right) =>
        new(left.Amount + right.Amount);

    public static Money<C, T> operator -(
        Money<C, T> left,
        Money<C, T> right) =>
        new(left.Amount - right.Amount);

    public static Money<C, T> operator -(
        Money<C, T> value) =>
        new(-value.Amount);

    public static Money<C, T> operator *(
        Money<C, T> value,
        T scalar) =>
        new(value.Amount * scalar);

    public static Money<C, T> operator /(
        Money<C, T> value,
        T scalar) =>
        new(value.Amount / scalar);

    public T RatioTo(Money<C, T> other) =>
        Amount / other.Amount;

    public Money<C, T> Abs() =>
        new(T.Abs(Amount));

    public override string ToString() =>
        $"{C.Symbol}{Amount} {C.Code}";
}
