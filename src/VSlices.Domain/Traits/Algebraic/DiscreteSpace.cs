using System;
using System.Numerics;

namespace VSlices.Domain.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
///
/// </typeparam>
public interface DiscreteSpace<SELF> :
    IEquatable<SELF>,
    IEqualityOperators<SELF, SELF, bool>
    where SELF : DiscreteSpace<SELF>;
