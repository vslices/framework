using VSlices.Domain.Environments.DataAccess;
using VSlices.Domain.Environments.EventBuffer;
using VSlices.Domain.Environments.Persistence;

namespace VSlices.Domain;

public interface DomainRuntime<TSelf> 
    : HasEventBuffer<TSelf>, 
      HasPersistence<TSelf>,
      HasDataAccess<TSelf>
    where TSelf : DomainRuntime<TSelf>;
