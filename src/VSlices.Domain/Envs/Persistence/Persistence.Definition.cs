namespace VSlices.Domain.Envs;

/// <summary>
/// Represents an interface for persistence operations, providing functionality
/// to manage transactional workflows such as starting, committing, and rolling back transactions.
/// </summary>
public interface PersistenceIO
{
    /// <summary>
    /// Commits the current transaction, ensuring that all changes made during the transaction
    /// are saved and made permanent.
    /// </summary>
    /// <returns>An <see cref="IO{T}"/> representing the result of the commit operation.</returns>
    IO<Unit> Commit();

    /// <summary>
    /// Rolls back the current transaction, undoing all changes made during the transaction
    /// and restoring the state to what it was before the transaction began.
    /// </summary>
    /// <returns>An <see cref="IO{T}"/> representing the result of the rollback operation.</returns>
    IO<Unit> Rollback();

    /// <summary>
    /// Starts a new transaction, initializing the necessary state for subsequent
    /// operations within the transactional workflow.
    /// </summary>
    /// <returns>An <see cref="IO{T}"/> representing the result of the start operation.</returns>
    IO<Unit> Start();
}
