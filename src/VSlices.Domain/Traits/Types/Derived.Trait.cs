namespace VSlices.Domain.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
///
/// </typeparam>
/// <typeparam name="BASE">
///
/// </typeparam>
public interface Derived<SELF, BASE> : DomainType<SELF>
    where SELF : Derived<SELF, BASE>
    where BASE : DomainType<BASE>
{
    /// <summary>
    ///
    /// </summary>
    /// <returns>
    ///
    /// </returns>
    BASE ToBase();
}

/// <summary>
///
/// </summary>
/// <typeparam name="SELF">
///
/// </typeparam>
/// <typeparam name="BASE">
///
/// </typeparam>
/// <typeparam name="REPR">
///
/// </typeparam>
public interface Derived<SELF, BASE, REPR> :
    Derived<SELF, BASE>,
    DomainType<SELF, REPR>
    where SELF : Derived<SELF, BASE, REPR>
    where BASE : DomainType<BASE, REPR>
{
    /// <inheritdoc/>
    REPR Represented<REPR>.To() =>
        ToBase().To();
}
