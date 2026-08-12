namespace VSlices.Literals;

/// <summary>
///
/// </summary>
public sealed class Space : Const<string>
{
    /// <summary>
    ///
    /// </summary>
    public static string Value => " ";
}

/// <summary>
///
/// </summary>
public sealed class LowerCaseAlpha : Const<string>
{
    /// <summary>
    ///
    /// </summary>
    public static string Value => "aábcdeéfghiíjklmnñoópqrstuúüvwxyz";
}

/// <summary>
///
/// </summary>
public sealed class UpperCaseAlpha : Const<string>
{
    /// <summary>
    ///
    /// </summary>
    public static string Value { get; } =
        LowerCaseAlpha.Value.ToUpper();
}

/// <summary>
///
/// </summary>
public sealed class AlphaCharset : Const<string>
{
    /// <summary>
    ///
    /// </summary>
    public static string Value { get; } =
        LowerCaseAlpha.Value + UpperCaseAlpha.Value;
}

/// <summary>
///
/// </summary>
public sealed class NumericCharset : Const<string>
{
    /// <summary>
    ///
    /// </summary>
    public static string Value => "0123456789";
}

/// <summary>
///
/// </summary>
public sealed class AlphaNumericCharset : Const<string>
{
    /// <summary>
    ///
    /// </summary>
    public static string Value { get; } =
        AlphaCharset.Value + NumericCharset.Value;
}

/// <summary>
///
/// </summary>
public sealed class NameCharset : Const<string>
{
    /// <summary>
    ///
    /// </summary>
    public static string Value { get; } =
        AlphaCharset.Value + Space.Value + "'-";
}

/// <summary>
///
/// </summary>
public sealed class EmailCharset : Const<string>
{
    /// <summary>
    ///
    /// </summary>
    public static string Value { get; } =
        AlphaNumericCharset.Value + "@._-";
}

/// <summary>
///
/// </summary>
public sealed class PhoneCharset : Const<string>
{
    /// <summary>
    ///
    /// </summary>
    public static string Value { get; } =
        NumericCharset.Value + "+() -";
}

/// <summary>
///
/// </summary>
public sealed class AddressCharset : Const<string>
{
    /// <summary>
    ///
    /// </summary>
    public static string Value { get; } =
        AlphaNumericCharset.Value + Space.Value + "'-.,#";
}

/// <summary>
///
/// </summary>
public sealed class CodeCharset : Const<string>
{
    /// <summary>
    ///
    /// </summary>
    public static string Value { get; } =
        AlphaNumericCharset.Value + "-_";
}

/// <summary>
///
/// </summary>
public sealed class SlugCharset : Const<string>
{
    /// <summary>
    ///
    /// </summary>
    public static string Value { get; } =
        AlphaNumericCharset.Value + "-";
}

/// <summary>
///
/// </summary>
public sealed class FreeTextCharset : Const<string>
{
    /// <summary>
    ///
    /// </summary>
    public static string Value { get; } = new string([.. Enumerable.Range(32, 95).Select(i => (char)i)]);
}

