using System.Numerics;

namespace VSlices.Domain.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF"></typeparam>
/// <typeparam name="DISTANCE"></typeparam>
/// <typeparam name="DISTANCE_SCALAR"></typeparam>
public interface AffineSpace<SELF, DISTANCE, DISTANCE_SCALAR> :
    DiscreteSpace<SELF>,
    IAdditionOperators<SELF, DISTANCE, SELF>,
    ISubtractionOperators<SELF, SELF, DISTANCE>
    where SELF : AffineSpace<SELF, DISTANCE, DISTANCE_SCALAR>
    where DISTANCE : VectorSpace<DISTANCE, DISTANCE_SCALAR>
    where DISTANCE_SCALAR : notnull;
