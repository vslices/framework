using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using VSlices.Space.Traits;

namespace VSlices.Views.Abstract.Forms;

public abstract class vTextInput<T> : InputBase<T>
    where T : Transformable<string, T>
{
    private bool _subscribedToValidation;

    [Parameter]
    public vTextInputMetadata<T> Metadata { get; set; } = new();

    [Parameter]
    public string? Representation { get; set; }

    protected string IdAttributeValue =>
        AdditionalAttributes?.TryGetValue("id", out var explicitId) == true
        ? Convert.ToString(explicitId) ?? string.Empty
        : !string.IsNullOrWhiteSpace(NameAttributeValue)
            ? NameAttributeValue
            : FieldIdentifier.FieldName;

    public static bool TryMaterialize(
        string? value,
        [MaybeNullWhen(false)] out T result,
        [NotNullWhen(false)] out string? validationErrorMessage)
    {
        var parsed = Transformable.Transform<string, T>(value ?? string.Empty);

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

    protected override void OnInitialized()
    {
        base.OnInitialized();
        EditContext.OnValidationRequested += ValidateCurrentRepresentation;
        _subscribedToValidation = true;
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (string.IsNullOrWhiteSpace(DisplayName) &&
            !string.IsNullOrWhiteSpace(Metadata.DisplayName))
        {
            DisplayName = Metadata.DisplayName;
        }
    }

    protected override string? FormatValueAsString(T? value)
    {
        if (value is null)
            return Representation ?? string.Empty;

        return Metadata.Formatter?.Invoke(value) ?? value.ToString();
    }

    protected sealed override bool TryParseValueFromString(
        string? value,
        [MaybeNullWhen(false)] out T result,
        [NotNullWhen(false)] out string? validationErrorMessage) =>
        TryMaterialize(value, out result, out validationErrorMessage);

    private void ValidateCurrentRepresentation(
        object? sender,
        ValidationRequestedEventArgs args) =>
        CurrentValueAsString = CurrentValueAsString;

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
