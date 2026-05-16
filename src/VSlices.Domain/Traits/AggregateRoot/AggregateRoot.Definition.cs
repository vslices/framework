using System.ComponentModel;

namespace VSlices.Domain.Traits;

/// <summary>
/// Represents the base interface for aggregate root entities in a domain-driven design context.
/// </summary>
/// <typeparam name="SELF">
/// The type of the aggregate root itself, used to enforce type constraints and ensure consistency.
/// </typeparam>
[EditorBrowsable(EditorBrowsableState.Never)]
public partial interface AggregateRoot<SELF>;

/// <summary>
/// Represents the base interface for aggregate root entities in a domain-driven design context,
/// with an identifier to uniquely distinguish each aggregate root.
/// </summary>
/// <typeparam name="SELF">
/// The type of the aggregate root itself, used to enforce type constraints and ensure consistency.
/// </typeparam>
/// <typeparam name="ID">
/// The type of the identifier used to uniquely identify the aggregate root.
/// </typeparam>
public partial interface AggregateRoot<SELF, ID> : 
    AggregateRoot<SELF>, 
    DomainEntity<SELF, ID>
    where SELF : AggregateRoot<SELF, ID>
    where ID : Identifier<ID>;

/// <summary>
/// Represents the base interface for aggregate root entities in a domain-driven design context,
/// supporting a specific identifier type and a representation type.
/// </summary>
/// <typeparam name="SELF">
/// The type of the aggregate root itself, used to enforce type constraints and ensure consistency.
/// </typeparam>
/// <typeparam name="ID">
/// The type of the identifier associated with the aggregate root, which must implement <see cref="Identifier{ID}"/>.
/// </typeparam>
/// <typeparam name="REPR">
/// The type of the representation associated with the aggregate root, used for external or internal representation purposes.
/// </typeparam>
public partial interface AggregateRoot<SELF, ID, REPR> : 
    AggregateRoot<SELF, ID>, 
    DomainEntity<SELF, ID, REPR>
    where SELF : AggregateRoot<SELF, ID, REPR>, DomainType<SELF, REPR>
    where ID : Identifier<ID>;
