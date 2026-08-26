using LanguageExt;
using VSlices.Domain.Traits;
using VSlices.Services;

namespace VSlices.Products;

public abstract class vRole : DomainType<vRole, vRole.Repr>
{
    public sealed record Repr(
        string Name,
        Seq<vClaim.Repr> Claims);

    public abstract string Name { get; }

    public abstract Seq<vClaim> Claims { get; }

    public Repr To() =>
        new(Name, Claims.Map(c => c.To()));

    public override string ToString() =>
        Name;
}
