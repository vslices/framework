namespace VSlices.Domain.Environments.Persistence;

public interface PersistenceIO
{
    IO<Unit> Commit();

    IO<Unit> Rollback();

    IO<Unit> Start();
}
