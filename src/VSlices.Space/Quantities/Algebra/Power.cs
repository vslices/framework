using System.Numerics;
using VSlices.Space.Quantities.Abstract;

namespace VSlices.Space.Quantities;

/// <summary>
/// Structural exponentiation of one magnitude over one primitive coordinate basis.
/// The algebraic shape lives in M.Pow; C remains the primitive basis used to express
/// the powered magnitude.
/// </summary>
public sealed record Power<BASE_F, EXPONENT, C, T>(T Value) :
    Q<M.Pow<BASE_F, EXPONENT>, C, T>,
    DiscreteSpace<Power<BASE_F, EXPONENT, C, T>>
    where BASE_F : M
    where EXPONENT : Exponent
    where C : Coordinate
    where T : INumber<T>;

/// <summary>
/// Explicit power operations for the exponent vocabulary currently pressured by
/// production cases. C# has no exponentiation operator, so these operations are named.
/// </summary>
public static class PowerOperations
{
    public static Power<M.Length, N2, C, T> square<SELF, C, T>(
        Length<SELF, C, T> value)
        where SELF : Length<SELF, C, T>
        where C : Coordinate<M.Length>
        where T : INumber<T> =>
        new(value.Value * value.Value);

    public static Power<M.Length, N3, C, T> cube<SELF, C, T>(
        Length<SELF, C, T> value)
        where SELF : Length<SELF, C, T>
        where C : Coordinate<M.Length>
        where T : INumber<T> =>
        new(value.Value * value.Value * value.Value);
}
