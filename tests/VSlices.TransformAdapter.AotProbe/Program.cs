using VSlices.Arrows;
using VSlices.Space.Traits;

var direct = TransformAdapter<string, LocationName>
    .Transform("  Warehouse  ");

if (direct.IsFail ||
    direct.ThrowIfFail().Value != "Warehouse")
{
    return 1;
}

var adapted = TransformAdapter<string, EmailAddress>
    .Transform("person@example.com");

if (adapted.IsFail ||
    adapted.ThrowIfFail().Value != "person@example.com")
{
    return 2;
}

return 0;

public sealed record LocationName :
    Transformable<string, LocationName>
{
    private LocationName(string value) =>
        Value = value;

    public string Value { get; }

    public static Req<string, LocationName>.Full Transformation =>
        Req<string, LocationName>.Transform<string, LocationName>(
            static value => new LocationName(value.Trim()));
}

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
