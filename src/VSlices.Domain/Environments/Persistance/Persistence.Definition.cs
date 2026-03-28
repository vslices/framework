using VSlices.Domain.Interfaces;

namespace VSlices.Domain.Environments.Persistance;

public interface PersistenceIO<M>
    where M : MonadIO<M>
{
    IO<IUnitOfWork<M>> GetUnitOfWork();

}
