using LanguageExt;
using VSlices.Domain.Traits;
using VSlices.Services;

namespace VSlices.Products;

public abstract class AppRole : DomainType<AppRole, AppRole.Repr>
{
    public sealed record Repr(
        string Name,
        Seq<AppClaim.Repr> Claims);

    public abstract string Name { get; }

    public abstract Seq<AppClaim> Claims { get; }

    public Repr To() =>
        new(Name, Claims.Map(c => c.To()));

    public override string ToString() =>
        Name;
}
