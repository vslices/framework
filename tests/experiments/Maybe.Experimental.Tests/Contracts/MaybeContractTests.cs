using Xunit;

namespace Maybe.Experimental.Tests.Contracts;

/// <summary>
/// Version-independent behavior for the optional-presence flavor.
/// A version supplies only the adaptation to its concrete representation.
/// Do not add monadic laws here until the version actually claims Monad.
/// </summary>
public abstract class MaybeContractTests<TMaybe>
{
    protected abstract TMaybe Some(int value);
    protected abstract TMaybe None();
    protected abstract bool IsSome(TMaybe value);
    protected abstract bool IsNone(TMaybe value);
    protected abstract int ReadSome(TMaybe value);

    [Fact]
    public void Some_represents_presence()
    {
        var maybe = Some(42);

        Assert.True(IsSome(maybe));
        Assert.False(IsNone(maybe));
    }

    [Fact]
    public void Some_preserves_the_supplied_value()
    {
        Assert.Equal(42, ReadSome(Some(42)));
    }

    [Fact]
    public void None_represents_absence()
    {
        var maybe = None();

        Assert.True(IsNone(maybe));
        Assert.False(IsSome(maybe));
    }

    [Fact]
    public void Default_integer_value_is_still_present_when_wrapped_in_Some()
    {
        var maybe = Some(0);

        Assert.True(IsSome(maybe));
        Assert.Equal(0, ReadSome(maybe));
    }

    [Fact]
    public void Some_and_None_are_distinct_alternatives()
    {
        Assert.True(IsSome(Some(0)));
        Assert.True(IsNone(None()));
        Assert.False(IsNone(Some(0)));
        Assert.False(IsSome(None()));
    }
}
