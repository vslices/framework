using System.Diagnostics.CodeAnalysis;
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
/// Static SSR forms can reuse <see cref="TryMaterialize"/> when the HTTP form mapper
/// transports the raw string separately from the domain-valued edit model. This keeps
/// the transformation semantics identical across interactive and HTTP-post boundaries.
///
/// This component intentionally owns no visual language. Concrete view libraries are
/// expected to inherit from it and provide markup and styling.
/// </remarks>
/// <typeparam name="T">The domain value produced from the textual representation.</typeparam>
public abstract class vTextInput<T> : InputBase<T>
    where T : Transform<T, string>
{
    private bool _subscribedToValidation;

    /// <summary>
    /// Gets or sets optional metadata for the transform-backed input.
    /// </summary>
    [Parameter]
    public vTextInputMetadata<T> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets a raw representation to display when no valid value exists yet.
    /// </summary>
    /// <remarks>
    /// This is primarily useful for static SSR form posts, where HTTP form mapping may
    /// transport strings in a dedicated representation model before they are materialized
    /// into domain values.
    /// </remarks>
    [Parameter]
    public string? Representation { get; set; }

    protected string IdAttributeValue => 
        AdditionalAttributes?.TryGetValue("id", out var explicitId) == true
        ? Convert.ToString(explicitId) ?? string.Empty
        : !string.IsNullOrWhiteSpace(NameAttributeValue)
            ? NameAttributeValue
            : FieldIdentifier.FieldName;

    /// <summary>
    /// Attempts to materialize <typeparamref name="T"/> from a textual representation.
    /// </summary>
    /// <remarks>
    /// This is the canonical text-to-domain adapter used by the component itself and can
    /// also be reused by static SSR submit handlers without duplicating domain invariants.
    /// </remarks>
    public static bool TryMaterialize(
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

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        EditContext.OnValidationRequested += ValidateCurrentRepresentation;
        _subscribedToValidation = true;
    }

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
    protected override string? FormatValueAsString(T? value)
    {
        if (value is null)
        {
            return Representation ?? string.Empty;
        }

        return Metadata.Formatter?.Invoke(value) ?? value.ToString();
    }

    /// <inheritdoc />
    protected sealed override bool TryParseValueFromString(
        string? value,
        [MaybeNullWhen(false)] out T result,
        [NotNullWhen(false)] out string? validationErrorMessage) =>
        TryMaterialize(value, out result, out validationErrorMessage);

    private void ValidateCurrentRepresentation(
        object? sender,
        ValidationRequestedEventArgs args) =>
        CurrentValueAsString = CurrentValueAsString;

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing && _subscribedToValidation)
        {
            EditContext.OnValidationRequested -= ValidateCurrentRepresentation;
            _subscribedToValidation = false;
        }

        base.Dispose(disposing);
    }
}
