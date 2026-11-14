using System.Numerics;
using Xunit;

namespace Maybe.Experimental.Tests.Contracts;

/// <summary>
/// Version-independent contract for arithmetic over a nonempty sequence of optional operands.
/// A missing required operand propagates absence; order is relevant for subtraction.
/// The arithmetic mechanism is intentionally external to MaybeV0.
/// </summary>
public abstract class MaybeArithmeticContractTests<TMaybe, T>
    where T : INumber<T>
{
    protected abstract TMaybe Some(T value);
    protected abstract TMaybe None();
    protected abstract bool IsNone(TMaybe value);
    protected abstract T ReadSome(TMaybe value);
    protected abstract TMaybe Combine(Func<T, T, T> operation, params TMaybe[] values);

    private static T N(int value) => T.CreateChecked(value);

    private void ExpectSome(int expected, TMaybe actual)
    {
        Assert.False(IsNone(actual));
        Assert.Equal(N(expected), ReadSome(actual));
    }

    private void ExpectNone(TMaybe actual) => Assert.True(IsNone(actual));

    [Fact]
    public void Addition_of_three_present_operands()
        => ExpectSome(15, Combine((a, b) => a + b, Some(N(10)), Some(N(3)), Some(N(2))));

    [Fact]
    public void Subtraction_is_left_associative_and_preserves_operand_order()
    {
        ExpectSome(5, Combine((a, b) => a - b, Some(N(10)), Some(N(3)), Some(N(2))));
        ExpectSome(-9, Combine((a, b) => a - b, Some(N(3)), Some(N(10)), Some(N(2))));
    }

    [Fact]
    public void Multiplication_of_multiple_present_operands()
        => ExpectSome(120, Combine((a, b) => a * b, Some(N(2)), Some(N(3)), Some(N(4)), Some(N(5))));

    [Fact]
    public void Missing_operand_propagates_for_all_three_operations()
    {
        foreach (var operation in new Func<T, T, T>[]
                 {
                     (a, b) => a + b,
                     (a, b) => a - b,
                     (a, b) => a * b
                 })
        {
            ExpectNone(Combine(operation, None(), Some(N(3))));
            ExpectNone(Combine(operation, Some(N(10)), None()));
            ExpectNone(Combine(operation, None(), None()));
        }
    }

    [Fact]
    public void Missing_operand_in_any_position_propagates()
    {
        foreach (var operation in new Func<T, T, T>[]
                 {
                     (a, b) => a + b,
                     (a, b) => a - b,
                     (a, b) => a * b
                 })
        {
            ExpectNone(Combine(operation, None(), Some(N(3)), Some(N(2))));
            ExpectNone(Combine(operation, Some(N(10)), None(), Some(N(2))));
            ExpectNone(Combine(operation, Some(N(10)), Some(N(3)), None()));
        }
    }

    [Fact]
    public void Some_zero_is_not_absence()
    {
        ExpectSome(10, Combine((a, b) => a + b, Some(N(10)), Some(N(0))));
        ExpectSome(0, Combine((a, b) => a * b, Some(N(10)), Some(N(0))));
        ExpectNone(Combine((a, b) => a + b, None(), Some(N(0))));
    }

    [Fact]
    public void Single_present_operand_keeps_its_value()
        => ExpectSome(7, Combine((a, b) => a - b, Some(N(7))));
}
