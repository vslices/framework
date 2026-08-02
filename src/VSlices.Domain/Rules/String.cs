namespace VSlices.Domain.Rules;

public sealed class StringOnlyNumbers :
    Rule<StringOnlyNumbers, string>
{
    public static bool Check(string value) =>
        value.All(char.IsDigit);
}

public sealed class StringLengthEquals<LEN> :
    Rule<StringLengthEquals<LEN>, string>
    where LEN : Const<int>
{
    public int Length => LEN.Value;

    public static bool Check(string value) =>
        value.Length == LEN.Value;
}

public sealed class StringLength<MIN, MAX> :
    Rule<StringLength<MIN, MAX>, string>
    where MIN : Const<int>
    where MAX : Const<int>
{
    public int Min => MIN.Value;

    public int Max => MAX.Value;

    public static bool Check(string value) =>
        value.Length >= MIN.Value && value.Length <= MAX.Value;
}
