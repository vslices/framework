using VSlices.Domain.Environments.Persistance;

namespace VSlices.Domain.Interfaces;

public interface IUnitOfWork<M>
    where M : MonadIO<M>
{
    K<M, Unit> Commit();

    K<M, T> GetRepository<T>()
        where T : IRepository;
}
