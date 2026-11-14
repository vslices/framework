namespace Maybe.Experimental.MaybeV0;

/// <summary>
/// External experimental operation, not a method or a trait of MaybeV0.
/// Models the "all operands required" interpretation of optional arithmetic.
/// Empty input has no agreed semantics and is intentionally excluded.
/// </summary>
public static class MaybeV0Arithmetic
{
    public static MaybeV0<A> Combine<A>(
        Func<A, A, A> operation,
        params MaybeV0<A>[] operands)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(operands);

        if (operands.Length == 0)
            throw new ArgumentException("At least one operand is required.", nameof(operands));

        if (operands[0] is not SomeV0<A> first)
            return new NoneV0<A>();

        var accumulator = first.Value;

        for (var i = 1; i < operands.Length; i++)
        {
            if (operands[i] is not SomeV0<A> present)
                return new NoneV0<A>();

            accumulator = operation(accumulator, present.Value);
        }

        return new SomeV0<A>(accumulator);
    }
}
