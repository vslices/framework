namespace VSlices.Domain.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
///
/// </typeparam>
public interface Maintained<SELF> :
    DomainType<SELF>,
    DiscreteSpace<SELF>
    where SELF : Maintained<SELF>
{
    /// <summary>
    ///
    /// </summary>
    static abstract Seq<SELF> All { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    static virtual Option<SELF> Find(Func<SELF, bool> f) =>
        SELF.All.Find(f);

    /// <summary>
    ///
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    static virtual SELF First(Func<SELF, bool> f) =>
        SELF.All.First(f);

    /// <summary>
    ///
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    static virtual bool Exists(Func<SELF, bool> f) =>
        SELF.All.Exists(f);
}

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
///
/// </typeparam>
/// <typeparam name="REPR">
///
/// </typeparam>
public interface Maintained<SELF, REPR> :
    Maintained<SELF>,
    DomainType<SELF, REPR>
    where SELF : Maintained<SELF, REPR>;
