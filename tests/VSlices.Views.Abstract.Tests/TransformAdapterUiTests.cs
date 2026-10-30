using LanguageExt;
using VSlices.Spaces.Traits;
using VSlices.Views.Abstract.Forms;
using Xunit;

namespace VSlices.Views.Abstract.Tests;

public sealed class TransformAdapterUiTests
{
    [Fact]
    public void Adapter_materializes_the_canonical_Input_then_uses_target_transformation()
    {
        var result = TransformAdapter<string, EmailAddress>
            .Transform("person@example.com");

        Assert.False(result.IsFail);
        Assert.Equal(
            "person@example.com",
            result.ThrowIfFail().Value);
    }

    [Fact]
    public void Adapter_preserves_existing_direct_target_owned_transforms()
    {
        var result = TransformAdapter<string, LocationName>
            .Transform("  Warehouse  ");

        Assert.False(result.IsFail);
        Assert.Equal(
            "Warehouse",
            result.ThrowIfFail().Value);
    }

    [Fact]
    public void Adapter_preserves_errors_from_the_target_owned_transformation()
    {
        var result = TransformAdapter<string, EmailAddress>
            .Transform("not-an-email");

        Assert.True(result.IsFail);
        Assert.Contains(
            result.FailSpan().ToArray(),
            error => error.Message.Contains(
                "must contain @",
                StringComparison.Ordinal));
    }

    [Fact]
    public void Text_input_materializes_through_TransformAdapter()
    {
        var success = vTextInput<EmailAddress>.TryMaterialize(
            "person@example.com",
            out var email,
            out var validationError);

        Assert.True(success);
        Assert.Equal("person@example.com", email.Value);
        Assert.Null(validationError);

        var failure = vTextInput<EmailAddress>.TryMaterialize(
            "invalid",
            out _,
            out var failureMessage);

        Assert.False(failure);
        Assert.Contains(
            "must contain @",
            failureMessage,
            StringComparison.Ordinal);
    }

    public sealed record LocationName :
        Transformable<string, LocationName>
    {
        private LocationName(string value) =>
            Value = value;

        public string Value { get; }

        public static Fin<LocationName> Transform(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? LanguageExt.Common.Error.New("A location name cannot be empty.")
                : new LocationName(value.Trim());
    }

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
}
