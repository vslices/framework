namespace VSlices.Domain.Environments;

public interface PersistenceIO
{
    IO<Unit> Commit();

    IO<Unit> Rollback();

    IO<Unit> Start();
}
