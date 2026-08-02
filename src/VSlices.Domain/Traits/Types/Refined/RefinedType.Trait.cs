namespace VSlices.Domain.Traits;

/// <summary>
/// Represents a refined domain type that can be constructed from already existing
/// type.
/// </summary>
/// <typeparam name="SELF">The concrete refined domain type.</typeparam>
/// <typeparam name="BASE">The base domain type being refined.</typeparam>
public interface Refined<SELF, BASE> : Derived<SELF, BASE>
    where SELF : Refined<SELF, BASE>
    where BASE : DomainType<BASE>;


/// <summary>
/// Represents a refined domain type that can be constructed from
/// already existing type and represented by some underlying part.
/// </summary>
/// <typeparam name="SELF">The concrete refined domain type.</typeparam>
/// <typeparam name="BASE">The base domain type being refined.</typeparam>
/// <typeparam name="REPR">The underlying representation of the type.</typeparam>
public interface RefinedType<SELF, BASE, REPR> :
    Refined<SELF, BASE>,
    DomainType<SELF, REPR>
    where SELF : RefinedType<SELF, BASE, REPR>
    where BASE : DomainType<BASE>;
