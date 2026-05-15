namespace VSlices.Domain.Envs;

public interface HasPersistence<RT> : Has<Eff<RT>, PersistenceIO>;

public partial record PersistenceEnv<RT>
    where RT : HasPersistence<RT>
{
    static Eff<RT, PersistenceIO> persistenceIO => 
        Has<Eff<RT>, RT, PersistenceIO>.ask.As();

    public static Eff<RT, Unit> start() =>
        persistenceIO.Bind(io => io.Start());

    public static Eff<RT, Unit> commit() =>
        persistenceIO.Bind(io => io.Commit());

    public static Eff<RT, Unit> rollback() =>
        persistenceIO.Bind(io => io.Rollback());

}

