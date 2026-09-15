using System.Numerics;
using LanguageExt;
using VSlices.Space.Quantities.Abstract;

namespace VSlices.Space.Quantities;

/// <summary>
/// A coordinate for temperature points. ReferenceScale describes the associated
/// temperature-difference unit, while ReferenceOffset places the coordinate origin
/// relative to Kelvin:
///
///   kelvin = value * ReferenceScale + ReferenceOffset
///
/// Temperature differences use only ReferenceScale; point temperatures use both.
/// </summary>
public interface TemperatureCoordinate : Coordinate<M.Temperature>
{
    static abstract decimal ReferenceOffset { get; }
}

public readonly struct Kelvin : TemperatureCoordinate
{
    public static decimal ReferenceScale => 1m;
    public static decimal ReferenceOffset => 0m;
}

public readonly struct Celsius : TemperatureCoordinate
{
    public static decimal ReferenceScale => 1m;
    public static decimal ReferenceOffset => 273.15m;
}

public readonly struct Fahrenheit : TemperatureCoordinate
{
    public static decimal ReferenceScale => 5m / 9m;
    public static decimal ReferenceOffset => 459.67m * 5m / 9m;
}

/// <summary>
/// A temperature displacement. Unlike an absolute Temperature point, a difference
/// has no affine offset and therefore forms a vector space.
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
/// A physically admissible absolute temperature expressed in one affine coordinate.
///
/// Temperature intentionally does not implement AffineSpace: the absolute-zero lower
/// bound means arbitrary translation by TemperatureDifference is not closed. Subtracting
/// two valid temperatures is total; translating a temperature is a partial operation
/// that may fail when it would cross absolute zero.
/// </summary>
public sealed record Temperature<C, T> : DiscreteSpace<Temperature<C, T>>
    where C : TemperatureCoordinate
    where T : IFloatingPoint<T>
{
    private Temperature(T value) => Value = value;

    public T Value { get; }

    public static Fin<Temperature<C, T>> Create(T value)
    {
        var reference = ToReference(value);

        return reference >= T.Zero
            ? new Temperature<C, T>(value)
            : Error.New($"Temperature cannot be below absolute zero. Sent: {value} {typeof(C).Name}.");
    }

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

    public Fin<Temperature<C, T>> Translate<DELTA_C>(TemperatureDifference<DELTA_C, T> displacement)
        where DELTA_C : TemperatureCoordinate
    {
        var deltaReference = displacement.Value * T.CreateChecked(DELTA_C.ReferenceScale);
        return FromReferenceChecked(ReferenceValue + deltaReference);
    }

    public static TemperatureDifference<C, T> operator -(
        Temperature<C, T> left,
        Temperature<C, T> right) =>
        left.Difference(right);

    internal T ReferenceValue => ToReference(Value);

    internal static Temperature<C, T> FromReference(T reference)
    {
        var scale = T.CreateChecked(C.ReferenceScale);
        var offset = T.CreateChecked(C.ReferenceOffset);
        return new((reference - offset) / scale);
    }

    private static Fin<Temperature<C, T>> FromReferenceChecked(T reference) =>
        reference >= T.Zero
            ? FromReference(reference)
            : Error.New("Temperature translation would cross absolute zero.");

    private static T ToReference(T value)
    {
        var scale = T.CreateChecked(C.ReferenceScale);
        var offset = T.CreateChecked(C.ReferenceOffset);
        return value * scale + offset;
    }
}
