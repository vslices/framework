using VSlices.Domain.Envs;

namespace VSlices.Domain;

public interface DomainRuntime<SELF>
    : HasPersistence<SELF>,
      HasDataAccess<SELF>,
      HasClock<SELF>
    where SELF : DomainRuntime<SELF>;
