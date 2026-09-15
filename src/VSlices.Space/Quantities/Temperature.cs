using System.Numerics;
using LanguageExt;
using VSlices.Space.Quantities.Abstract;

namespace VSlices.Space.Quantities;

/// <summary>
/// A coordinate for temperature points.
///
/// ReferenceScale describes the associated temperature-difference unit relative to
/// Kelvin, while AbsoluteZero identifies the coordinate value of the physical
/// absolute-zero point. Point conversion is therefore:
///
///   kelvin = (value - AbsoluteZero) * ReferenceScale
///
/// Temperature differences use only ReferenceScale; point temperatures use both.
/// </summary>
public interface TemperatureCoordinate : Coordinate<M.Temperature>
{
    static abstract decimal AbsoluteZero { get; }
}

public readonly struct Kelvin : TemperatureCoordinate
{
    public static decimal ReferenceScale => 1m;
    public static decimal AbsoluteZero => 0m;
}

public readonly struct Celsius : TemperatureCoordinate
{
    public static decimal ReferenceScale => 1m;
    public static decimal AbsoluteZero => -273.15m;
}

public readonly struct Fahrenheit : TemperatureCoordinate
{
    public static decimal ReferenceScale => 5m / 9m;
    public static decimal AbsoluteZero => -459.67m;
}

/// <summary>
/// A temperature displacement. Unlike an absolute Temperature point, a difference
/// has no affine origin and therefore forms a vector space.
/// </summary>
public sealed record TemperatureDifference<C, T>(T Value) :
    Q<M.Temperature, C, T>,
    VectorSpace<TemperatureDifference<C, T>, T>
    where C : TemperatureCoordinate
    where T : IFloatingPoint<T>
{
    public TemperatureDifference<TO, T> Convert<TO>()
        where TO : TemperatureCoordinate
    {
        var reference = Value * T.CreateChecked(C.ReferenceScale);
        var targetScale = T.CreateChecked(TO.ReferenceScale);
        return new(reference / targetScale);
    }

    public static TemperatureDifference<C, T> operator +(
        TemperatureDifference<C, T> left,
        TemperatureDifference<C, T> right) =>
        new(left.Value + right.Value);

    public static TemperatureDifference<C, T> operator -(
        TemperatureDifference<C, T> left,
        TemperatureDifference<C, T> right) =>
        new(left.Value - right.Value);

    public static TemperatureDifference<C, T> operator -(
        TemperatureDifference<C, T> value) =>
        new(-value.Value);

    public static TemperatureDifference<C, T> operator *(
        TemperatureDifference<C, T> value,
        T scalar) =>
        new(value.Value * scalar);

    public static TemperatureDifference<C, T> operator /(
        TemperatureDifference<C, T> value,
        T scalar) =>
        new(value.Value / scalar);

    public static TemperatureDifference<C, T> AdditiveIdentity =>
        new(T.Zero);
}

/// <summary>
/// A mathematical temperature point expressed in one affine coordinate.
///
/// The affine line is intentionally total: values below physical absolute zero remain
/// representable because the geometry itself is not responsible for physical admissibility.
/// Physical admissibility is introduced separately by PhysicalTemperature.
/// </summary>
public sealed record Temperature<C, T>(T Value) :
    Q<M.Temperature, C, T>,
    AffineSpace<Temperature<C, T>, TemperatureDifference<C, T>, T>
    where C : TemperatureCoordinate
    where T : IFloatingPoint<T>
{
    public Temperature<TO, T> Convert<TO>()
        where TO : TemperatureCoordinate =>
        Temperature<TO, T>.FromReference(ReferenceValue);

    public TemperatureDifference<C, T> Difference<RIGHT_C>(Temperature<RIGHT_C, T> right)
        where RIGHT_C : TemperatureCoordinate
    {
        var differenceInReference = ReferenceValue - right.ReferenceValue;
        var scale = T.CreateChecked(C.ReferenceScale);
        return new(differenceInReference / scale);
    }

    public Temperature<C, T> Translate<DELTA_C>(TemperatureDifference<DELTA_C, T> displacement)
        where DELTA_C : TemperatureCoordinate
    {
        var displacementInReference = displacement.Value * T.CreateChecked(DELTA_C.ReferenceScale);
        var displacementInThisCoordinate = displacementInReference / T.CreateChecked(C.ReferenceScale);
        return new(Value + displacementInThisCoordinate);
    }

    public static Temperature<C, T> operator +(
        Temperature<C, T> point,
        TemperatureDifference<C, T> displacement) =>
        point.Translate(displacement);

    public static Temperature<C, T> operator -(
        Temperature<C, T> point,
        TemperatureDifference<C, T> displacement) =>
        point.Translate(-displacement);

    public static TemperatureDifference<C, T> operator -(
        Temperature<C, T> left,
        Temperature<C, T> right) =>
        left.Difference(right);

    internal T ReferenceValue => ToReference(Value);

    internal static Temperature<C, T> FromReference(T reference)
    {
        var scale = T.CreateChecked(C.ReferenceScale);
        var absoluteZero = T.CreateChecked(C.AbsoluteZero);
        return new(reference / scale + absoluteZero);
    }

    private static T ToReference(T value)
    {
        var scale = T.CreateChecked(C.ReferenceScale);
        var absoluteZero = T.CreateChecked(C.AbsoluteZero);
        return (value - absoluteZero) * scale;
    }
}

/// <summary>
/// A temperature point established as physically admissible under the current
/// absolute-zero model.
///
/// This is a semantic subset of the complete mathematical Temperature affine line.
/// Widening to Temperature is total; narrowing from Temperature is a fallible
/// transformation because values below absolute zero do not inhabit this space.
/// </summary>
public sealed record PhysicalTemperature<C, T> :
    DerivedSpace<PhysicalTemperature<C, T>, Temperature<C, T>>
    where C : TemperatureCoordinate
    where T : IFloatingPoint<T>
{
    private PhysicalTemperature(Temperature<C, T> temperature) =>
        Temperature = temperature;

    public Temperature<C, T> Temperature { get; }

    public T Value => Temperature.Value;

    public Temperature<C, T> ToBase() => Temperature;

    public static Fin<PhysicalTemperature<C, T>> Create(Temperature<C, T> temperature)
    {
        var absoluteZero = T.CreateChecked(C.AbsoluteZero);

        return temperature.Value >= absoluteZero
            ? new PhysicalTemperature<C, T>(temperature)
            : Error.New($"Physical temperature cannot be below absolute zero. Sent: {temperature.Value} {typeof(C).Name}.");
    }

    public static Fin<PhysicalTemperature<C, T>> Create(T value) =>
        Create(new Temperature<C, T>(value));

    public PhysicalTemperature<TO, T> Convert<TO>()
        where TO : TemperatureCoordinate =>
        new(Temperature.Convert<TO>());

    public Fin<PhysicalTemperature<C, T>> Translate<DELTA_C>(TemperatureDifference<DELTA_C, T> displacement)
        where DELTA_C : TemperatureCoordinate =>
        Create(Temperature.Translate(displacement));
}
