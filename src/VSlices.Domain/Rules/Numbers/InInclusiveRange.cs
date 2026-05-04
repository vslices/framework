using System.Numerics;

namespace VSlices.Domain.Rules;

public sealed class InInclusiveRange<LL, UL, A> : Rule<InInclusiveRange<LL, UL, A>, A>
    where LL : Const<A>
    where UL : Const<A>
    where A : IComparisonOperators<A, A, bool>
{
    public A LowerLimit => LL.Value;
    
    public A UpperLimit => UL.Value;

    public static bool Check(A value) =>
        value >= LL.Value && value <= UL.Value;

}