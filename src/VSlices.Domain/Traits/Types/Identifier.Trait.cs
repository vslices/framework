namespace VSlices.Domain.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
///
/// </typeparam>
public interface Identifier<SELF> :
    DomainType<SELF>,
    DiscreteSpace<SELF>
    where SELF : Identifier<SELF>;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
///
/// </typeparam>
/// <typeparam name="REPR">
///
/// </typeparam>
public interface Identifier<SELF, REPR> :
    Identifier<SELF>,
    DomainType<SELF, REPR>
    where SELF : Identifier<SELF, REPR>;
