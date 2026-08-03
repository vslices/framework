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
