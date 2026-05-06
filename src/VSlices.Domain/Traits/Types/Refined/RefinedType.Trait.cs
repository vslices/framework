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

/// <summary>
/// Represents a refined domain type that can be constructed from the same representation
/// as its base domain type through a pure factory.
/// </summary>
/// <typeparam name="SELF">The concrete refined domain type.</typeparam>
/// <typeparam name="BASE">The base domain type being refined.</typeparam>
/// <typeparam name="REPR">The representation shared by both the refined and base domain types.</typeparam>
public interface RefinedTypeFactory<SELF, BASE, REPR> :
    RefinedType<SELF, BASE, REPR>,
    DomainFactory<SELF, REPR>
    where SELF : RefinedTypeFactory<SELF, BASE, REPR>
    where BASE : DomainType<BASE, REPR>, DomainTypeFactory<BASE, REPR>
{
    /// <summary>
    /// Attempts to refine an already valid base domain value.
    /// </summary>
    /// <param name="repr">The valid base domain value to refine.</param>
    /// <returns>
    /// A successful refined value when the additional constraints are satisfied;
    /// otherwise, a failed <see cref="Fin{A}"/>.
    /// </returns>
    static abstract Fin<SELF> From(BASE repr);

    /// <inheritdoc/>
    static Fin<SELF> DomainFactory<SELF, SELF, REPR>.From(REPR repr) => 
        BASE.From(repr).Bind(SELF.From);
}

/// <summary>
/// Represents a refined domain type that can be constructed from the same representation
/// as its base domain type through an effectful factory.
/// </summary>
/// <typeparam name="SELF">The concrete refined domain type.</typeparam>
/// <typeparam name="BASE">The base domain type being refined.</typeparam>
/// <typeparam name="M">The effect context used during construction.</typeparam>
/// <typeparam name="REPR">The representation shared by both the refined and base domain types.</typeparam>
public interface RefinedTypeFactoryM<SELF, BASE, M, REPR> :
    RefinedType<SELF, BASE, REPR>,
    DomainFactoryM<SELF, M, REPR>
    where SELF : RefinedTypeFactoryM<SELF, BASE, M, REPR>
    where BASE : DomainType<BASE, REPR>, DomainTypeFactoryM<BASE, M, REPR>
    where M : Monad<M>
{
    /// <summary>
    /// Attempts to refine an already valid base domain value inside the effect context
    /// <typeparamref name="M"/>.
    /// </summary>
    /// <param name="repr">The valid base domain value to refine.</param>
    /// <returns>
    /// An effectful validation producing the refined value when successful;
    /// otherwise, a failed <see cref="FinT{M, A}"/>.
    /// </returns>
    static abstract FinT<M, SELF> FromM(BASE repr);

    /// <inheritdoc/>
    static FinT<M, SELF> DomainFactoryM<SELF, M, SELF, REPR>.FromM(REPR repr) =>
        BASE.FromM(repr).Bind(SELF.FromM);
}
