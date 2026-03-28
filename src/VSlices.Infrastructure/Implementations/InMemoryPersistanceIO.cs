using VSlices.Domain.Environments.Persistence;

namespace VSlices.Infrastructure.Implementations;

public sealed class InMemoryPersistanceIO : PersistenceIO
{
    public IO<Unit> Commit() => throw new NotImplementedException();
    public IO<Unit> Rollback() => throw new NotImplementedException();
    public IO<Unit> Start() => throw new NotImplementedException();
}
