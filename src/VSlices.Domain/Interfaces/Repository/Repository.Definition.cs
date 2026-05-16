using System.ComponentModel;

namespace VSlices.Domain;

/// <summary>
/// Represents a repository interface for performing operations on a collection of aggregate roots.
/// </summary>
/// <remarks>
/// DO NOT USE, this interface in intended to be used as a base interface for more specific
/// repository interfaces that operate on aggregate roots.
/// </remarks>

[EditorBrowsable(EditorBrowsableState.Never)]
public interface IRepository;

/// <summary>
/// Represents a repository interface for performing operations on a collection of aggregate roots.
/// </summary>
/// <remarks>
/// DO NOT USE, this interface in intended to be used as a base interface for more specific
/// repository interfaces that operate on aggregate roots.
/// </remarks>

[EditorBrowsable(EditorBrowsableState.Never)]
public interface IRepository<RT, A> : IRepository
    where A : AggregateRoot<A>
{
    /// <summary>
    /// Adds a range of aggregate roots to the repository.
    /// </summary>
    /// <param name="roots">
    /// The sequence of aggregate roots to be added to the repository.
    /// </param>
    /// <returns>
    /// An effect that, when executed, returns the sequence of aggregate roots
    /// that were successfully added to the repository.
    /// </returns>
    Eff<RT, Seq<A>> AddRange(Seq<A> roots);

    /// <summary>
    /// Updates a range of aggregate roots in the repository.
    /// </summary>
    /// <param name="aggregateRoot">
    /// The sequence of aggregate roots to be updated in the repository.
    /// </param>
    /// <returns>
    /// An effect that, when executed, returns the sequence of aggregate roots
    /// that were successfully updated in the repository.
    /// </returns>
    Eff<RT, Seq<A>> UpdateRange(Seq<A> aggregateRoot);

    /// <summary>
    /// Deletes a range of aggregate roots from the repository.
    /// </summary>
    /// <param name="aggregateRoot">
    /// The sequence of aggregate roots to be deleted from the repository.
    /// </param>
    /// <returns>
    /// An effect that, when executed, returns the sequence of aggregate roots
    /// that were successfully deleted from the repository.
    /// </returns>
    Eff<RT, Seq<A>> DeleteRange(Seq<A> aggregateRoot);

    /// <summary>
    /// Adds a single aggregate root to the repository.
    /// </summary>
    /// <param name="root">
    /// The aggregate root to be added to the repository.
    /// </param>
    /// <returns>
    /// An effect that, when executed, returns the aggregate root
    /// that was successfully added to the repository.
    /// </returns>
    Eff<RT, A> Add(A root) =>
        AddRange([root]).Map(r => r.First());
    
    /// <summary>
    /// Updates a single aggregate root in the repository.
    /// </summary>
    /// <param name="root">
    /// The aggregate root to be updated in the repository.
    /// </param>
    /// <returns>
    /// An effect that, when executed, returns the aggregate root
    /// that was successfully updated in the repository.
    /// </returns>
    Eff<RT, A> Update(A root) =>
        UpdateRange([root]).Map(r => r.First());

    /// <summary>
    /// Deletes a single aggregate root from the repository.
    /// </summary>
    /// <param name="root">
    /// The aggregate root to be deleted from the repository.
    /// </param>
    /// <returns>
    /// An effect that, when executed, returns the aggregate root
    /// that was successfully deleted from the repository.
    /// </returns>
    Eff<RT, A> Delete(A root) =>
        DeleteRange([root]).Map(r => r.First());

}

/// <summary>
/// Represents a repository interface for performing operations on a collection of aggregate roots
/// with a specific identifier in the domain layer.
/// </summary>
/// <typeparam name="RT">
/// The runtime environment or context in which the repository operates.
/// </typeparam>
/// <typeparam name="ROOT">
/// The type of the aggregate root managed by the repository.
/// </typeparam>
/// <typeparam name="ID">
/// The type of the identifier associated with the aggregate root.
/// </typeparam>
public interface IRepository<RT, ROOT, ID> : IRepository<RT, ROOT>
    where ROOT : AggregateRoot<ROOT, ID>
    where ID : Identifier<ID>
{
    /// <summary>
    /// Attempts to retrieve an aggregate root by its identifier, returning an optional result.
    /// </summary>
    /// <param name="id">
    /// The identifier of the aggregate root to retrieve.
    /// </param>
    /// <returns>
    /// An <see cref="OptionT{T}"/> containing the aggregate root if found, or an empty option if not found.
    /// </returns>
    /// <remarks>
    /// This method provides a safe way to attempt retrieval of an aggregate root without throwing exceptions
    /// when the entity is not found.
    /// </remarks>
    OptionT<Eff<RT>, ROOT> ReadOrOption(ID id);

    /// <summary>
    /// Retrieves an aggregate root by its identifier.
    /// </summary>
    /// <param name="id">
    /// The identifier of the aggregate root to retrieve.
    /// </param>
    /// <returns>
    /// An <see cref="Eff{RT, ROOT}"/> representing the operation to retrieve the aggregate root.
    /// If the entity is not found, the operation will result in an error with a 404 status.
    /// </returns>
    /// <remarks>
    /// This method ensures that an aggregate root is either successfully retrieved or an error is returned,
    /// providing a clear and consistent way to handle missing entities.
    /// </remarks>
    Eff<RT, ROOT> Read(ID id) =>
        ReadOrOption(id).Match(Some: Eff<RT, ROOT> (s) => Pure(s),
                               None: Eff<RT, ROOT> () => Error.New(404, $"Entity {typeof(ROOT).Name} NotFound"))
                        .As()
                        .Flatten();
}
