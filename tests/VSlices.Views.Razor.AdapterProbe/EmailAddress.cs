using LanguageExt;
using VSlices.Spaces.Traits;

namespace VSlices.Views.Razor.AdapterProbe;

public sealed record EmailAddress :
    Transformable<EmailAddress.Input, EmailAddress>
{
    public readonly record struct Input(string Value);

    private EmailAddress(string value) =>
        Value = value;

    public string Value { get; }

    public static Fin<EmailAddress> Transform(Input input) =>
            input.Value.Contains('@')
                ? new EmailAddress(input.Value)
                : LanguageExt.Common.Error.New("An email address must contain @.");
}
