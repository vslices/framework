using VSlices.Spaces.Definitions;
using VSlices.Spaces.Definitions.Abstract;
using Xunit;

namespace VSlices.Spaces.Tests;

public sealed class DefinitionNamespaceTests
{
    [Fact]
    public void Framework_definitions_use_the_canonical_namespace()
    {
        const string expected = "VSlices.Spaces.Definitions";

        Assert.Equal(expected, typeof(Currency).Namespace);
        Assert.Equal(expected, typeof(Money<,>).Namespace);
        Assert.Equal(expected, typeof(Moment).Namespace);
        Assert.Equal(expected, typeof(Length<>).Namespace);
        Assert.Equal(expected, typeof(vMass).Namespace);
        Assert.Equal(expected, typeof(Area<,>).Namespace);
        Assert.Equal(expected, typeof(Conversions).Namespace);
        Assert.Equal(expected, typeof(AlgebraicSymbolAttribute).Namespace);
    }

    [Fact]
    public void Abstract_quantity_definitions_use_the_abstract_namespace()
    {
        const string expected = "VSlices.Spaces.Definitions.Abstract";

        Assert.Equal(expected, typeof(Coordinate).Namespace);
        Assert.Equal(expected, typeof(Grams).Namespace);
        Assert.Equal(expected, typeof(M).Namespace);
    }
}
