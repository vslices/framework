using VSlices.Domain.Environments.Clock;
using VSlices.Domain.Environments.DataAccess;
using VSlices.Domain.Environments.Persistence;

namespace VSlices.Domain;

public interface DomainRuntime<TSelf> 
    : HasPersistence<TSelf>,
      HasDataAccess<TSelf>,
      HasClockAccess<TSelf>
    where TSelf : DomainRuntime<TSelf>;
