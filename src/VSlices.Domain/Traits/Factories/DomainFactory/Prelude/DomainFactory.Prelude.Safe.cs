using System;
using LanguageExt;
using VSlices.Domain.Traits;

namespace VSlices;

public static partial class VSlicesDomainPrelude
{
    /// <summary>
    /// Creates a new instance of the specified domain type <typeparamref name="T"/> 
    /// using the provided representation value.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to create. Must implement both <see cref="DomainFactory{SELF, IN}"/> 
    /// and <see cref="DomainType{SELF, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The representation value used to create the domain type instance.
    /// </param>
    /// <returns>
    /// A <see cref="Fin{T}"/> containing the created domain type instance.
    /// </returns>
    public static Fin<T> New<T>(bool repr)
        where T : DomainFactory<T, bool>, DomainType<T, bool> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of the specified domain type <typeparamref name="T"/> 
    /// using the provided representation of type <see cref="char"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to create. Must implement both <see cref="DomainFactory{SELF, IN}"/> 
    /// and <see cref="DomainType{SELF, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">The representation of the domain type as a <see cref="char"/>.</param>
    /// <returns>A finalized instance of the domain type <typeparamref name="T"/>.</returns>
    public static Fin<T> New<T>(char repr)
        where T : DomainFactory<T, char>, DomainType<T, char> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of the specified domain type using the provided string representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{T, string}"/> 
    /// and <see cref="DomainType{T, string}"/>.
    /// </typeparam>
    /// <param name="repr">The string representation used to create the domain type instance.</param>
    /// <returns>
    /// A <see cref="Fin{T}"/> containing the newly created domain type instance.
    /// </returns>
    public static Fin<T> New<T>(string repr)
        where T : DomainFactory<T, string>, DomainType<T, string> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of the specified domain type using the provided byte representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{T, byte}"/> 
    /// and <see cref="DomainType{T, byte}"/>.
    /// </typeparam>
    /// <param name="repr">The byte representation used to create the domain type instance.</param>
    /// <returns>
    /// A <see cref="Fin{T}"/> containing the newly created domain type instance.
    /// </returns>
    public static Fin<T> New<T>(byte repr)
        where T : DomainFactory<T, byte>, DomainType<T, byte> =>
        T.From(repr);
    
