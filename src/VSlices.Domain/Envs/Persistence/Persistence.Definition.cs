namespace VSlices.Domain.Envs;

public interface PersistenceIO
{
    IO<Unit> Commit();

    IO<Unit> Rollback();

    IO<Unit> Start();
}
