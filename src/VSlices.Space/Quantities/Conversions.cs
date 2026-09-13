using System.Numerics;
using VSlices.Space.Quantities;
using VSlices.Space.Quantities.Abstract;

namespace VSlices.Space;

/// <summary>
/// Explicit semantic establishments for VSlices-owned algebraic quantities.
/// This manual surface is intentionally shaped like the code a future source generator may emit.
/// </summary>
public static partial class Conversions
{
    public static Area<C, T> area<C, T>(
        Power<M.Length, N2, C, T> power)
        where C : Coordinate<M.Length>
        where T : INumber<T> =>
        new(power);

    /// <summary>
    /// Consumer convenience for rectangular area. Product reduces algebraically to
    /// Power&lt;Length,N2&gt; before semantic establishment; Product itself never becomes Area.
    /// </summary>
    public static Area<C, T> area<C, T>(
        Product<M.Length, C, T> product)
        where C : Coordinate<M.Length>
        where T : INumber<T> =>
        area((Power<M.Length, N2, C, T>)product);

    public static Volume<C, T> volume<C, T>(
        Power<M.Length, N3, C, T> power)
        where C : Coordinate<M.Length>
        where T : INumber<T> =>
        new(power);

    public static Speed<LENGTH_C, DURATION_C, T> speed<LENGTH_C, DURATION_C, T>(
        Quotient<M.Length, LENGTH_C, M.Duration, DURATION_C, T> quotient)
        where LENGTH_C : Coordinate<M.Length>
        where DURATION_C : Coordinate<M.Duration>
        where T : INumber<T> =>
        new(quotient);
}
