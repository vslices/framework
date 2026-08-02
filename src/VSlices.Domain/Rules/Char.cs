namespace VSlices.Domain.Rules;

public sealed class CharOnlyNumbers :
    Rule<CharOnlyNumbers, char>
{
    public static bool Check(char value) =>
        char.IsDigit(value);
}

public sealed class CharInSet<SET> :
    Rule<CharInSet<SET>, char>
    where SET : Const<string>
{
    public static bool Check(char value) =>
        SET.Value.Contains(value);
}
