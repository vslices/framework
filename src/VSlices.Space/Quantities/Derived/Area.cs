using System.Numerics;
using VSlices.Space.Quantities.Abstract;

namespace VSlices.Space.Quantities;

/// <summary>
/// Semantic area established from squared Length.
/// Power&lt;Length,N2&gt; is the defining algebraic base; Length x Length may reach this
/// base through the implicit Product&lt;Length,C,T&gt; -> Power&lt;Length,N2,C,T&gt; reduction.
/// </summary>
[AlgebraicSymbol("area")]
public sealed record Area<C, T>(
    Power<M.Length, N2, C, T> Power) :
    Q<M.Pow<M.Length, N2>, C, T>,
    DerivedSpace<Area<C, T>, Power<M.Length, N2, C, T>>
    where C : Coordinate<M.Length>
    where T : INumber<T>
{
    public T Value => Power.Value;

    public Power<M.Length, N2, C, T> ToBase() => Power;
}

/// <summary>
/// Explicitly authorized Area x Length multiplication.
/// RIGHT is converted to the Area's primitive Length basis before multiplication.
/// Because Area is already established over Length^2, the algebraic result is reduced
/// directly to Length^3 rather than preserving an intermediate Mul&lt;Pow&lt;Length,N2&gt;,Length&gt;.
/// </summary>
public static class AreaProductOperators
{
    extension<LEFT_C, RIGHT_SELF, RIGHT_C, T>(Area<LEFT_C, T>)
        where LEFT_C : Coordinate<M.Length>
        where RIGHT_SELF : Length<RIGHT_SELF, RIGHT_C, T>
        where RIGHT_C : Coordinate<M.Length>
        where T : INumber<T>
    {
        public static Power<M.Length, N3, LEFT_C, T> operator *(
            Area<LEFT_C, T> left,
            Length<RIGHT_SELF, RIGHT_C, T> right)
        {
            var rightValue = T.CreateChecked(right.Value);
            var rightScale = T.CreateChecked(RIGHT_C.ReferenceScale);
            var leftScale = T.CreateChecked(LEFT_C.ReferenceScale);
            var rightInLeftCoordinate = rightValue * rightScale / leftScale;

            return new(left.Value * rightInLeftCoordinate);
        }
    }
}
