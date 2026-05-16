namespace VSlices.Domain.Envs;

/// <summary>
/// Represents an environment that supports persistence operations.
/// </summary>
/// <typeparam name="RT">
/// The runtime type that implements this interface, enabling persistence-related functionality.
/// </typeparam>
public interface HasPersistence<RT> : Has<Eff<RT>, PersistenceIO>;

/// <summary>
/// Represents a persistence environment that provides transactional operations such as 
/// starting, committing, and rolling back transactions.
/// </summary>
/// <typeparam name="RT">
/// The runtime type that implements <see cref="HasPersistence{RT}"/>, enabling the use of persistence-related functionality.
/// </typeparam>
public record PersistenceEnv<RT>
    where RT : HasPersistence<RT>
{
    private static Eff<RT, PersistenceIO> persistenceIO => 
        Has<Eff<RT>, RT, PersistenceIO>.ask.As();

    /// <summary>
    /// Initiates a new transactional operation within the persistence environment.
    /// </summary>
    /// <returns>
    /// An <see cref="Eff{RT, A}"/> representing the result of the start operation.
    /// </returns>
    public static Eff<RT, Unit> start() =>
        persistenceIO.Bind(io => io.Start());
    
    /// <summary>
    /// Commits the current transaction within the persistence environment, ensuring that
    /// all changes made during the transaction are saved and made permanent.
    /// </summary>
    /// <returns>
    /// An <see cref="Eff{RT, A}"/> representing the result of the commit operation.
    /// </returns>
    public static Eff<RT, Unit> commit() =>
        persistenceIO.Bind(io => io.Commit());

    /// <summary>
    /// Rolls back the current transaction within the persistence environment, undoing all changes
    /// made during the transaction and restoring the state to its previous condition.
    /// </summary>
    /// <returns>
    /// An <see cref="Eff{RT, Unit}"/> representing the result of the rollback operation.
    /// </returns>
    public static Eff<RT, Unit> rollback() =>
        persistenceIO.Bind(io => io.Rollback());

}

