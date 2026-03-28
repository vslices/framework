using VSlices.Domain.Interfaces;

namespace VSlices.Domain.Environments.Persistance;

public record PersistenceEnv<M, RT>
    where M : MonadIO<M>
    where RT : Has<M, PersistenceIO<M>>
{
    static K<M, PersistenceIO<M>> persistenceIO => Has<M, RT, PersistenceIO<M>>.ask;

    public static K<M, IUnitOfWork<M>> getUnitOfWork() =>
        persistenceIO.Bind(io => io.GetUnitOfWork());

}

public record PersistenceEnv<RT>
    where RT : Has<Eff<RT>, PersistenceIO<Eff<RT>>>
{
    public static Eff<RT, IUnitOfWork<Eff<RT>>> getUnitOfWork() =>
        PersistenceEnv<Eff<RT>, RT>.getUnitOfWork().As();

}
