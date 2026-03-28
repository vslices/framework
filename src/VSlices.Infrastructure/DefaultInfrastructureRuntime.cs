using VSlices.Infrastructure.Traits.InMemoryQueueIO;

namespace VSlices.Infrastructure;

public interface DefaultInfrastructureRuntime<TSelf, M> 
    : Has<M, InMemoryQueueIO>
    where TSelf : DefaultInfrastructureRuntime<TSelf, M>
    where M : MonadIO<M>;

