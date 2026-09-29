namespace VSlices.Views.Abstract.Forms;

public interface vTransformMetadata<T, TInput>
{
    string? DisplayName { get; }
}

public sealed record vTextInputMetadata<T>(
    string? DisplayName = null,
    string InputType = "text",
    string? AutoComplete = null,
    string? Placeholder = null,
    Func<T, string>? Formatter = null)
    : vTransformMetadata<T, string>;
