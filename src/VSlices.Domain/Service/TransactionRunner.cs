using VSlices.Domain.Environments.EventBuffer;
using VSlices.Domain.Environments.Persistance;
using VSlices.Domain.Interfaces;

namespace VSlices.Domain.Service;

public sealed class TransactionRunner<M, RT>
    where M : MonadIO<M>
    where RT : Has<M, PersistenceIO<M>>, 
               Has<M, EventBufferIO>
{
    public static K<M, A> RunAtomic<A>(Func<IUnitOfWork<M>, K<M, A>> operation) =>
        from unitOfWork in PersistenceEnv<M, RT>.getUnitOfWork()
        from result in operation(unitOfWork)
        from _1 in unitOfWork.Commit()
        from events in EventBufferEnv<M, RT>.commit()
        select result;

}
