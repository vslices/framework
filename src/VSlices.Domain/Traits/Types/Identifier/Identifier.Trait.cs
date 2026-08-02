
using VSlices.Domain.Traits;

namespace VSlices.Domain.Traits;

/// <summary>
/// Represents a domain identifier with value-based equality semantics.
/// </summary>
/// <typeparam name="SELF">
/// The concrete identifier type.
/// </typeparam>
public interface Identifier<SELF> :
    DomainType<SELF>,
    DiscreteSpace<SELF>
    where SELF : Identifier<SELF>;

/// <summary>
/// Represents a domain identifier backed by an underlying
/// representation type.
/// </summary>
/// <typeparam name="SELF">
/// The concrete identifier type.
/// </typeparam>
/// <typeparam name="REPR">
/// The underlying representation type used by the identifier.
/// </typeparam>
public interface IdentifierType<SELF, REPR> :
    Identifier<SELF>,
    DomainType<SELF, REPR>
    where SELF : IdentifierType<SELF, REPR>;
