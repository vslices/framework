using System.Numerics;
using VSlices.Space.Quantities.Abstract;

namespace VSlices.Space.Quantities;

/// <summary>
/// Structural multiplication of one magnitude by itself over one shared primitive basis.
/// This form preserves the multiplication that was performed while admitting the
/// algebraic reduction Mul&lt;F,F&gt; -> Pow&lt;F,N2&gt;.
/// </summary>
public sealed record Product<F, C, T>(T Value) :
    Q<M.Mul<F, F>, C, T>,
    DiscreteSpace<Product<F, C, T>>
    where F : M
    where C : Coordinate
    where T : INumber<T>
{
    /// <summary>
    /// Algebraic reduction from F x F to F^2. This conversion is implicit because it
    /// preserves value, primitive coordinate basis and numeric carrier and adds no
    /// domain semantics.
    /// </summary>
    public static implicit operator Power<F, N2, C, T>(Product<F, C, T> product) =>
        new(product.Value);

    /// <summary>
    /// Explicit algebraic expansion from F^2 back to F x F.
    /// </summary>
    public static explicit operator Product<F, C, T>(Power<F, N2, C, T> power) =>
        new(power.Value);
}

/// <summary>
/// Structural multiplication of two different magnitude shapes that can still be
/// expressed over one shared primitive coordinate basis C.
/// </summary>
public sealed record Product<LEFT_F, RIGHT_F, C, T>(T Value) :
    Q<M.Mul<LEFT_F, RIGHT_F>, C, T>,
    DiscreteSpace<Product<LEFT_F, RIGHT_F, C, T>>
    where LEFT_F : M
    where RIGHT_F : M
    where C : Coordinate
    where T : INumber<T>;

/// <summary>
/// Structural multiplication whose operand coordinate bases remain independently
/// represented. Because there is no single truthful C, this form intentionally does
/// not implement Q&lt;F,C,T&gt;.
/// </summary>
public sealed record Product<LEFT_F, LEFT_C, RIGHT_F, RIGHT_C, T>(T Value) :
    DiscreteSpace<Product<LEFT_F, LEFT_C, RIGHT_F, RIGHT_C, T>>
    where LEFT_F : M
    where LEFT_C : Coordinate
    where RIGHT_F : M
    where RIGHT_C : Coordinate
    where T : INumber<T>;
