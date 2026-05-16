using System;
using VSlices.Domain.Traits;

namespace VSlices;

public static partial class VSlicesDomainPrelude
{
    /// <summary>
    /// Creates an instance of the specified domain type <typeparamref name="T"/> from an unsafe representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{SELF, IN}"/> 
    /// and <see cref="DomainType{SELF, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>An instance of the domain type <typeparamref name="T"/> created from the unsafe representation.</returns>
    /// <remarks>
    /// This method bypasses validation and directly creates the domain type from the provided representation.
    /// Use with caution as it may lead to invalid domain states.
    /// </remarks>
    public static T Unsafe<T>(bool repr)
        where T : DomainFactory<T, bool>, DomainType<T, bool> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the specified domain type <typeparamref name="T"/> from an unsafe representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{T, char}"/> 
    /// and <see cref="DomainType{T, char}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>An instance of the domain type <typeparamref name="T"/> created from the unsafe representation.</returns>
    /// <remarks>
    /// This method assumes that the provided representation is valid for the domain type.
    /// Use with caution as it bypasses standard validation mechanisms.
    /// </remarks>
    public static T Unsafe<T>(char repr)
        where T : DomainFactory<T, char>, DomainType<T, char> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the domain type <typeparamref name="T"/> from an unsafe string representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{T, string}"/> 
    /// and <see cref="DomainType{T, string}"/>.
    /// </typeparam>
    /// <param name="repr">The string representation used to create the domain type instance.</param>
    /// <returns>An instance of the domain type <typeparamref name="T"/> created from the provided string representation.</returns>
    /// <remarks>
    /// This method bypasses certain safety checks and directly creates the domain type instance. 
    /// Use with caution, as it assumes the provided representation is valid.
    /// </remarks>
    public static T Unsafe<T>(string repr)
        where T : DomainFactory<T, string>, DomainType<T, string> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the specified domain type <typeparamref name="T"/> from an unsafe byte representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{T, byte}"/> 
    /// and <see cref="DomainType{T, byte}"/>.
    /// </typeparam>
    /// <param name="repr">The byte representation to create the domain type from.</param>
    /// <returns>An instance of the domain type <typeparamref name="T"/> created from the provided byte representation.</returns>
    /// <remarks>
    /// This method bypasses certain safety checks and directly creates the domain type from the provided representation.
    /// Use with caution, as it assumes the representation is valid.
    /// </remarks>
    public static T Unsafe<T>(byte repr)
        where T : DomainFactory<T, byte>, DomainType<T, byte> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the specified domain type <typeparamref name="T"/> from an unsafe representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to create. Must implement <see cref="DomainFactory{T, short}"/> and <see cref="DomainType{T, short}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>An instance of the domain type <typeparamref name="T"/> created from the unsafe representation.</returns>
    /// <remarks>
    /// This method bypasses any validation or safety checks and should be used with caution.
    /// </remarks>
    public static T Unsafe<T>(short repr)
        where T : DomainFactory<T, short>, DomainType<T, short> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the specified domain type <typeparamref name="T"/> from an unsafe representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{SELF, IN}"/> and <see cref="DomainType{SELF, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The unsafe representation of the domain type.
    /// </param>
    /// <returns>
    /// An instance of the domain type <typeparamref name="T"/> created from the unsafe representation.
    /// </returns>
    /// <remarks>
    /// This method assumes that the provided representation is valid and does not perform any validation.
    /// Use with caution as it bypasses safety checks.
    /// </remarks>
    public static T Unsafe<T>(ushort repr)
        where T : DomainFactory<T, ushort>, DomainType<T, ushort> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the specified domain type <typeparamref name="T"/> from an unsafe representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to create. Must implement both <see cref="DomainFactory{T, int}"/> 
    /// and <see cref="DomainType{T, int}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe integer representation to create the domain type from.</param>
    /// <returns>An instance of the domain type <typeparamref name="T"/> created from the given representation.</returns>
    /// <remarks>
    /// This method assumes that the provided representation is valid for the domain type. 
    /// Use with caution as it bypasses validation.
    /// </remarks>
    public static T Unsafe<T>(int repr)
        where T : DomainFactory<T, int>, DomainType<T, int> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the specified domain type <typeparamref name="T"/> from an unsafe representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{T, uint}"/> and <see cref="DomainType{T, uint}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The unsafe representation of the domain type.
    /// </param>
    /// <returns>
    /// An instance of the domain type <typeparamref name="T"/> created from the provided unsafe representation.
    /// </returns>
    /// <remarks>
    /// This method assumes that the provided representation is valid and does not perform any validation.
    /// Use with caution, as invalid representations may lead to undefined behavior.
    /// </remarks>
    public static T Unsafe<T>(uint repr)
        where T : DomainFactory<T, uint>, DomainType<T, uint> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the specified domain type <typeparamref name="T"/> from an unsafe representation of type <see cref="long"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{T, long}"/> and <see cref="DomainType{T, long}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>An instance of the domain type <typeparamref name="T"/> created from the provided representation.</returns>
    /// <remarks>
    /// This method bypasses standard validation or safety checks and directly creates the domain type instance.
    /// Use with caution, as it assumes the provided representation is valid.
    /// </remarks>
    public static T Unsafe<T>(long repr)
        where T : DomainFactory<T, long>, DomainType<T, long> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the specified domain type <typeparamref name="T"/> from an unsafe representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{T, ulong}"/> 
    /// and <see cref="DomainType{T, ulong}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>An instance of the domain type <typeparamref name="T"/> created from the provided representation.</returns>
    /// <remarks>
    /// This method assumes that the provided representation is valid and does not perform any validation.
    /// Use with caution, as invalid representations may lead to undefined behavior.
    /// </remarks>
    public static T Unsafe<T>(ulong repr)
        where T : DomainFactory<T, ulong>, DomainType<T, ulong> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the specified domain type <typeparamref name="T"/> using an unsafe representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to create. Must implement both <see cref="DomainFactory{T, float}"/> 
    /// and <see cref="DomainType{T, float}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>An instance of the domain type <typeparamref name="T"/> created from the unsafe representation.</returns>
    /// <remarks>
    /// This method bypasses validation and directly creates the domain type from the provided representation.
    /// Use with caution as it may lead to invalid domain states.
    /// </remarks>
    public static T Unsafe<T>(float repr)
        where T : DomainFactory<T, float>, DomainType<T, float> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the domain type <typeparamref name="T"/> from an unsafe representation of type <see cref="double"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{T, double}"/> and <see cref="DomainType{T, double}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>An instance of the domain type <typeparamref name="T"/> created from the provided representation.</returns>
    /// <remarks>
    /// This method assumes that the provided representation is valid and does not perform any validation.
    /// Use with caution, as invalid representations may lead to undefined behavior.
    /// </remarks>
    public static T Unsafe<T>(double repr)
        where T : DomainFactory<T, double>, DomainType<T, double> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the specified domain type <typeparamref name="T"/> from an unsafe representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{T, decimal}"/> and <see cref="DomainType{T, decimal}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe decimal representation to create the domain type from.</param>
    /// <returns>An instance of the domain type <typeparamref name="T"/> created from the unsafe representation.</returns>
    /// <remarks>
    /// This method bypasses any safety checks or validations and directly creates the domain type from the provided representation.
    /// Use with caution as it may lead to invalid or inconsistent domain states.
    /// </remarks>
    public static T Unsafe<T>(decimal repr)
        where T : DomainFactory<T, decimal>, DomainType<T, decimal> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates a domain type instance from an unsafe representation of the specified type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the domain type to be created. Must implement both <see cref="DomainFactory{T, DateOnly}"/> 
    /// and <see cref="DomainType{T, DateOnly}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>
    /// An instance of the specified domain type <typeparamref name="T"/> created from the provided unsafe representation.
    /// </returns>
    /// <remarks>
    /// This method assumes that the provided representation is valid and does not perform any validation.
    /// Use with caution, as invalid representations may lead to undefined behavior.
    /// </remarks>
    public static T Unsafe<T>(DateOnly repr)
        where T : DomainFactory<T, DateOnly>, DomainType<T, DateOnly> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the specified domain type <typeparamref name="T"/> from an unsafe representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{T, TimeOnly}"/> 
    /// and <see cref="DomainType{T, TimeOnly}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>An instance of the domain type <typeparamref name="T"/> created from the unsafe representation.</returns>
    /// <remarks>
    /// This method bypasses any validation or safety checks and directly creates the domain type instance.
    /// Use with caution as it may lead to invalid or inconsistent states.
    /// </remarks>
    public static T Unsafe<T>(TimeOnly repr)
        where T : DomainFactory<T, TimeOnly>, DomainType<T, TimeOnly> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the domain type <typeparamref name="T"/> from an unsafe representation of type <see cref="DateTime"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type that implements both <see cref="DomainFactory{T, DateTime}"/> and <see cref="DomainType{T, DateTime}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type as a <see cref="DateTime"/>.</param>
    /// <returns>An instance of the domain type <typeparamref name="T"/> created from the provided representation.</returns>
    /// <remarks>
    /// This method bypasses any validation or safety checks, and directly creates the domain type from the provided representation.
    /// Use with caution, as it assumes the input is valid.
    /// </remarks>
    public static T Unsafe<T>(DateTime repr)
        where T : DomainFactory<T, DateTime>, DomainType<T, DateTime> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the specified domain type <typeparamref name="T"/> using an unsafe representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{T, DateTimeOffset}"/> 
    /// and <see cref="DomainType{T, DateTimeOffset}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>An instance of the domain type <typeparamref name="T"/> created from the unsafe representation.</returns>
    /// <remarks>
    /// This method bypasses validation and directly creates the domain type from the provided representation.
    /// Use with caution, as it may lead to invalid domain states.
    /// </remarks>
    public static T Unsafe<T>(DateTimeOffset repr)
        where T : DomainFactory<T, DateTimeOffset>, DomainType<T, DateTimeOffset> =>
        T.FromUnsafe(repr);

    /// <summary>
    /// Creates an instance of the domain type <typeparamref name="T"/> from an unsafe representation of type <see cref="TimeSpan"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement <see cref="DomainFactory{T, TimeSpan}"/> and <see cref="DomainType{T, TimeSpan}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The unsafe representation of the domain type, provided as a <see cref="TimeSpan"/>.
    /// </param>
    /// <returns>
    /// An instance of the domain type <typeparamref name="T"/> created from the provided unsafe representation.
    /// </returns>
    /// <remarks>
    /// This method assumes that the provided representation is valid and does not perform any validation.
    /// Use with caution, as invalid representations may lead to undefined behavior.
    /// </remarks>
    public static T Unsafe<T>(TimeSpan repr)
        where T : DomainFactory<T, TimeSpan>, DomainType<T, TimeSpan> =>
        T.FromUnsafe(repr);
    
    /// <summary>
    /// Creates a domain type instance from an unsafe representation of the specified type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the domain type to be created. Must implement both 
    /// <see cref="DomainFactory{T, Guid}"/> and <see cref="DomainType{T, Guid}"/>.
    /// </typeparam>
    /// <param name="repr">The unsafe representation of the domain type.</param>
    /// <returns>An instance of the domain type <typeparamref name="T"/> created from the unsafe representation.</returns>
    /// <remarks>
    /// This method assumes that the provided representation is valid and does not perform any validation.
    /// Use with caution as it bypasses safety checks.
    /// </remarks>
    public static T Unsafe<T>(Guid repr)
        where T : DomainFactory<T, Guid>, DomainType<T, Guid> =>
        T.FromUnsafe(repr);

}
