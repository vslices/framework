using System;
using LanguageExt.Traits;
using VSlices.Domain.Traits;

namespace VSlices;

public static partial class VSlicesDomainPrelude
{
    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation.
    /// </summary>
    /// <typeparam name="M">The monadic type that defines the context.</typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, REPR}"/> 
    /// and <see cref="DomainType{T, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>
    /// An instance of <see cref="K{M, T}"/> created from the provided unsafe representation.
    /// </returns>
    /// <remarks>
    /// This method relies on the domain type's implementation of 
    /// <see cref="DomainFactoryM{T, M, REPR}.FromUnsafeM"/> to construct the result.
    /// </remarks>
    public static K<M, T> Unsafe<M, T>(bool repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, bool>, DomainType<T, bool> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="char"/>.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic type that defines the context for the operation.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, char}"/> 
    /// and <see cref="DomainType{T, char}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The unsafe representation of type <see cref="char"/> used to create the instance.
    /// </param>
    /// <returns>
    /// An instance of <see cref="K{M, T}"/> created from the provided unsafe representation.
    /// </returns>
    public static K<M, T> Unsafe<M, T>(char repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, char>, DomainType<T, char> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates a new instance of <see cref="K{M, T}"/> from an unsafe string representation.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic type that defines the computational context.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, string}"/> 
    /// and <see cref="DomainType{T, string}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The unsafe string representation used to create the domain type.
    /// </param>
    /// <returns>
    /// An instance of <see cref="K{M, T}"/> created from the provided unsafe string representation.
    /// </returns>
    public static K<M, T> Unsafe<M, T>(string repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, string>, DomainType<T, string> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="byte"/>.
    /// </summary>
    /// <typeparam name="M">The monad type that defines the computational context.</typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, byte}"/> and <see cref="DomainType{T, byte}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>An instance of <see cref="K{M, T}"/> created from the unsafe representation.</returns>
    public static K<M, T> Unsafe<M, T>(byte repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, byte>, DomainType<T, byte> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="short"/>.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic type that defines the context in which the operation is performed.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, short}"/> 
    /// and <see cref="DomainType{T, short}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The unsafe representation of the domain type.
    /// </param>
    /// <returns>
    /// An instance of <see cref="K{M, T}"/> created from the provided unsafe representation.
    /// </returns>
    public static K<M, T> Unsafe<M, T>(short repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, short>, DomainType<T, short> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="ushort"/>.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic type that defines the computational context.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, ushort}"/> 
    /// and <see cref="DomainType{T, ushort}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The unsafe representation of type <see cref="ushort"/> to be converted into the domain type.
    /// </param>
    /// <returns>
    /// An instance of <see cref="K{M, T}"/> representing the domain type within the monadic context.
    /// </returns>
    public static K<M, T> Unsafe<M, T>(ushort repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, ushort>, DomainType<T, ushort> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="int"/>.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic type that defines the computational context.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type that implements <see cref="DomainFactoryM{T, M, int}"/> and <see cref="DomainType{T, int}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The unsafe representation of the domain type.
    /// </param>
    /// <returns>
    /// An instance of <see cref="K{M, T}"/> created from the provided unsafe representation.
    /// </returns>
    public static K<M, T> Unsafe<M, T>(int repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, int>, DomainType<T, int> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="uint"/>.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic type that defines the context in which the operation is performed.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, uint}"/> and <see cref="DomainType{T, uint}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The unsafe representation of the value of type <see cref="uint"/>.
    /// </param>
    /// <returns>
    /// An instance of <see cref="K{M, T}"/> created from the provided unsafe representation.
    /// </returns>
    public static K<M, T> Unsafe<M, T>(uint repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, uint>, DomainType<T, uint> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="long"/>.
    /// </summary>
    /// <typeparam name="M">The monad type that defines the computation context.</typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, long}"/> and <see cref="DomainType{T, long}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>An instance of <see cref="K{M, T}"/> created from the unsafe representation.</returns>
    public static K<M, T> Unsafe<M, T>(long repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, long>, DomainType<T, long> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="ulong"/>.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic type that defines the computation context.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type that implements <see cref="DomainFactoryM{T, M, ulong}"/> and <see cref="DomainType{T, ulong}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The unsafe representation of the domain type.
    /// </param>
    /// <returns>
    /// An instance of <see cref="K{M, T}"/> created from the unsafe representation.
    /// </returns>
    public static K<M, T> Unsafe<M, T>(ulong repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, ulong>, DomainType<T, ulong> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="float"/>.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic type that defines the context in which the operation is performed.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, float}"/> and <see cref="DomainType{T, float}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The unsafe representation of the domain type.
    /// </param>
    /// <returns>
    /// An instance of <see cref="K{M, T}"/> created from the provided unsafe representation.
    /// </returns>
    public static K<M, T> Unsafe<M, T>(float repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, float>, DomainType<T, float> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="double"/>.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic context in which the operation is performed. Must implement <see cref="Monad{M}"/>.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type that represents the value. Must implement both <see cref="DomainFactoryM{T, M, double}"/> 
    /// and <see cref="DomainType{T, double}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the value.</param>
    /// <returns>
    /// An instance of <see cref="K{M, T}"/> encapsulating the domain type <typeparamref name="T"/> 
    /// within the monadic context <typeparamref name="M"/>.
    /// </returns>
    /// <remarks>
    /// This method is considered unsafe because it directly converts the provided representation 
    /// into the domain type without additional validation.
    /// </remarks>
    public static K<M, T> Unsafe<M, T>(double repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, double>, DomainType<T, double> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="decimal"/>.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic type that defines the computational context.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, decimal}"/> 
    /// and <see cref="DomainType{T, decimal}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The unsafe representation of the domain value.
    /// </param>
    /// <returns>
    /// An instance of <see cref="K{M, T}"/> representing the domain value.
    /// </returns>
    public static K<M, T> Unsafe<M, T>(decimal repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, decimal>, DomainType<T, decimal> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="DateOnly"/>.
    /// </summary>
    /// <typeparam name="M">The monadic type that defines the computational context.</typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, DateOnly}"/> 
    /// and <see cref="DomainType{T, DateOnly}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>An instance of <see cref="K{M, T}"/> created from the unsafe representation.</returns>
    public static K<M, T> Unsafe<M, T>(DateOnly repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, DateOnly>, DomainType<T, DateOnly> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="TimeOnly"/>.
    /// </summary>
    /// <typeparam name="M">The monad type that defines the computational context.</typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, TimeOnly}"/> and 
    /// <see cref="DomainType{T, TimeOnly}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of type <see cref="TimeOnly"/>.</param>
    /// <returns>An instance of <see cref="K{M, T}"/> created from the provided unsafe representation.</returns>
    public static K<M, T> Unsafe<M, T>(TimeOnly repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, TimeOnly>, DomainType<T, TimeOnly> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="DateTime"/>.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic type that defines the computational context.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, DateTime}"/> 
    /// and <see cref="DomainType{T, DateTime}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The unsafe representation of the domain type.
    /// </param>
    /// <returns>
    /// An instance of <see cref="K{M, T}"/> created from the provided unsafe representation.
    /// </returns>
    public static K<M, T> Unsafe<M, T>(DateTime repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, DateTime>, DomainType<T, DateTime> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic type that defines the computational context.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type that implements <see cref="DomainFactoryM{T, M, DateTimeOffset}"/> 
    /// and <see cref="DomainType{T, DateTimeOffset}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The unsafe representation of the domain type.
    /// </param>
    /// <returns>
    /// A monadic instance of <see cref="K{M, T}"/> representing the domain type.
    /// </returns>
    public static K<M, T> Unsafe<M, T>(DateTimeOffset repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, DateTimeOffset>, DomainType<T, DateTimeOffset> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> using an unsafe representation of the specified type.
    /// </summary>
    /// <typeparam name="M">The monadic type that defines the context for the operation.</typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, TimeSpan}"/> 
    /// and <see cref="DomainType{T, TimeSpan}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>An instance of <see cref="K{M, T}"/> created from the unsafe representation.</returns>
    /// <remarks>
    /// This method is intended for advanced scenarios where the representation is known to be valid.
    /// Use with caution, as improper usage may lead to runtime errors or undefined behavior.
    /// </remarks>
    public static K<M, T> Unsafe<M, T>(TimeSpan repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, TimeSpan>, DomainType<T, TimeSpan> =>
        T.FromUnsafeM(repr);

    /// <summary>
    /// Creates an instance of <see cref="K{M, T}"/> from an unsafe representation of type <see cref="Guid"/>.
    /// </summary>
    /// <typeparam name="M">
    /// The monadic type that provides the context for the operation.
    /// </typeparam>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactoryM{T, M, Guid}"/> 
    /// and <see cref="DomainType{T, Guid}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The unsafe representation of the domain type.
    /// </param>
    /// <returns>
    /// An instance of <see cref="K{M, T}"/> created from the unsafe representation.
    /// </returns>
    public static K<M, T> Unsafe<M, T>(Guid repr)
        where M : Monad<M>
        where T : DomainFactoryM<T, M, Guid>, DomainType<T, Guid> =>
        T.FromUnsafeM(repr);

}
