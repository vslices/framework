using VSlices.Monads;
using LanguageExt;

namespace VSlices.Domain.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
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
public interface Transform<SELF, OUT, IN> : DomainType<SELF>
    where SELF : Transform<SELF, OUT, IN>
    where OUT : DomainType<OUT>
{
    /// <summary>
    ///
    /// </summary>
    /// <returns>
    ///
    /// </returns>
    static abstract Req<IN, OUT, IN, OUT> Invariants { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr">
    ///
    /// </param>
    /// <returns>
    ///
    /// </returns>
    public static virtual Fin<OUT> Create(IN repr) =>
        SELF.Invariants.RunFin(repr);

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr">
    ///
    /// </param>
    /// <returns>
    ///
    /// </returns>
    public static virtual OUT New(IN repr) =>
        SELF.Create(repr).ThrowIfFail();

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr"></param>
    /// <returns></returns>
    public static virtual Seq<OUT> New(Seq<IN> repr) =>
        repr.Map(SELF.New);
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

