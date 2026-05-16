using VSlices.Domain.Envs;

namespace VSlices.Domain;

/// <summary>
/// Represents a domain runtime environment that integrates persistence, 
/// data access, and clock-related capabilities.
/// </summary>
/// <typeparam name="SELF">
/// The type of the runtime environment implementing this interface, 
/// which must also satisfy the <see cref="DomainRuntime{SELF}"/> constraint.
/// </typeparam>
public interface DomainRuntime<SELF>
    : HasPersistence<SELF>,
      HasDataAccess<SELF>,
      HasClock<SELF>
    where SELF : DomainRuntime<SELF>;