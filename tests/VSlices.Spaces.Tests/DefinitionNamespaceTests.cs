using VSlices.Spaces;
using VSlices.Spaces.Finance;
using VSlices.Spaces.Temporal;
using VSlices.Spaces.Quantities;
using VSlices.Spaces.Quantities.Abstract;
using Xunit;

namespace VSlices.Spaces.Tests;

public sealed class DefinitionNamespaceTests
{
    [Fact]
    public void Semantic_definitions_retain_their_respective_namespaces()
    {
        Assert.Equal("VSlices.Spaces.Finance", typeof(Currency).Namespace);
        Assert.Equal("VSlices.Spaces.Finance", typeof(Money<,>).Namespace);
        Assert.Equal("VSlices.Spaces.Temporal", typeof(Moment).Namespace);
        Assert.Equal("VSlices.Spaces.Quantities", typeof(Length<>).Namespace);
        Assert.Equal("VSlices.Spaces.Quantities", typeof(vMass).Namespace);
        Assert.Equal("VSlices.Spaces.Quantities", typeof(Area<,>).Namespace);
        Assert.Equal("VSlices.Spaces", typeof(Conversions).Namespace);
        Assert.Equal("VSlices.Spaces.Quantities", typeof(AlgebraicSymbolAttribute).Namespace);
    }

    [Fact]
    public void Abstract_quantity_definitions_use_the_quantities_abstract_namespace()
    {
        Assert.Equal("VSlices.Spaces.Quantities.Abstract", typeof(Coordinate).Namespace);
        Assert.Equal("VSlices.Spaces.Quantities.Abstract", typeof(Grams).Namespace);
        Assert.Equal("VSlices.Spaces.Quantities.Abstract", typeof(M).Namespace);
    }
}
