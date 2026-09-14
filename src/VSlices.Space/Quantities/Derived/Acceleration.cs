using System.Numerics;
using VSlices.Space.Quantities.Abstract;

namespace VSlices.Space.Quantities;

/// <summary>
/// Semantic acceleration established from Length / Duration^2.
/// The defining algebraic base is the reduced quotient
/// Div&lt;Length,Pow&lt;Duration,N2&gt;&gt;, regardless of whether it was produced directly
/// from Length / square(Duration) or by reducing Speed / Duration.
/// </summary>
[AlgebraicSymbol("acceleration")]
public sealed record Acceleration<LENGTH_C, DURATION_C, T>(
    Quotient<M.Length, LENGTH_C, M.Pow<M.Duration, N2>, DURATION_C, T> Quotient) :
    DerivedSpace<
        Acceleration<LENGTH_C, DURATION_C, T>,
        Quotient<M.Length, LENGTH_C, M.Pow<M.Duration, N2>, DURATION_C, T>>
    where LENGTH_C : Coordinate<M.Length>
    where DURATION_C : Coordinate<M.Duration>
    where T : INumber<T>
{
    public T Value => Quotient.Value;

    public Quotient<M.Length, LENGTH_C, M.Pow<M.Duration, N2>, DURATION_C, T> ToBase() =>
        Quotient;
}
