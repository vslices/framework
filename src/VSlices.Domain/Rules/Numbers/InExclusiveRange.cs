using System.Numerics;

namespace VSlices.Domain.Rules;

public sealed class InExclusiveRange<LL, UL, A> : Rule<InExclusiveRange<LL, UL, A>, A>
    where LL : Const<A>
    where UL : Const<A>
    where A : IComparisonOperators<A, A, bool>
{
    public A LowerLimit => LL.Value;
    
    public A UpperLimit => UL.Value;
    public static bool Check(A value) =>
        value > LL.Value && value < UL.Value;
}