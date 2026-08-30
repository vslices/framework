using System.Diagnostics.CodeAnalysis;
using LanguageExt;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using VSlices.Domain.Traits;

namespace VSlices.Views.Abstract.Forms;

/// <summary>
/// Base component for text-based view inputs that materialize a valid domain value.
/// </summary>
/// <remarks>
/// The browser-facing representation remains a string. When Blazor updates the field,
/// the component executes the <see cref="Transform{SELF, IN}"/> implemented by
/// <typeparamref name="T"/> and projects any requirement failures into Blazor's normal
/// field-validation pipeline.
///
/// This component intentionally owns no visual language. Concrete view libraries are
/// expected to inherit from it and provide markup and styling.
/// </remarks>
/// <typeparam name="T">The domain value produced from the textual representation.</typeparam>
public abstract class vTextInput<T> : InputBase<T>
    where T : Transform<T, string>
{
    /// <summary>
    /// Gets or sets optional metadata for the transform-backed input.
    /// </summary>
    [Parameter]
    public vTextInputMetadata<T> Metadata { get; set; } = new();

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (string.IsNullOrWhiteSpace(DisplayName) &&
            !string.IsNullOrWhiteSpace(Metadata.DisplayName))
        {
            DisplayName = Metadata.DisplayName;
        }
    }

    /// <inheritdoc />
    protected sealed override bool TryParseValueFromString(
        string? value,
        [MaybeNullWhen(false)] out T result,
        [NotNullWhen(false)] out string? validationErrorMessage)
    {
        var parsed = T.Create(value ?? string.Empty);

        if (parsed.IsFail)
        {
            result = default!;
            validationErrorMessage = string.Join(
                Environment.NewLine,
                parsed.FailSpan().ToArray().Select(error => error.Message));

            return false;
        }

        result = parsed.ThrowIfFail();
        validationErrorMessage = null;

        return true;
    }
}
