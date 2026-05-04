namespace VSlices.Domain.Rules;

public sealed class MaxLength<MAX> 
    : Rule<MaxLength<MAX>, string> 
    where MAX : Const<int>
{
    public int Max => MAX.Value;

    public static bool Check(string value) =>
        value.Length <= MAX.Value;
}