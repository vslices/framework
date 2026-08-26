namespace Shared.Domain.Products;

public abstract class Product : DomainType<Product, Product.Repr>
{
    public readonly record struct Repr(string Name);

    public abstract string Name { get; }

    public Repr To() =>
        new(Name);

    public override string ToString() =>
        Name;
}
