using System.Numerics;

namespace VSlices.Literals.Abstracts;

/// <summary>
/// Represents a strongly-typed constant value composed of whole numbers and fractional parts, 
/// including tenths for a specified numeric type.
/// </summary>
/// <typeparam name="WholeType">
/// The type of the whole number part, constrained to implement <see cref="System.Numerics.INumber{T}"/>.
/// </typeparam>
/// <typeparam name="Whole">
/// A constant representing the whole number part of the value.
/// </typeparam>
/// <typeparam name="Tenths">
/// A constant representing the tenths part of the value.
/// </typeparam>
/// <typeparam name="T">
/// The resulting numeric type, constrained to implement <see cref="System.Numerics.IFloatingPoint{T}"/>.
/// </typeparam>
public class P<WholeType, Whole, Tenths, T> : P<WholeType, Whole, Tenths, N0, T>
    where WholeType : INumber<WholeType>
    where Whole : Const<WholeType>
    where Tenths : Const<int>
    where T : IFloatingPoint<T>;

/// <summary>
/// Represents a strongly-typed constant value composed of whole numbers and fractional parts, 
/// including tenths and hundredths for a specified numeric type.
/// </summary>
/// <typeparam name="WholeType">
/// The type of the whole number part, constrained to implement <see cref="System.Numerics.INumber{T}"/>.
/// </typeparam>
/// <typeparam name="Whole">
/// A constant representing the whole number part of the value.
/// </typeparam>
/// <typeparam name="Tenths">
/// A constant representing the tenths part of the value.
/// </typeparam>
/// <typeparam name="Hundredths">
/// A constant representing the hundredths part of the value.
/// </typeparam>
/// <typeparam name="T">
/// The resulting numeric type, constrained to implement <see cref="System.Numerics.IFloatingPoint{T}"/>.
/// </typeparam>
public class P<WholeType, Whole, Tenths, Hundredths, T> : P<WholeType, Whole, Tenths, Hundredths, N0, T>
    where WholeType : INumber<WholeType>
    where Whole : Const<WholeType>
    where Tenths : Const<int>
    where Hundredths : Const<int>
    where T : IFloatingPoint<T>;

/// <summary>
/// Represents a strongly-typed constant value composed of whole numbers and fractional parts, 
/// including tenths, hundredths, and thousandths, for a specified numeric type.
/// </summary>
/// <typeparam name="WholeType">
/// The type of the whole number part, constrained to implement <see cref="System.Numerics.INumber{T}"/>.
/// </typeparam>
/// <typeparam name="Whole">
/// A constant representing the whole number part of the value.
/// </typeparam>
/// <typeparam name="Tenths">
/// A constant representing the tenths part of the value.
/// </typeparam>
/// <typeparam name="Hundredths">
/// A constant representing the hundredths part of the value.
/// </typeparam>
/// <typeparam name="Thousandths">
/// A constant representing the thousandths part of the value.
/// </typeparam>
/// <typeparam name="T">
/// The resulting numeric type, constrained to implement <see cref="System.Numerics.IFloatingPoint{T}"/>.
/// </typeparam>
public class P<WholeType, Whole, Tenths, Hundredths, Thousandths, T> : Const<T>
    where WholeType : INumber<WholeType>
    where Whole : Const<WholeType>
    where Tenths : Const<int>
    where Hundredths : Const<int>
    where Thousandths : Const<int>
    where T : IFloatingPoint<T>
{
    /// <summary>
    /// Gets the computed constant value of the type <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// The value is calculated as the sum of the whole number part and the fractional parts 
    /// (tenths, hundredths, and thousandths), where each part is converted to the specified 
    /// numeric type <typeparamref name="T"/>.
    /// </remarks>
    public static T Value { get; } =
        T.CreateChecked(Whole.Value) +
        T.CreateChecked(Tenths.Value) / T.CreateChecked(N10.Value) +
        T.CreateChecked(Hundredths.Value) / T.CreateChecked(N100.Value) +
        T.CreateChecked(Thousandths.Value) / T.CreateChecked(N1000.Value);
}