namespace VSlices.Domain.Traits;

/// <summary>
/// Represents a domain entity with a unique identifier and type-level identity.
/// </summary>
/// <typeparam name="SELF">
/// The concrete domain entity type, enabling static polymorphism and self-referential constraints.
/// </typeparam>
/// <typeparam name="ID">
/// The type of the unique identifier associated with the domain entity.
/// </typeparam>
/// <remarks>
/// <para>
/// <see cref="DomainEntity{SELF, ID}"/> serves as a foundational abstraction for domain entities,
/// combining the concept of a unique identifier (<typeparamref name="ID"/>) with the type-level identity
/// provided by <see cref="DomainType{SELF}"/>.
/// </para>
/// 
/// <para>
/// This interface is designed to support domain-driven design principles, ensuring that each entity
/// is uniquely identifiable and participates in the domain model's type hierarchy.
/// </para>
/// </remarks>
public interface DomainEntity<SELF, ID> : DomainType<SELF>
    where SELF : DomainType<SELF>
    where ID : Identifier<ID>
{
    /// <summary>
    /// Gets the unique identifier associated with the domain entity.
    /// </summary>
    /// <value>
    /// The unique identifier of type <typeparamref name="ID"/>.
    /// </value>
    /// <remarks>
    /// This property provides a means to uniquely identify an instance of the domain entity
    /// within the context of the domain model. The identifier is expected to adhere to the
    /// value-based equality semantics defined by the <see cref="Identifier{SELF}"/> interface.
    /// </remarks>
    ID Id { get; }

}

/// <summary>
/// Represents a domain entity with a unique identifier and a specific representation.
/// </summary>
/// <typeparam name="SELF">The type of the domain entity itself, used for enforcing type constraints.</typeparam>
/// <typeparam name="ID">The type of the unique identifier associated with the domain entity.</typeparam>
/// <typeparam name="REPR">The type of the representation of the domain entity.</typeparam>
public interface DomainEntity<SELF, ID, REPR> :
    DomainEntity<SELF, ID>
    where SELF : DomainType<SELF, REPR>
    where ID : Identifier<ID>;
