using VSlices.Domain.Interfaces;
using VSlices.Domain.Traits;

namespace VSlices.Domain.Traits.Persistance;

public record Persistence<M, RT>
    where M : MonadIO<M>
    where RT : Has<M, PersistenceIO<RT>>
{
    static K<M, PersistenceIO<RT>> persistenceIO => Has<M, RT, PersistenceIO<RT>>.ask;

    public static K<M, IUnitOfWork<RT>> getUnitOfWork() =>
        persistenceIO.Bind(io => io.GetUnitOfWork());

}

public record Persistence<RT>
    where RT : Has<Eff<RT>, PersistenceIO<RT>>
{
    public static Eff<RT, IUnitOfWork<RT>> getUnitOfWork() =>
        Persistence<Eff<RT>, RT>.getUnitOfWork().As();

}