    /// <summary>
    /// Creates a new instance of the domain type <typeparamref name="T"/> from the given representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{SELF, IN}"/> and <see cref="DomainType{SELF, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The representation of the domain type, of type <see cref="short"/>.
    /// </param>
    /// <returns>
    /// A <see cref="Fin{T}"/> instance representing the created domain type.
    /// </returns>
    public static Fin<T> New<T>(short repr)
        where T : DomainFactory<T, short>, DomainType<T, short> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of the domain type <typeparamref name="T"/> using the provided representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{SELF, IN}"/> 
    /// and <see cref="DomainType{SELF, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The representation value used to create the domain type instance.
    /// </param>
    /// <returns>
    /// A <see cref="Fin{T}"/> containing the created domain type instance.
    /// </returns>
    public static Fin<T> New<T>(ushort repr)
        where T : DomainFactory<T, ushort>, DomainType<T, ushort> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of the specified domain type <typeparamref name="T"/> 
    /// using the provided integer representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to create. Must implement both <see cref="DomainFactory{SELF, IN}"/> 
    /// and <see cref="DomainType{SELF, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">The integer representation used to create the domain type instance.</param>
    /// <returns>
    /// A <see cref="Fin{T}"/> containing the created domain type instance.
    /// </returns>
    public static Fin<T> New<T>(int repr)
        where T : DomainFactory<T, int>, DomainType<T, int> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of the domain type <typeparamref name="T"/> using the specified representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to create. Must implement both <see cref="DomainFactory{SELF, IN}"/> and <see cref="DomainType{SELF, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The representation value used to create the domain type instance.
    /// </param>
    /// <returns>
    /// A <see cref="Fin{T}"/> containing the created domain type instance.
    /// </returns>
    public static Fin<T> New<T>(uint repr)
        where T : DomainFactory<T, uint>, DomainType<T, uint> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of the domain type <typeparamref name="T"/> from the specified representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{SELF, IN}"/> 
    /// and <see cref="DomainType{SELF, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">The representation value used to create the domain type instance.</param>
    /// <returns>
    /// A <see cref="Fin{T}"/> instance representing the result of the creation process.
    /// </returns>
    public static Fin<T> New<T>(long repr)
        where T : DomainFactory<T, long>, DomainType<T, long> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of the domain type <typeparamref name="T"/> from the provided representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{SELF, IN}"/> and <see cref="DomainType{SELF, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">The representation value used to create the domain type instance.</param>
    /// <returns>A <see cref="Fin{T}"/> containing the created domain type instance.</returns>
    public static Fin<T> New<T>(ulong repr)
        where T : DomainFactory<T, ulong>, DomainType<T, ulong> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of the domain type <typeparamref name="T"/> from the specified representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement <see cref="DomainFactory{SELF, IN}"/> and <see cref="DomainType{SELF, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The representation value used to create the domain type instance.
    /// </param>
    /// <returns>
    /// A finalized instance of the domain type <typeparamref name="T"/>.
    /// </returns>
    public static Fin<T> New<T>(float repr)
        where T : DomainFactory<T, float>, DomainType<T, float> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of the specified domain type <typeparamref name="T"/> 
    /// using the provided <paramref name="repr"/> representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both 
    /// <see cref="DomainFactory{SELF, IN}"/> and <see cref="DomainType{SELF, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The representation value used to create the domain type instance.
    /// </param>
    /// <returns>
    /// A <see cref="Fin{T}"/> containing the newly created instance of type <typeparamref name="T"/>.
    /// </returns>
    public static Fin<T> New<T>(double repr)
        where T : DomainFactory<T, double>, DomainType<T, double> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of the specified domain type <typeparamref name="T"/> using the provided decimal representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{T, decimal}"/> 
    /// and <see cref="DomainType{T, decimal}"/>.
    /// </typeparam>
    /// <param name="repr">The decimal representation used to create the domain type instance.</param>
    /// <returns>A finalized instance of the domain type <typeparamref name="T"/>.</returns>
    public static Fin<T> New<T>(decimal repr)
        where T : DomainFactory<T, decimal>, DomainType<T, decimal> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of the domain type <typeparamref name="T"/> from the specified representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{SELF, IN}"/> 
    /// and <see cref="DomainType{SELF, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">The representation used to create the domain type instance.</param>
    /// <returns>A finalized instance of the domain type <typeparamref name="T"/>.</returns>
    public static Fin<T> New<T>(DateOnly repr)
        where T : DomainFactory<T, DateOnly>, DomainType<T, DateOnly> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of the domain type <typeparamref name="T"/> using the provided representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement <see cref="DomainFactory{SELF, IN}"/> and <see cref="DomainType{SELF, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">
    /// The representation value used to create the domain type instance.
    /// </param>
    /// <returns>
    /// A finalized instance of the domain type <typeparamref name="T"/>.
    /// </returns>
    public static Fin<T> New<T>(TimeOnly repr)
        where T : DomainFactory<T, TimeOnly>, DomainType<T, TimeOnly> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of the domain type <typeparamref name="T"/> using the provided <see cref="DateTime"/> representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{SELF, IN}"/> and <see cref="DomainType{SELF, REPR}"/>.
    /// </typeparam>
    /// <param name="repr">The <see cref="DateTime"/> representation used to create the domain type instance.</param>
    /// <returns>A <see cref="Fin{T}"/> instance representing the created domain type.</returns>
    public static Fin<T> New<T>(DateTime repr)
        where T : DomainFactory<T, DateTime>, DomainType<T, DateTime> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of the domain type <typeparamref name="T"/> from the specified representation.
    /// </summary>
    /// <typeparam name="T">
    /// The domain type to be created. Must implement both <see cref="DomainFactory{SELF, IN}"/> 
    /// and <see cref="DomainType{SELF, REPR}"/> interfaces.
    /// </typeparam>
    /// <param name="repr">The representation used to create the domain type instance.</param>
    /// <returns>A <see cref="Fin{T}"/> containing the created domain type instance.</returns>
    public static Fin<T> New<T>(DateTimeOffset repr)
        where T : DomainFactory<T, DateTimeOffset>, DomainType<T, DateTimeOffset> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the provided <see cref="TimeSpan"/> representation.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the domain object to create. Must implement both <see cref="DomainFactory{T, TimeSpan}"/> 
    /// and <see cref="DomainType{T, TimeSpan}"/>.
    /// </typeparam>
    /// <param name="repr">The <see cref="TimeSpan"/> representation used to create the domain object.</param>
    /// <returns>A <see cref="Fin{T}"/> representing the result of the creation operation.</returns>
    public static Fin<T> New<T>(TimeSpan repr)
        where T : DomainFactory<T, TimeSpan>, DomainType<T, TimeSpan> =>
        T.From(repr);

    /// <summary>
    /// Creates a new instance of a domain type using the provided <see cref="Guid"/> representation.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the domain object to create. Must implement both <see cref="DomainFactory{T, Guid}"/> 
    /// and <see cref="DomainType{T, Guid}"/>.
    /// </typeparam>
    /// <param name="repr">The <see cref="Guid"/> representation used to create the domain object.</param>
    /// <returns>A <see cref="Fin{T}"/> representing the result of the creation operation.</returns>
    public static Fin<T> New<T>(Guid repr)
        where T : DomainFactory<T, Guid>, DomainType<T, Guid> =>
        T.From(repr);
}
