using VSlices.Monads;

namespace VSlices.Domain.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
///
/// </typeparam>
/// <typeparam name="A">
///
/// </typeparam>
/// <typeparam name="B">
///
/// </typeparam>
/// <remarks>
///
/// </remarks>
public interface Transform<SELF, A, B> : DomainType<SELF>
    where SELF : Transform<SELF, A, B>
    where A : DomainType<A>
{
    /// <summary>
    ///
    /// </summary>
    /// <returns>
    ///
    /// </returns>
    static abstract Req<B, A> Invariants { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr">
    ///
    /// </param>
    /// <returns>
    ///
    /// </returns>
    public static virtual Fin<A> Create(B repr) =>
        SELF.Invariants.Onto(repr);

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr">
    ///
    /// </param>
    /// <returns>
    ///
    /// </returns>
    public static virtual A New(B repr) =>
        SELF.Create(repr).ThrowIfFail();
}

/// <summary>
///
/// </summary>
/// <typeparam name="A">
///
/// </typeparam>
/// <typeparam name="B">
///
/// </typeparam>
/// <remarks>
///
/// </remarks>
public interface Transform<A, B> : Transform<A, A, B>
    where A : Transform<A, B>;

