using VSlices.Domain;

namespace VSlices;

public static class RepositoryExtensions
{
    extension<RT, TRoot>(IRepository<RT, TRoot> repository)
        where TRoot : AggregateRoot<TRoot>
    {
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
        public Eff<RT, TRoot> Add(TRoot root) =>
            repository.Add(root);

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
        public Eff<RT, TRoot> Update(TRoot root) =>
            repository.Update(root);

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
        public Eff<RT, TRoot> Delete(TRoot root) =>
            repository.Delete(root);
    }

    extension<RT, TRoot, TId>(IRepository<RT, TRoot, TId> repository)
        where TRoot : AggregateRoot<TRoot, TId>
        where TId : Identifier<TId>
    {
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
        public Eff<RT, TRoot> Read(TId id) =>
            repository.Read(id);
    }
}
