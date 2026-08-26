namespace Shared.Domain.Products;

public abstract class Profile : DomainType<Profile, Profile.Repr>
{
    public readonly record struct Repr(string Name);

    public abstract string Name { get; }

    public Repr To() =>
        new(Name);

    public override string ToString() =>
        Name;
}
