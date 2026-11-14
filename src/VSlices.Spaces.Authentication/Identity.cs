using LanguageExt;
using VSlices.Spaces;
using VSlices.Spaces.Traits;

namespace VSlices.Spaces.Definitions;

/// <summary>
/// Extensible semantic identity. Concrete identities own their construction and
/// equality; the base does not impose human accounts or a credential mechanism.
/// </summary>
public abstract record Identity : DiscreteSpace<Identity>;

/// <summary>
/// Explicit placeholder for a context in which no authenticated identity exists.
/// This carries no fabricated account, claims or credentials.
/// </summary>
public sealed record UnauthenticatedIdentity :
    Identity,
    DiscreteSpace<UnauthenticatedIdentity>,
    Transformable<UnauthenticatedIdentity.Input, UnauthenticatedIdentity>
{
    public readonly record struct Input;

    internal static UnauthenticatedIdentity Value { get; } = new();

    private UnauthenticatedIdentity() { }

    public static Fin<UnauthenticatedIdentity> Transform(Input input) => Value;
}
