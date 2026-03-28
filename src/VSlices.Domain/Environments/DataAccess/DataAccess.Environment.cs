using VSlices.Domain.Interfaces;

namespace VSlices.Domain.Environments.DataAccess;

public interface HasDataAccess<TSelf> : Has<Eff<TSelf>, DataAccessIO>;

public record DataAccessEnv<RT>
    where RT : HasDataAccess<RT>
{
    static Eff<RT, DataAccessIO> dataAccessIO =>
        Has<Eff<RT>, RT, DataAccessIO>.ask.As();

    internal static Eff<RT, T> get<T>()
        where T : IRepository =>
        dataAccessIO.Bind(io => io.Get<T>());

}
