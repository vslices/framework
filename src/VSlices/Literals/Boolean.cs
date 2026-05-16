namespace VSlices.Literals;

/// <summary>
/// Represents a strongly-typed constant value of true or false, backed by a <see cref="bool"/> value.
/// </summary>
public sealed class True : Const<bool>
{
    /// <summary>
    /// Represents the boolean value true.
    /// </summary>
    public static bool Value { get; } = true;
}

/// <summary>
/// Represents a strongly-typed constant value of false, backed by a <see cref="bool"/> value.
/// </summary>
public sealed class False : Const<bool>
{
    /// <summary>
    /// Represents the boolean value false.
    /// </summary>
    public static bool Value { get; } = false;
}