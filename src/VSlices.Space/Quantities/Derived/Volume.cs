using System.Numerics;
using VSlices.Space.Quantities.Abstract;

namespace VSlices.Space.Quantities;

/// <summary>
/// Semantic volume established from cubed Length.
/// Power&lt;Length,N3&gt; is the defining algebraic base regardless of whether that power
/// was produced directly by cube(length) or through Area x Length.
/// </summary>
[AlgebraicSymbol("volume")]
public sealed record Volume<C, T>(
    Power<M.Length, N3, C, T> Power) :
    Q<M.Pow<M.Length, N3>, C, T>,
    DerivedSpace<Volume<C, T>, Power<M.Length, N3, C, T>>
    where C : Coordinate<M.Length>
    where T : INumber<T>
{
    public T Value => Power.Value;

    public Power<M.Length, N3, C, T> ToBase() => Power;
}
