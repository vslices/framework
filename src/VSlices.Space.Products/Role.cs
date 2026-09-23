using LanguageExt;
using VSlices.Services;
using VSlices.Space;

namespace VSlices.Products;

public abstract class ProductRole : DiscreteSpace<ProductRole>
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

    public bool Equals(ProductRole? other) =>
        other is not null &&
        Name.Equals(
            other.Name,
            StringComparison.Ordinal);

    public override bool Equals(object? obj) =>
        obj is ProductRole other && Equals(other);

    public override int GetHashCode() =>
        StringComparer.Ordinal.GetHashCode(Name);

    public static bool operator ==(ProductRole? left, ProductRole? right) =>
        EqualityComparer<ProductRole>.Default.Equals(left, right);

    public static bool operator !=(ProductRole? left, ProductRole? right) =>
        !(left == right);
}
