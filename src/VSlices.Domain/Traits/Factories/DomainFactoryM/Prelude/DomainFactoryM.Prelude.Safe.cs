using System;
using LanguageExt;
using LanguageExt.Traits;
using VSlices.Domain.Traits;

namespace VSlices;

public static partial class VSlicesDomainPrelude
{
    /// <summary>
    /// Creates a new instance of <see cref="FinT{M, T}"/> using the specified representation.
    /// </summary>
    /// <typeparam name="M">The type of the monad.</typeparam>
    /// <typeparam name="T">The type of the domain factory and domain type.</typeparam>
    /// <param name="repr">The representation value used to create the instance.</param>
    /// <returns>A new instance of <see cref="FinT{M, T}"/> created from the specified representation.</returns>
    public static FinT<M, T> New<M, T>(bool repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, bool>, DomainType<T, bool> =>
        T.FromM(repr);
    
    /// <summary>
    /// Creates a new instance of <see cref="FinT{M, T}"/> using the specified character representation.
    /// </summary>
    /// <typeparam name="M">The type of the monad implementing <see cref="Monad{M}"/>.</typeparam>
    /// <typeparam name="T">
    /// The type implementing both <see cref="DomainFactoryM{T, M, char}"/> and <see cref="DomainType{T, char}"/>.
    /// </typeparam>
    /// <param name="repr">The character representation used to create the instance.</param>
    /// <returns>A new instance of <see cref="FinT{M, T}"/>.</returns>
    public static FinT<M, T> New<M, T>(char repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, char>, DomainType<T, char> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the specified string representation.
    /// </summary>
    /// <typeparam name="M">The type of the monad.</typeparam>
    /// <typeparam name="T">The type of the domain factory and domain type.</typeparam>
    /// <param name="repr">The string representation used to create the domain type.</param>
    /// <returns>A new instance of the domain type wrapped in a monadic context.</returns>
    public static FinT<M, T> New<M, T>(string repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, string>, DomainType<T, string> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the specified byte representation.
    /// </summary>
    /// <typeparam name="M">
    /// The monad type that defines the context in which the domain type operates.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type to be created, which must implement both <see cref="DomainFactoryM{T, M, byte}"/> 
    /// and <see cref="DomainType{T, byte}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The byte representation used to create the domain type.
    /// </param>
    /// <returns>
    /// A new instance of the domain type wrapped in a <see cref="FinT{M, T}"/>.
    /// </returns>
    public static FinT<M, T> New<M, T>(byte repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, byte>, DomainType<T, byte> =>
        T.FromM(repr);
    
    /// <summary>
    /// Creates a new instance of a domain type using the specified representation.
    /// </summary>
    /// <typeparam name="M">The type of the monad.</typeparam>
    /// <typeparam name="T">The type of the domain factory and domain type.</typeparam>
    /// <param name="repr">The representation value used to create the domain type.</param>
    /// <returns>A new instance of the domain type wrapped in a monadic context.</returns>
    public static FinT<M, T> New<M, T>(short repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, short>, DomainType<T, short> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the specified representation.
    /// </summary>
    /// <typeparam name="M">The monad type that defines the context for the operation.</typeparam>
    /// <typeparam name="T">The domain type that implements both <see cref="DomainFactoryM{T, M, ushort}"/> 
    /// and <see cref="DomainType{T, ushort}"/>.</typeparam>
    /// <param name="repr">The representation used to create the domain type instance.</param>
    /// <returns>A new instance of the domain type wrapped in a <see cref="FinT{M, T}"/>.</returns>
    public static FinT<M, T> New<M, T>(ushort repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, ushort>, DomainType<T, ushort> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the specified integer representation.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic type that defines the context for the operation.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type to be created, which must implement both 
    /// <see cref="DomainFactoryM{T, M, int}"/> and <see cref="DomainType{T, int}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The integer representation used to create the domain type.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="FinT{M, T}"/> representing the created domain type.
    /// </returns>
    public static FinT<M, T> New<M, T>(int repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, int>, DomainType<T, int> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the specified representation.
    /// </summary>
    /// <typeparam name="M">
    /// The monad type that defines the computational context.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, uint}"/> 
    /// and <see cref="DomainType{T, uint}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The representation value used to create the domain type instance.
    /// </param>
    /// <returns>
    /// A <see cref="FinT{M, T}"/> instance representing the result of the creation process.
    /// </returns>
    public static FinT<M, T> New<M, T>(uint repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, uint>, DomainType<T, uint> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the specified representation.
    /// </summary>
    /// <typeparam name="M">The monad type that defines the computational context.</typeparam>
    /// <typeparam name="T">The domain type to be created, which implements <see cref="DomainFactoryM{T, M, long}"/> and <see cref="DomainType{T, long}"/>.</typeparam>
    /// <param name="repr">The representation value used to create the domain type instance.</param>
    /// <returns>A new instance of the domain type wrapped in the computational context defined by <typeparamref name="M"/>.</returns>
    public static FinT<M, T> New<M, T>(long repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, long>, DomainType<T, long> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the specified representation.
    /// </summary>
    /// <typeparam name="M">The monad type that defines the computational context.</typeparam>
    /// <typeparam name="T">The domain type to be created, which must implement both <see cref="DomainFactoryM{T, M, ulong}"/> and <see cref="DomainType{T, ulong}"/>.</typeparam>
    /// <param name="repr">The representation value used to create the domain type instance.</param>
    /// <returns>A new instance of the domain type wrapped in the computational context defined by <typeparamref name="M"/>.</returns>
    public static FinT<M, T> New<M, T>(ulong repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, ulong>, DomainType<T, ulong> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the specified representation.
    /// </summary>
    /// <typeparam name="M">The monad type that defines the context for the operation.</typeparam>
    /// <typeparam name="T">The domain type to be created, which must implement both <see cref="DomainFactoryM{SELF, M, IN}"/> and <see cref="DomainType{SELF, REPR}"/>.</typeparam>
    /// <param name="repr">The representation value used to create the domain type instance.</param>
    /// <returns>A new instance of the domain type wrapped in a monadic context.</returns>
    public static FinT<M, T> New<M, T>(float repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, float>, DomainType<T, float> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the specified representation.
    /// </summary>
    /// <typeparam name="M">The monad type that defines the context for the operation.</typeparam>
    /// <typeparam name="T">The domain type to be created, which must implement both <see cref="DomainFactoryM{T, M, double}"/> and <see cref="DomainType{T, double}"/>.</typeparam>
    /// <param name="repr">The representation value used to create the domain type instance.</param>
    /// <returns>A new instance of the domain type wrapped in a <see cref="FinT{M, T}"/>.</returns>
    public static FinT<M, T> New<M, T>(double repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, double>, DomainType<T, double> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the specified representation.
    /// </summary>
    /// <typeparam name="M">
    /// The type of the monad that defines the computational context.
    /// </typeparam>
    /// <typeparam name="T">
    /// The type of the domain factory and domain type that will be created.
    /// </typeparam>
    /// <param name="repr">
    /// The representation value used to create the domain type instance.
    /// </param>
    /// <returns>
    /// A new instance of the domain type wrapped in the specified monad.
    /// </returns>
    public static FinT<M, T> New<M, T>(decimal repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, decimal>, DomainType<T, decimal> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the specified representation.
    /// </summary>
    /// <typeparam name="M">The monad type that defines the computation context.</typeparam>
    /// <typeparam name="T">The domain type that implements <see cref="DomainFactoryM{SELF, M, TYPE, IN}"/> and <see cref="DomainType{SELF, REPR}"/>.</typeparam>
    /// <param name="repr">The representation value used to create the domain type instance.</param>
    /// <returns>A new instance of the domain type wrapped in a monadic computation.</returns>
    public static FinT<M, T> New<M, T>(DateOnly repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, DateOnly>, DomainType<T, DateOnly> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the specified representation.
    /// </summary>
    /// <typeparam name="M">The monad type that defines the computation context.</typeparam>
    /// <typeparam name="T">The domain type to be created, which must implement both <see cref="DomainFactoryM{T, M, TimeOnly}"/> 
    /// and <see cref="DomainType{T, TimeOnly}"/>.</typeparam>
    /// <param name="repr">The representation of the domain type.</param>
    /// <returns>A new instance of the domain type wrapped in a monadic computation.</returns>
    public static FinT<M, T> New<M, T>(TimeOnly repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, TimeOnly>, DomainType<T, TimeOnly> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the specified <see cref="DateTime"/> representation.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic type that defines the computational context.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, DateTime}"/> 
    /// and <see cref="DomainType{T, DateTime}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The <see cref="DateTime"/> representation used to create the domain type instance.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="FinT{M, T}"/> representing the domain type.
    /// </returns>
    public static FinT<M, T> New<M, T>(DateTime repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, DateTime>, DomainType<T, DateTime> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of the domain type <typeparamref name="T"/> using the provided representation.
    /// </summary>
    /// <typeparam name="M">The monad type that defines the context for the operation.</typeparam>
    /// <typeparam name="T">The domain type to be created, which must implement <see cref="DomainFactoryM{T, M, DateTimeOffset}"/> and <see cref="DomainType{T, DateTimeOffset}"/>.</typeparam>
    /// <param name="repr">The representation of the domain type, of type <see cref="DateTimeOffset"/>.</param>
    /// <returns>An instance of <see cref="FinT{M, T}"/> representing the created domain type.</returns>
    public static FinT<M, T> New<M, T>(DateTimeOffset repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, DateTimeOffset>, DomainType<T, DateTimeOffset> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the specified <see cref="TimeSpan"/> representation.
    /// </summary>
    /// <typeparam name="M">The monadic type that defines the context for the operation.</typeparam>
    /// <typeparam name="T">The domain type to be created, which implements both <see cref="DomainFactoryM{T, M, TimeSpan}"/> and <see cref="DomainType{T, TimeSpan}"/>.</typeparam>
    /// <param name="repr">The <see cref="TimeSpan"/> representation used to create the domain type instance.</param>
    /// <returns>A new instance of the domain type wrapped in a monadic context.</returns>
    public static FinT<M, T> New<M, T>(TimeSpan repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, TimeSpan>, DomainType<T, TimeSpan> =>
        T.FromM(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the specified <see cref="Guid"/> representation.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic context in which the domain type operates. Must implement <see cref="Monad{M}"/>.
    /// </typeparam>
    /// <typeparam name="T">
    /// The type of the domain object being created. Must implement both 
    /// <see cref="DomainFactoryM{T, M, Guid}"/> and <see cref="DomainType{T, Guid}"/>.
    /// </typeparam>
    /// <param name="repr">The <see cref="Guid"/> representation used to create the domain type.</param>
    /// <returns>
    /// A new instance of <see cref="FinT{M, T}"/> representing the created domain type.
    /// </returns>
    public static FinT<M, T> New<M, T>(Guid repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, Guid>, DomainType<T, Guid> =>
        T.FromM(repr);
}
