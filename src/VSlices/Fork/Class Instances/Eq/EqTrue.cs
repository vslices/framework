using System.Diagnostics.Contracts;
using ForkExt.Traits;

namespace ForkExt.ClassInstances;

/// <summary>
/// Always returns true for equality checks
/// </summary>
public struct EqTrue<A> : Eq<A>
{
    [Pure]
    public static bool Equals(A x, A y) =>
        true;

    [Pure]
    public static int GetHashCode(A x) =>
        EqDefault<A>.GetHashCode(x);
}
