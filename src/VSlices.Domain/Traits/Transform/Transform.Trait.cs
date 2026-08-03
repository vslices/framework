namespace VSlices.Domain.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
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
public interface Transform<SELF, TYPE, IN> : DomainType<SELF>
    where SELF : Transform<SELF, TYPE, IN>
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
    public static abstract Fin<TYPE> From(IN repr);

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr">
    ///
    /// </param>
    /// <returns>
    ///
    /// </returns>
    /// <exception cref="Exception">
    ///
    /// </exception>
    public static virtual TYPE FromUnsafe(IN repr) =>
        SELF.From(repr).ThrowIfFail();
}

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
///
/// </typeparam>
/// <typeparam name="IN">
///
/// </typeparam>
/// <remarks>
///
/// </remarks>
public interface Transform<SELF, IN> : Transform<SELF, SELF, IN>
    where SELF : Transform<SELF, IN>;
