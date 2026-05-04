namespace VSlices.Domain.Rules;

public sealed record InvalidCharsetTextError(Seq<char> Invalid, string AllowedChars, string Value)
    : Expected(nameof(InvalidCharsetTextError), 0);

public 
public sealed class Text<Min, Max, Chars>
    where Min : Const<int>
    where Max : Const<int>
    where Chars : Const<string>
{
    public string Value { get; }

    public static int MinLength => Min.Value;

    public static int MaxLength => Max.Value;

    public static string Charset => Chars.Value;

    private Text(string value) => Value = value.Trim();

    public static Text<Min, Max, Chars> New(string value) => 
        new(value);

    public static Fin<Text<Min, Max, Chars>> From(string repr)
    {
        var lengthVal = ValidateLength(repr);
        var charsetVal = ValidateCharset(repr);

        return (lengthVal, charsetVal).Apply((l, c) => New(repr)).As();
    }

    private static Fin<string> ValidateLength(string repr) =>
        repr.Length < MinLength || repr.Length > MaxLength
            ? new TextOutOfBoundsError(MinLength, MaxLength, repr.Length, repr)
            : repr;

    private static Fin<string> ValidateCharset(string repr)
    {
        var invalidChars = repr.Where(c => !Chars.Value.Contains(c))
                               .Distinct()
                               .AsIterable()
                               .ToSeq();

        Fin<string> validCharset = invalidChars.IsEmpty
            ? repr
            : new InvalidCharsetTextError(invalidChars, Chars.Value, repr);

        return validCharset;
    }
}
