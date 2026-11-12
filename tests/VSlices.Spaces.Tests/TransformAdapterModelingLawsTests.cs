using LanguageExt;
using VSlices.Spaces.Laws;
using VSlices.Spaces.Traits;
using Xunit;

namespace VSlices.Spaces.Tests;

public sealed class TransformAdapterModelingLawsTests
{
    [Fact]
    public void Direct_transform_does_not_require_nested_input() =>
        Assert.Empty(TransformAdapterModelingLaws.Inspect(typeof(string), typeof(DirectName)));

    [Fact]
    public void Nested_input_contract_passes() =>
        Assert.Empty(TransformAdapterModelingLaws.Inspect(typeof(string), typeof(ValidName)));

    [Theory]
    [InlineData(typeof(NoInputName), "TA001", "ta001-nested-input")]
    [InlineData(typeof(WrongConstructorName), "TA002", "ta002-input-constructor")]
    [InlineData(typeof(MissingTransformName), "TA003", "ta003-canonical-transform")]
    public void Invalid_contract_reports_stable_diagnostic_and_guidance(
        Type target, string rule, string anchor)
    {
        var violation = Assert.Single(TransformAdapterModelingLaws.InspectTextInput(target));
        Assert.Equal(rule, violation.RuleId);
        Assert.Equal(typeof(string).FullName, violation.Source);
        Assert.Equal(target.FullName, violation.Target);
        Assert.Equal(TransformAdapterModelingLaws.Guide + "#" + anchor, violation.GuidanceUrl);
        Assert.Contains("[" + rule + "]", violation.ToString());
    }

    [Fact]
    public void Open_generic_types_require_a_closed_contract()
    {
        var violation = Assert.Single(TransformAdapterModelingLaws.Inspect(
            typeof(string), typeof(GenericName<>)));
        Assert.Equal("TA000", violation.RuleId);
    }

    public sealed class DirectName : Transformable<string, DirectName>
    {
        public static Fin<DirectName> Transform(string input) => default;
    }

    public sealed class ValidName : Transformable<ValidName.Input, ValidName>
    {
        public readonly record struct Input(string Value);
        public static Fin<ValidName> Transform(Input input) => default;
    }

    public sealed class NoInputName { }

    public sealed class WrongConstructorName :
        Transformable<WrongConstructorName.Input, WrongConstructorName>
    {
        public readonly record struct Input(int Value);
        public static Fin<WrongConstructorName> Transform(Input input) => default;
    }

    public sealed class MissingTransformName
    {
        public readonly record struct Input(string Value);
    }

    public sealed class GenericName<T> { }
}
