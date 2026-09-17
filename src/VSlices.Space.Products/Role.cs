using LanguageExt;
using VSlices.Services;

namespace VSlices.Products;

public abstract class ProductRole
{
    public sealed record Repr(
        string Name,
        Seq<ServiceClaim.Repr> Claims);

    public abstract string Name { get; }

    public abstract Seq<ServiceClaim> Claims { get; }

    public Repr To() =>
        new(Name, Claims.Map(c => c.To()));

    public override string ToString() =>
        Name;
}
