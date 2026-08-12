namespace VSlices.Domain.ValueObjects;

/// <summary>
///
/// </summary>
/// <typeparam name="FROM">
///
/// </typeparam>
/// <typeparam name="TO">
///
/// </typeparam>
/// <remarks>
///
/// </remarks>
public readonly struct ExchangeRate<FROM, TO> : DomainType<ExchangeRate<FROM, TO>>
    where FROM : Currency, new()
    where TO : Currency, new()
{
    readonly decimal Value;

    private ExchangeRate(decimal value) =>
        Value = value;

    /// <summary>
    ///
    /// </summary>
    public FROM Source =>
        Currency<FROM>.Value;

    /// <summary>
    ///
    /// </summary>
    public TO Target =>
        Currency<TO>.Value;

    /// <summary>
    ///
    /// </summary>
    public decimal To() =>
        Value;

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    internal static ExchangeRate<FROM, TO> New(decimal value) =>
        new(value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr">
    ///
    /// </param>
    /// <returns>
    ///
    /// </returns>
    public static Fin<ExchangeRate<FROM, TO>> Create(decimal repr) =>
        repr > 0
            ? new ExchangeRate<FROM, TO>(repr)
            : Error.New($"{nameof(ExchangeRate<,>)} must be positive. Sent: {repr}.");

    /// <summary>
    ///
    /// </summary>
    public Money<TO> Apply(Money<FROM> money) =>
        Money<TO>.New(money.To() * Value);

    /// <summary>
    ///
    /// </summary>
    public ExchangeRate<TO, FROM> Invert() =>
        ExchangeRate<TO, FROM>.New(1m / Value);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="NEXT">
    ///
    /// </typeparam>
    public ExchangeRate<FROM, NEXT> Then<NEXT>(ExchangeRate<TO, NEXT> next)
        where NEXT : Currency, new() =>
        ExchangeRate<FROM, NEXT>.New(Value * next.To());

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() =>
        $"1 {Source.Code} = {Value} {Target.Code}";

}
