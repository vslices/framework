using VSlices.Domain.Environments.Clock;
using VSlices.Domain.Environments.DataAccess;
using VSlices.Domain.Environments.Persistence;

namespace VSlices.Domain;

public interface DomainRuntime<SELF>
    : CoreRuntime<SELF>,
      HasPersistence<SELF>,
      HasDataAccess<SELF>,
      HasClock<SELF>
    where SELF : DomainRuntime<SELF>;
