using VSlices.Domain.Interfaces;

namespace VSlices.Domain.Envs;

public interface DataAccessIO
{
    IO<T> Get<T>()
        where T : IRepository;
}
