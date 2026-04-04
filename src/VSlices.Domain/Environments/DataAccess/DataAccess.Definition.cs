using VSlices.Domain.Interfaces;

namespace VSlices.Domain.Environments.DataAccess;

public interface DataAccessIO
{
    IO<T> Get<T>()
        where T : IRepository;
}
