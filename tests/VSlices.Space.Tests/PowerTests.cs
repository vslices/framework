using VSlices.Space.Quantities;
using VSlices.Space.Quantities.Abstract;
using static VSlices.Space.Conversions;
using static VSlices.Space.Quantities.PowerOperations;

namespace VSlices.Space.Tests;

public class PowerTests
{
    [Fact]
    public void square_materializes_length_power_over_the_same_primitive_basis()
    {
        var side = new ProbeLength<Kilometers, decimal>(3m);
        Power<M.Length, N2, Kilometers, decimal> structural = square(side);
        Assert.Equal(9m, structural.Value);
        Assert.IsAssignableFrom<Q<M.Pow<M.Length, N2>, Kilometers, decimal>>(structural);
    }

    [Fact]
    public void cube_materializes_length_power_over_the_same_primitive_basis()
    {
        var side = new ProbeLength<Meters, decimal>(3m);
        Power<M.Length, N3, Meters, decimal> structural = cube(side);
        Assert.Equal(27m, structural.Value);
        Assert.IsAssignableFrom<Q<M.Pow<M.Length, N3>, Meters, decimal>>(structural);
    }

    [Fact]
    public void same_factor_product_implicitly_reduces_to_square_power()
    {
        var side = new ProbeLength<Kilometers, decimal>(3m);
        Product<M.Length, Kilometers, decimal> product = side * side;
        Power<M.Length, N2, Kilometers, decimal> power = product;
        Assert.Equal(product.Value, power.Value);
    }

    [Fact]
    public void square_power_can_be_explicitly_expanded_back_to_product()
    {
        var side = new ProbeLength<Meters, decimal>(4m);
        var power = square(side);
        var expanded = (Product<M.Length, Meters, decimal>)power;
        Power<M.Length, N2, Meters, decimal> reducedAgain = expanded;
        Assert.Equal(power.Value, expanded.Value);
        Assert.Equal(power, reducedAgain);
    }

    [Fact]
    public void area_accepts_direct_power_and_product_reduced_to_power()
    {
        var width = new ProbeLength<Meters, decimal>(3m);
        var height = new ProbeLength<Meters, decimal>(4m);
        Area<Meters, decimal> squareArea = area(square(width));
        Area<Meters, decimal> productArea = area(width * height);
        Assert.Equal(9m, squareArea.Value);
        Assert.Equal(12m, productArea.Value);
    }

    [Fact]
    public void product_and_power_keep_distinct_structural_histories_before_reduction()
    {
        var side = new ProbeLength<Kilometers, decimal>(3m);
        var powered = square(side);
        var multiplied = side * side;
        Assert.Equal(multiplied.Value, powered.Value);
        Assert.IsType<Power<M.Length, N2, Kilometers, decimal>>(powered);
        Assert.IsType<Product<M.Length, Kilometers, decimal>>(multiplied);
    }

    [Fact]
    public void volume_accepts_direct_cube_and_area_times_length()
    {
        var side = new ProbeLength<Meters, decimal>(3m);
        var direct = volume(cube(side));
        var composed = volume(area(square(side)) * side);
        Assert.Equal(27m, direct.Value);
        Assert.Equal(direct.Value, composed.Value);
    }

    private sealed class ProbeLength<C, T> : Length<ProbeLength<C, T>, C, T>
        where C : Coordinate<M.Length>
        where T : System.Numerics.INumber<T>
    {
        public ProbeLength(T value) : base(value) { }
    }
}
