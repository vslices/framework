using System.Numerics;

namespace VSlices.Domain.Rules;

public sealed class NumberOnlyPositive<N> : Rule<NumberOnlyPositive<N>, N>
    where N : INumber<N>
{
    public static bool Check(N value) =>
        N.Zero > value;
}
