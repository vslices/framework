using VSlices.Arrows;
using VSlices.Space.Traits;

namespace VSlices.Views.Razor.AdapterProbe;

public sealed record EmailAddress :
    Transformable<EmailAddress.Input, EmailAddress>
{
    public readonly record struct Input(string Value);

    private EmailAddress(string value) =>
        Value = value;

    public string Value { get; }

    public static Req<Input, EmailAddress>.Full Transformation =>
        Req<Input, EmailAddress>.Ensure<Input>(
            static input => input.Value.Contains('@'),
            "An email address must contain @.") >>
        Req<Input, EmailAddress>.Transform<Input, EmailAddress>(
            static input => new EmailAddress(input.Value));
}
