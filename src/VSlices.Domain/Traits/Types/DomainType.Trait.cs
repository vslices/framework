namespace VSlices.Domain.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
///
/// </typeparam>
/// <remarks>
///
/// </remarks>
public interface DomainType<SELF>
    where SELF : DomainType<SELF>;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
///
/// </typeparam>
/// <typeparam name="REPR">
///
/// </typeparam>
/// <remarks>
///
/// </remarks>
public interface DomainType<SELF, REPR> :
    DomainType<SELF>,
    Represented<REPR>
    where SELF : DomainType<SELF, REPR>;
