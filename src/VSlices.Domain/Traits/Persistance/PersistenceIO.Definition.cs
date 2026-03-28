using VSlices.Domain.Interfaces;

namespace VSlices.Domain.Traits;

public interface PersistenceIO<RT>
{
    IO<IUnitOfWork<RT>> GetUnitOfWork();

}
