using System.Numerics;

namespace VSlices.Domain.Rules;

public sealed class GreaterOrEqualThan<L, A> : Rule<GreaterOrEqualThan<L, A>, A>
    where L : Const<A>
    where A : IComparisonOperators<A, A, bool>
{
    public A Limit => L.Value;
    public static bool Check(A value) =>
        value >= L.Value;
}