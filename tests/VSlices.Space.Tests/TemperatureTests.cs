using LanguageExt;
using VSlices.Space.Quantities;
using VSlices.Space.Quantities.Abstract;

namespace VSlices.Space.Tests;

public class TemperatureTests
{
    [Fact]
    public void temperature_coordinates_are_affine_while_differences_use_only_scale()
    {
        Assert.Equal(1m, Kelvin.ReferenceScale);
        Assert.Equal(0m, Kelvin.ReferenceOffset);
        Assert.Equal(1m, Celsius.ReferenceScale);
        Assert.Equal(273.15m, Celsius.ReferenceOffset);
        Assert.Equal(5m / 9m, Fahrenheit.ReferenceScale);
    }

    [Fact]
    public void absolute_zero_is_the_same_physical_point_across_supported_coordinates()
    {
        var kelvin = Success(Temperature<Kelvin, decimal>.Create(0m));
        var celsius = Success(Temperature<Celsius, decimal>.Create(-273.15m));
        var fahrenheit = Success(Temperature<Fahrenheit, decimal>.Create(-459.67m));

        Assert.Equal(0m, kelvin.Value);
        Assert.Equal(0m, celsius.Convert<Kelvin>().Value);
        Assert.Equal(0m, fahrenheit.Convert<Kelvin>().Value);
    }

    [Fact]
    public void values_below_absolute_zero_are_rejected_by_space_establishment()
    {
        var result = Temperature<Celsius, decimal>.Create(-273.16m);

        Assert.True(result.Match(Succ: _ => false, Fail: _ => true));
    }

    [Fact]
    public void point_conversion_uses_scale_and_offset()
    {
        var freezing = Success(Temperature<Celsius, decimal>.Create(0m));
        var fahrenheit = freezing.Convert<Fahrenheit>();

        Assert.InRange(fahrenheit.Value, 31.999999999999999999999999m, 32.000000000000000000000001m);
    }

    [Fact]
    public void subtracting_temperatures_yields_a_temperature_difference()
    {
        var boiling = Success(Temperature<Celsius, decimal>.Create(100m));
        var freezing = Success(Temperature<Fahrenheit, decimal>.Create(32m));

        TemperatureDifference<Celsius, decimal> difference = boiling.Difference(freezing);
        var inFahrenheitDegrees = difference.Convert<Fahrenheit>();

        Assert.InRange(difference.Value, 99.999999999999999999999999m, 100.000000000000000000000001m);
        Assert.InRange(inFahrenheitDegrees.Value, 179.99999999999999999999999m, 180.00000000000000000000001m);
    }

    [Fact]
    public void temperature_difference_is_a_vector_space_quantity()
    {
        var first = new TemperatureDifference<Celsius, decimal>(10m);
        var second = new TemperatureDifference<Celsius, decimal>(5m);

        var result = (first + second) * 2m - new TemperatureDifference<Celsius, decimal>(4m);

        Assert.Equal(26m, result.Value);
        Assert.IsAssignableFrom<Q<M.Temperature, Celsius, decimal>>(result);
        Assert.Contains(
            result.GetType().GetInterfaces(),
            candidate => candidate.IsGenericType &&
                         candidate.GetGenericTypeDefinition() == typeof(VectorSpace<,>));
    }

    [Fact]
    public void translating_a_temperature_is_partial_because_absolute_zero_bounds_the_space()
    {
        var freezing = Success(Temperature<Celsius, decimal>.Create(0m));

        var valid = freezing.Translate(new TemperatureDifference<Fahrenheit, decimal>(18m));
        var invalid = freezing.Translate(new TemperatureDifference<Celsius, decimal>(-300m));

        var translated = Success(valid);

        Assert.InRange(translated.Value, 9.999999999999999999999999m, 10.000000000000000000000001m);
        Assert.True(invalid.Match(Succ: _ => false, Fail: _ => true));
    }

    [Fact]
    public void physical_temperature_is_not_claimed_as_an_affine_space()
    {
        var type = typeof(Temperature<Celsius, decimal>);

        Assert.DoesNotContain(
            type.GetInterfaces(),
            candidate => candidate.IsGenericType &&
                         candidate.GetGenericTypeDefinition() == typeof(AffineSpace<,,>));
    }

    private static A Success<A>(Fin<A> value) =>
        value.Match(
            Succ: result => result,
            Fail: error => throw new InvalidOperationException(error.ToString()));
}
