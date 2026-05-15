namespace VSlices.Literals;

public sealed class Space : Const<string>
{
    public static string Value => " ";
}

public sealed class LowerCaseAlpha : Const<string>
{
    public static string Value => "aábcdeéfghiíjklmnñoópqrstuúüvwxyz";
}

public sealed class UpperCaseAlpha : Const<string>
{
    public static string Value { get; } = 
        LowerCaseAlpha.Value.ToUpper();
}

public sealed class AlphaCharset : Const<string>
{
    public static string Value { get; } = 
        LowerCaseAlpha.Value + UpperCaseAlpha.Value;
}

public sealed class NumericCharset : Const<string>
{
    public static string Value => "0123456789";
}

public sealed class AlphaNumericCharset : Const<string>
{
    public static string Value { get; } = 
        AlphaCharset.Value + NumericCharset.Value;
}

public sealed class NameCharset : Const<string>
{
    public static string Value { get; } = 
        AlphaCharset.Value + Space.Value + "'-";
}

public sealed class EmailCharset : Const<string>
{
    public static string Value { get; } = 
        AlphaNumericCharset.Value + "@._-";
}

public sealed class PhoneCharset : Const<string>
{
    public static string Value { get; } = 
        NumericCharset.Value + "+() -";
}

public sealed class AddressCharset : Const<string>
{
    public static string Value { get; } = 
        AlphaNumericCharset.Value + Space.Value + "'-.,#";
}

public sealed class CodeCharset : Const<string>
{
    public static string Value { get; } = 
        AlphaNumericCharset.Value + "-_";
}

public sealed class SlugCharset : Const<string>
{
    public static string Value { get; } = 
        AlphaNumericCharset.Value + "-";
}

public sealed class FreeTextCharset : Const<string>
{
    public static string Value => new string(Enumerable.Range(32, 95).Select(i => (char)i).ToArray());
}

