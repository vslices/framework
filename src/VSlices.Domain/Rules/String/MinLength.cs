namespace VSlices.Domain.Rules;

public sealed class MinLength<MIN> : Rule<MinLength<MIN>, string>
    where MIN : Const<int>
{
    public int Min => MIN.Value;

    public static bool Check(string value) =>
        value.Length >= MIN.Value;
}
