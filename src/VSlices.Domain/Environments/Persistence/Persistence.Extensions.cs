namespace VSlices.Domain.Environments.Persistence;

public partial record PersistenceEnv<RT>
    where RT : HasPersistence<RT>
{
}
