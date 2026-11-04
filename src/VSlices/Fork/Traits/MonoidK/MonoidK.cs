using System.Diagnostics.Contracts;

namespace ForkExt.Traits;

/// <summary>
/// A monoid for higher-kinds
/// </summary>
/// <typeparam name="M">Higher kind</typeparam>
public interface MonoidK<M> : SemigroupK<M>
    where M : MonoidK<M>
{
    /// <summary>
    /// Identity
    /// </summary>
    [Pure]
    public static abstract K<M, A> Empty<A>();
}
