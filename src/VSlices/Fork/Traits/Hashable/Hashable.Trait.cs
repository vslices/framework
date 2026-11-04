using System.Diagnostics.Contracts;
using ForkExt.Attributes;

namespace ForkExt;

/// <summary>
/// Hashable trait
/// </summary>
/// <typeparam name="A">
/// The type for which GetHashCode is defined
/// </typeparam>
[Trait("Hashable*")]
public interface Hashable<in A>
{
    /// <summary>
    /// Get the hash-code of the provided value
    /// </summary>
    /// <returns>Hash code of x</returns>
    [Pure]
    public static abstract int GetHashCode(A x);
}
