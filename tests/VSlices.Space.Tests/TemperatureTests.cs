using LanguageExt;
using VSlices.Space.Quantities;
using VSlices.Space.Quantities.Abstract;

namespace VSlices.Space.Tests;

public class TemperatureTests
{
    [Fact]
    public void temperature_coordinates_share_absolute_zero_but_have_distinct_scales()
    {
        Assert.Equal(1m, Kelvin.ReferenceScale);
        Assert.Equal(0m, Kelvin.AbsoluteZero);
        Assert.Equal(1m, Celsius.ReferenceScale);
        Assert.Equal(-273.15m, Celsius.AbsoluteZero);
        Assert.Equal(5m / 9m, Fahrenheit.ReferenceScale);
        Assert.Equal(-459.67m, Fahrenheit.AbsoluteZero);
    }

    [Fact]
    public void absolute_zero_is_the_same_point_across_supported_coordinates()
    {
        var kelvin = new Temperature<Kelvin, decimal>(0m);
        var celsius = new Temperature<Celsius, decimal>(-273.15m);
        var fahrenheit = new Temperature<Fahrenheit, decimal>(-459.67m);

        Assert.Equal(0m, kelvin.Value);
        Assert.Equal(0m, celsius.Convert<Kelvin>().Value);
        Assert.Equal(0m, fahrenheit.Convert<Kelvin>().Value);
    }

    [Fact]
    public void mathematical_temperature_may_exist_below_physical_absolute_zero()
    {
        var belowAbsoluteZero = new Temperature<Kelvin, decimal>(-1m);
        var translated = belowAbsoluteZero + new TemperatureDifference<Kelvin, decimal>(2m);

        Assert.Equal(-1m, belowAbsoluteZero.Value);
        Assert.Equal(1m, translated.Value);
        Assert.IsAssignableFrom<Q<M.Temperature, Kelvin, decimal>>(belowAbsoluteZero);
        Assert.Contains(
            belowAbsoluteZero.GetType().GetInterfaces(),
            candidate => candidate.IsGenericType &&
                         candidate.GetGenericTypeDefinition() == typeof(AffineSpace<,,>));
    }

    [Fact]
    public void physical_temperature_establishment_rejects_values_below_absolute_zero()
    {
        var mathematical = new Temperature<Celsius, decimal>(-273.16m);
        var result = PhysicalTemperature<Celsius, decimal>.Create(mathematical);

        Assert.True(result.Match(Succ: _ => false, Fail: _ => true));
    }

    [Fact]
    public void point_conversion_uses_scale_and_absolute_zero_origin()
    {
        var freezing = new Temperature<Celsius, decimal>(0m);
        var fahrenheit = freezing.Convert<Fahrenheit>();

        Assert.InRange(fahrenheit.Value, 31.999999999999999999999999m, 32.000000000000000000000001m);
    }

    [Fact]
    public void subtracting_temperatures_yields_a_temperature_difference()
    {
        var boiling = new Temperature<Celsius, decimal>(100m);
        var freezing = new Temperature<Fahrenheit, decimal>(32m);

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
    public void temperature_affine_translation_is_total()
    {
        var zeroKelvin = new Temperature<Kelvin, decimal>(0m);
        var belowPhysicalZero = zeroKelvin + new TemperatureDifference<Kelvin, decimal>(-1m);
        var recovered = belowPhysicalZero - new TemperatureDifference<Kelvin, decimal>(-1m);

        Assert.Equal(-1m, belowPhysicalZero.Value);
        Assert.Equal(0m, recovered.Value);
        Assert.Equal(
            new TemperatureDifference<Kelvin, decimal>(-1m),
            belowPhysicalZero - zeroKelvin);
    }

    [Fact]
    public void physical_temperature_is_a_derived_space_of_mathematical_temperature()
    {
        var mathematical = new Temperature<Celsius, decimal>(20m);
        var physical = Success(PhysicalTemperature<Celsius, decimal>.Create(mathematical));

        Assert.Equal(mathematical, physical.ToBase());
        Assert.IsAssignableFrom<DerivedSpace<PhysicalTemperature<Celsius, decimal>, Temperature<Celsius, decimal>>>(physical);
    }

    [Fact]
    public void physical_temperature_translation_reestablishes_the_derived_space()
    {
        var freezing = Success(PhysicalTemperature<Celsius, decimal>.Create(0m));

        var valid = freezing.Translate(new TemperatureDifference<Fahrenheit, decimal>(18m));
        var invalid = freezing.Translate(new TemperatureDifference<Celsius, decimal>(-300m));

        var translated = Success(valid);

        Assert.InRange(translated.Value, 9.999999999999999999999999m, 10.000000000000000000000001m);
        Assert.True(invalid.Match(Succ: _ => false, Fail: _ => true));
    }

    [Fact]
    public void converting_a_physical_temperature_preserves_physical_establishment()
    {
        var celsius = Success(PhysicalTemperature<Celsius, decimal>.Create(100m));
        var fahrenheit = celsius.Convert<Fahrenheit>();

        Assert.InRange(fahrenheit.Value, 211.99999999999999999999999m, 212.00000000000000000000001m);
    }

    private static A Success<A>(Fin<A> value) =>
        value.Match(
            Succ: result => result,
            Fail: error => throw new InvalidOperationException(error.ToString()));
}
