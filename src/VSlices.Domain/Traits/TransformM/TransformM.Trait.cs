using LanguageExt;

namespace VSlices.Domain.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
///
/// </typeparam>
/// <typeparam name="M">
///
/// </typeparam>
/// <typeparam name="OUT">
///
/// </typeparam>
/// <typeparam name="IN">
///
/// </typeparam>
/// <remarks>
///
/// </remarks>
public interface TransformM<SELF, M, OUT, IN> : DomainType<SELF>
    where SELF : TransformM<SELF, M, OUT, IN>
    where M : Monad<M>
{
    /// <summary>
    ///
    /// </summary>
    public static abstract ReqK<M, IN, OUT>.Completed Invariants { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr">
    ///
    /// </param>
    /// <returns>
    ///
    /// </returns>
    public static virtual FinT<M, OUT> Create(IN repr) =>
        SELF.Invariants.RunFinT(repr);

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr">
    ///
    /// </param>
    /// <returns>
    ///
    /// </returns>
    public static virtual K<M, OUT> New(IN repr) =>
        SELF.Create(repr)
            .Run()
            .Map(f => f.ThrowIfFail());

}

/// <inheritdoc/>
public interface TransformM<SELF, M, IN> : TransformM<SELF, M, SELF, IN>
    where SELF : TransformM<SELF, M, IN>
    where M : Monad<M>;
