using System.Numerics;

namespace VSlices.Traits;

/// <summary>
/// Represents a trait for defining floating-point suffixes with specific values.
/// </summary>
/// <typeparam name="WSelf">
/// The type that implements this interface, representing the specific floating-point suffix.
/// </typeparam>
/// <typeparam name="WType">
/// The numeric type associated with the floating-point suffix, constrained to implement
/// <see cref="System.Numerics.INumber{T}" />.
/// </typeparam>
public partial interface FloatingSuffixes<WSelf, WType>
    where WSelf : Const<WType>
    where WType : INumber<WType>;
