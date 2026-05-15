namespace VSlices.Literals;

public sealed class True : Const<bool>
{
    public static bool Value { get; } = true;
}

public sealed class False : Const<bool>
{
    public static bool Value { get; } = false;
}