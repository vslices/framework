using System.Numerics;

namespace VSlices.Domain.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF"></typeparam>
/// <typeparam name="SCALAR"></typeparam>
public interface QuantitySpace<SELF, SCALAR> :
    VectorSpace<SELF, SCALAR>,
    IComparable<SELF>,
    IComparisonOperators<SELF, SELF, bool>
    where SELF : QuantitySpace<SELF, SCALAR>
    where SCALAR : notnull
{
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    SCALAR CanonValue { get; }
}
