using VSlices.Domain.Interfaces;
using VSlices.Domain.Traits;
using VSlices.Domain.Traits.EventBuffer;
using VSlices.Domain.Traits.Persistance;

namespace VSlices.Domain.Service;

public sealed class TransactionRunner<RT>
    where RT : Has<Eff<RT>, PersistenceIO<RT>>, 
               Has<Eff<RT>, EventBufferIO>
{
    public static Eff<RT, A> RunAtomic<A>(Func<IUnitOfWork<RT>, Eff<RT, A>> operation) =>
        from unitOfWork in Persistence<RT>.getUnitOfWork()
        from result in operation(unitOfWork)
        from _1 in unitOfWork.Commit()
        from events in EventBuffer<RT>.commit()
        select result;

}
