using System.Numerics;

namespace VSlices.Domain.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF"></typeparam>
/// <typeparam name="SCALAR"></typeparam>
public interface VectorSpace<SELF, SCALAR> :
    DiscreteSpace<SELF>,
    IUnaryNegationOperators<SELF, SELF>,
    IAdditiveIdentity<SELF, SELF>,
    IAdditionOperators<SELF, SELF, SELF>,
    ISubtractionOperators<SELF, SELF, SELF>,
    IMultiplyOperators<SELF, SCALAR, SELF>,
    IDivisionOperators<SELF, SCALAR, SELF>
    where SELF : VectorSpace<SELF, SCALAR>
    where SCALAR : notnull
{
    /// <summary>
    /// Returns the origin of the vector space.
    /// </summary>
    /// <returns>The origin point of the vector space.</returns>
    public static virtual SELF Origin => SELF.AdditiveIdentity;
}
