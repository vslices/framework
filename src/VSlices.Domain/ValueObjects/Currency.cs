namespace VSlices.Domain.ValueObjects;

/// <summary>
///
/// </summary>
/// <remarks>
///
/// </remarks>
public interface Currency : DomainType<Currency, string>
{
    /// <summary>
    ///
    /// </summary>
    string Code { get; }

    /// <summary>
    ///
    /// </summary>
    string Name { get; }

    /// <summary>
    ///
    /// </summary>
    string Symbol { get; }

    /// <summary>
    ///
    /// </summary>
    int Decimals { get; }

    /// <summary>
    ///
    /// </summary>
    string Represented<string>.To() =>
        Code;
}

/// <summary>
///
/// </summary>
/// <typeparam name="C">
///
/// </typeparam>
public static class Currency<C>
    where C : Currency, new()
{
    /// <summary>
    ///
    /// </summary>
    public static C Value { get; } =
        new();
}
