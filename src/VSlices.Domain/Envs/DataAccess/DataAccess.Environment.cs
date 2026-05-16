namespace VSlices.Domain.Envs;

/// <summary>
/// Represents an interface that defines a contract for data access capabilities
/// within a specific runtime environment.
/// </summary>
/// <typeparam name="TSelf">
/// The type of the runtime environment implementing this interface, 
/// which must also satisfy the <see cref="HasDataAccess{TSelf}"/> constraint.
/// </typeparam>
public interface HasDataAccess<TSelf> : Has<Eff<TSelf>, DataAccessIO>;

/// <summary>
/// Represents a runtime environment that provides data access capabilities
/// constrained by the <see cref="HasDataAccess{TSelf}"/> interface.
/// </summary>
/// <typeparam name="RT">
/// The type of the runtime environment implementing this record, 
/// which must satisfy the <see cref="HasDataAccess{RT}"/> constraint.
/// </typeparam>
public record DataAccessEnv<RT>
    where RT : HasDataAccess<RT>
{
    /// <summary>
    /// Represents an effectful computation that provides access to the 
    /// <see cref="DataAccessIO"/> interface within the runtime environment.
    /// </summary>
    /// <remarks>
    /// This property is protected and static, enabling derived types to access 
    /// the data access capabilities defined by the <see cref="DataAccessIO"/> interface.
    /// </remarks>
    protected static Eff<RT, DataAccessIO> dataAccessIO =>
        Has<Eff<RT>, RT, DataAccessIO>.ask.As();

    /// <summary>
    /// Retrieves an instance of the specified repository type from the data access environment.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the repository to retrieve. Must implement <see cref="IRepository"/>.
    /// </typeparam>
    /// <returns>
    /// An effect computation that, when executed, provides an instance of type <typeparamref name="T"/>.
    /// </returns>
    public static Eff<RT, T> get<T>()
        where T : IRepository =>
        dataAccessIO.Bind(io => io.Get<T>());

}
