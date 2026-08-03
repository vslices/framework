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
/// <typeparam name="TYPE">
///
/// </typeparam>
/// <typeparam name="IN">
///
/// </typeparam>
/// <remarks>
///
/// </remarks>
public interface TransformM<SELF, M, TYPE, IN> : DomainType<SELF>
    where SELF : TransformM<SELF, M, TYPE, IN>
    where M : Monad<M>
    where TYPE : DomainType<TYPE>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="repr">
    ///
    /// </param>
    /// <returns>
    ///
    /// </returns>
    public static abstract FinT<M, TYPE> FromM(IN repr);

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr">
    ///
    /// </param>
    /// <returns>
    ///
    /// </returns>
    public static virtual K<M, TYPE> FromUnsafeM(IN repr) =>
        SELF.FromM(repr)
            .Run()
            .Map(f => f.ThrowIfFail());

}


/// <inheritdoc/>
public interface TransformM<SELF, M, IN> : TransformM<SELF, M, SELF, IN>
    where SELF : TransformM<SELF, M, IN>
    where M : Monad<M>;
