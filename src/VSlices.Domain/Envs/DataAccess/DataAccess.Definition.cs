namespace VSlices.Domain.Envs;

/// <summary>
/// Represents an interface for data access operations, providing functionality
/// to retrieve repository instances for specific types.
/// </summary>
public interface DataAccessIO
{
    /// <summary>
    /// Retrieves an instance of the specified repository type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the repository to retrieve. Must implement <see cref="IRepository"/>.
    /// </typeparam>
    /// <returns>
    /// An instance of type <typeparamref name="T"/> representing the requested repository.
    /// </returns>
    IO<T> Get<T>()
        where T : IRepository;
}
