using Maybe.Experimental.MaybeV0;
using Xunit;

namespace Maybe.Experimental.Tests.Versions;

/// <summary>
/// C# generics are exercised beyond numeric types, without relying on a language-ext trait.
/// </summary>
public sealed class MaybeV0TypeCoverageTests
{
    [Fact]
    public void String_payload_is_preserved()
    {
        MaybeV0<string> maybe = new SomeV0<string>("hello");

        var some = Assert.IsType<SomeV0<string>>(maybe);
        Assert.Equal("hello", some.Value);
    }

    [Fact]
    public void None_of_string_is_an_absence()
    {
        MaybeV0<string> maybe = new NoneV0<string>();

        Assert.IsType<NoneV0<string>>(maybe);
    }

    [Fact]
    public void Record_payload_is_preserved()
    {
        var item = new SampleItem("abc", 4);
        MaybeV0<SampleItem> maybe = new SomeV0<SampleItem>(item);

        var some = Assert.IsType<SomeV0<SampleItem>>(maybe);
        Assert.Equal(item, some.Value);
    }

    private sealed record SampleItem(string Id, int Count);
}
