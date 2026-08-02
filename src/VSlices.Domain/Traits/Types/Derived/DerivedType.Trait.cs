using System;

namespace VSlices.Domain.Traits;

/// <summary>
/// Represents a domain type derived from another domain type.
/// </summary>
/// <typeparam name="SELF">The concrete derived domain type.</typeparam>
/// <typeparam name="BASE">The base domain type being wrapped or specialized.</typeparam>
public interface Derived<SELF, BASE> : DomainType<SELF>
    where SELF : Derived<SELF, BASE>
    where BASE : DomainType<BASE>
{
    /// <summary>
    /// Returns the base domain value from which this value is derived.
    /// </summary>
    /// <returns>The underlying base domain value.</returns>
    BASE ToBase();
}

/// <summary>
/// Represents a derived domain type that can be converted to a underlying representation.
/// </summary>
/// <typeparam name="SELF">The concrete derived domain type.</typeparam>
/// <typeparam name="BASE">The base domain type being wrapped or specialized.</typeparam>
/// <typeparam name="REPR">The representation shared by both the derived and base domain types.</typeparam>
public interface DerivedType<SELF, BASE, REPR> :
    Derived<SELF, BASE>,
    DomainType<SELF, REPR>
    where SELF : DerivedType<SELF, BASE, REPR>
    where BASE : DomainType<BASE, REPR>
{
    /// <inheritdoc/>
    REPR DomainRepresent<REPR>.To() =>
        ToBase().To();
}

/// <summary>
/// Represents a derived domain type that can be constructed from the same representation
/// as its base domain type through a pure factory.
/// </summary>
/// <typeparam name="SELF">The concrete derived domain type.</typeparam>
/// <typeparam name="BASE">The base domain type being wrapped or specialized.</typeparam>
/// <typeparam name="REPR">The representation shared by both the derived and base domain types.</typeparam>
