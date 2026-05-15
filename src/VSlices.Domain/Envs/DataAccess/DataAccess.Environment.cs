using VSlices.Domain.Interfaces;

namespace VSlices.Domain.Envs;

public interface HasDataAccess<TSelf> : Has<Eff<TSelf>, DataAccessIO>;

public record DataAccessEnv<RT>
    where RT : HasDataAccess<RT>
{
    protected static Eff<RT, DataAccessIO> dataAccessIO =>
        Has<Eff<RT>, RT, DataAccessIO>.ask.As();

    public static Eff<RT, T> get<T>()
        where T : IRepository =>
        dataAccessIO.Bind(io => io.Get<T>());

}
