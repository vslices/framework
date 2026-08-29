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
public interface Refined<SELF, BASE> : Derived<SELF, BASE>
    where SELF : Refined<SELF, BASE>
    where BASE : DomainType<BASE>;


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
public interface Refined<SELF, BASE, REPR> :
    Refined<SELF, BASE>,
    DomainType<SELF, REPR>
    where SELF : Refined<SELF, BASE, REPR>
    where BASE : DomainType<BASE>;
