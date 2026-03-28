using VSlices.Domain.Environments.EventBuffer;
using VSlices.Domain.Environments.Persistance;

namespace VSlices.Domain;

public interface DomainRuntime<TSelf, M> 
    : Has<M, EventBufferIO>, 
      Has<M, PersistenceIO<M>>
    where TSelf : DomainRuntime<TSelf, M>
    where M : MonadIO<M>;
