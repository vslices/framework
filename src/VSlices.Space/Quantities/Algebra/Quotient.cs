using System.Numerics;
using VSlices.Space.Quantities.Abstract;

namespace VSlices.Space.Quantities;

/// <summary>
/// Structural division whose numerator and denominator retain their own primitive
/// coordinate bases. Quotient materializes M.Div without inventing a synthetic
/// coordinate that would collapse distinct primitive bases into one.
/// </summary>
public sealed record Quotient<LEFT_F, LEFT_C, RIGHT_F, RIGHT_C, T>(T Value) :
    DiscreteSpace<Quotient<LEFT_F, LEFT_C, RIGHT_F, RIGHT_C, T>>
    where LEFT_F : M
    where LEFT_C : Coordinate
    where RIGHT_F : M
    where RIGHT_C : Coordinate
    where T : INumber<T>;

/// <summary>
/// Explicitly authorized Length / Duration division.
/// The result preserves both primitive coordinate bases instead of normalizing
/// them into an invented single coordinate.
/// </summary>
public static class LengthDurationQuotientOperators
{
    extension<LEFT_SELF, LEFT_C, RIGHT_SELF, RIGHT_C, T>(Length<LEFT_SELF, LEFT_C, T>)
        where LEFT_SELF : Length<LEFT_SELF, LEFT_C, T>
        where LEFT_C : Coordinate<M.Length>
        where RIGHT_SELF : Duration<RIGHT_SELF, RIGHT_C, T>
        where RIGHT_C : Coordinate<M.Duration>
        where T : INumber<T>
    {
        public static Quotient<M.Length, LEFT_C, M.Duration, RIGHT_C, T> operator /(
            Length<LEFT_SELF, LEFT_C, T> left,
            Duration<RIGHT_SELF, RIGHT_C, T> right) =>
            new(left.Value / right.Value);
    }
}

/// <summary>
/// Explicitly authorized Length / Duration^2 division. This is the already-reduced
/// algebraic form used by Acceleration.
/// </summary>
public static class LengthSquaredDurationQuotientOperators
{
    extension<LEFT_SELF, LEFT_C, RIGHT_C, T>(Length<LEFT_SELF, LEFT_C, T>)
        where LEFT_SELF : Length<LEFT_SELF, LEFT_C, T>
        where LEFT_C : Coordinate<M.Length>
        where RIGHT_C : Coordinate<M.Duration>
        where T : INumber<T>
    {
        public static Quotient<M.Length, LEFT_C, M.Pow<M.Duration, N2>, RIGHT_C, T> operator /(
            Length<LEFT_SELF, LEFT_C, T> left,
            Power<M.Duration, N2, RIGHT_C, T> right) =>
            new(left.Value / right.Value);
    }
}

/// <summary>
/// Concrete algebraic reduction pressured by acceleration:
///
///   Div&lt;Div&lt;Length,Duration&gt;,Duration&gt;
///   -> Div&lt;Length,Pow&lt;Duration,N2&gt;&gt;
///
/// The incoming Duration is first converted to the Duration basis already carried by
/// Speed, then the operation materializes directly in the reduced Quotient shape.
/// No general quotient-normalization engine is implied by this rule.
/// </summary>
public static class SpeedDurationQuotientOperators
{
    extension<LENGTH_C, DURATION_C, RIGHT_SELF, RIGHT_C, T>(Speed<LENGTH_C, DURATION_C, T>)
        where LENGTH_C : Coordinate<M.Length>
        where DURATION_C : Coordinate<M.Duration>
        where RIGHT_SELF : Duration<RIGHT_SELF, RIGHT_C, T>
        where RIGHT_C : Coordinate<M.Duration>
        where T : INumber<T>
    {
        public static Quotient<M.Length, LENGTH_C, M.Pow<M.Duration, N2>, DURATION_C, T> operator /(
            Speed<LENGTH_C, DURATION_C, T> left,
            Duration<RIGHT_SELF, RIGHT_C, T> right)
        {
            var rightValue = T.CreateChecked(right.Value);
            var rightScale = T.CreateChecked(RIGHT_C.ReferenceScale);
            var leftDurationScale = T.CreateChecked(DURATION_C.ReferenceScale);
            var rightInLeftDurationCoordinate = rightValue * rightScale / leftDurationScale;

            return new(left.Value / rightInLeftDurationCoordinate);
        }
    }
}
