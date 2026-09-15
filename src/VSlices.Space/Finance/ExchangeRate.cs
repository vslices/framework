using System.Numerics;
using LanguageExt;

namespace VSlices.Space.Finance;

/// <summary>
/// An established positive conversion relation from one currency space to another.
///
/// The rate value is runtime semantic evidence. It is deliberately not encoded as a
/// Currency coordinate scale because exchange rates are contextual and may change.
/// </summary>
public sealed record ExchangeRate<FROM, TO, T> : DiscreteSpace<ExchangeRate<FROM, TO, T>>
    where FROM : Currency
    where TO : Currency
    where T : IFloatingPoint<T>
{
    private ExchangeRate(T value) => Value = value;

    public T Value { get; }

    public static Fin<ExchangeRate<FROM, TO, T>> Create(T value) =>
        value > T.Zero
            ? new ExchangeRate<FROM, TO, T>(value)
            : Error.New($"Exchange rate must be positive. Sent: {value}.");

    public Money<TO, T> Apply(Money<FROM, T> money) =>
        new(money.Amount * Value);

    public ExchangeRate<TO, FROM, T> Invert() =>
        ExchangeRate<TO, FROM, T>.Established(T.One / Value);

    public ExchangeRate<FROM, NEXT, T> Then<NEXT>(ExchangeRate<TO, NEXT, T> next)
        where NEXT : Currency =>
        ExchangeRate<FROM, NEXT, T>.Established(Value * next.Value);

    internal static ExchangeRate<FROM, TO, T> Established(T value) =>
        new(value);

    public override string ToString() =>
        $"1 {FROM.Code} = {Value} {TO.Code}";
}
