using System.Numerics;

namespace VSlices.Domain.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
///
/// </typeparam>
/// <typeparam name="SCALAR">
///
/// </typeparam>
/// <remarks>
///
/// </remarks>
public interface Magnitude<SELF, SCALAR> :
    DomainType<SELF>,
    VectorSpace<SELF, SCALAR>,
    IComparable<SELF>,
    IComparisonOperators<SELF, SELF, bool>
    where SELF : Magnitude<SELF, SCALAR>
    where SCALAR : notnull;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
///
/// </typeparam>
/// <typeparam name="SCALAR">
///
/// </typeparam>
/// <typeparam name="REPR">
///
/// </typeparam>
/// <remarks>
///
/// </remarks>
public interface Magnitude<SELF, SCALAR, REPR> :
    DomainType<SELF, REPR>,
    Magnitude<SELF, SCALAR>
    where SELF : Magnitude<SELF, SCALAR, REPR>
    where SCALAR : notnull;

