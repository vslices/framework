using System.Numerics;
using LanguageExt;
using VSlices.Space.Quantities.Abstract;

namespace VSlices.Space.Quantities;

/// <summary>
/// Most general nominally closed Duration family.
/// SELF is first because nominal closure is the primary type being defined;
/// C and T describe its coordinate basis and numeric carrier.
/// </summary>
public abstract class Duration<SELF, C, T> : Q<M.Duration, C, T>
    where SELF : Duration<SELF, C, T>
    where C : Coordinate<M.Duration>
    where T : INumber<T>
{
    private static readonly Func<T, SELF> Reconstruct = IL.Ctor<T, SELF>();

    protected Duration(T value) => Value = value;
    public T Value { get; }

    public SELF Add<RIGHT_SELF, RIGHT_C, RIGHT_T>(Duration<RIGHT_SELF, RIGHT_C, RIGHT_T> right)
        where RIGHT_SELF : Duration<RIGHT_SELF, RIGHT_C, RIGHT_T>
        where RIGHT_C : Coordinate<M.Duration>
        where RIGHT_T : INumber<RIGHT_T> =>
        Reconstruct(Value + ConvertToLeft(right));

    public SELF Subtract<RIGHT_SELF, RIGHT_C, RIGHT_T>(Duration<RIGHT_SELF, RIGHT_C, RIGHT_T> right)
        where RIGHT_SELF : Duration<RIGHT_SELF, RIGHT_C, RIGHT_T>
        where RIGHT_C : Coordinate<M.Duration>
        where RIGHT_T : INumber<RIGHT_T> =>
        Reconstruct(Value - ConvertToLeft(right));

    private static T ConvertToLeft<RIGHT_SELF, RIGHT_C, RIGHT_T>(Duration<RIGHT_SELF, RIGHT_C, RIGHT_T> right)
        where RIGHT_SELF : Duration<RIGHT_SELF, RIGHT_C, RIGHT_T>
        where RIGHT_C : Coordinate<M.Duration>
        where RIGHT_T : INumber<RIGHT_T>
    {
        var rightValue = T.CreateChecked(right.Value);
        var rightScale = T.CreateChecked(RIGHT_C.ReferenceScale);
        var leftScale = T.CreateChecked(C.ReferenceScale);
        return rightValue * rightScale / leftScale;
    }
}

public abstract class Duration<SELF, T> : Duration<SELF, Seconds, T>
    where SELF : Duration<SELF, T>
    where T : INumber<T>
{
    protected Duration(T value) : base(value) { }
}

/// <summary>
/// Default seconds-based Duration. This concrete family is also a VectorSpace:
/// durations are displacements, have an additive origin, and close under homogeneous
/// addition/subtraction, negation, and scalar multiplication/division.
///
/// The VectorSpace promotion is intentionally made on this concrete coordinate form
/// first. More general coordinate-aware vector closure remains separate pressure.
/// </summary>
public class Duration<T> :
    Duration<Duration<T>, T>,
    VectorSpace<Duration<T>, T>
    where T : INumber<T>
{
    public Duration(T value) : base(value) { }

    public bool Equals(Duration<T>? other) =>
        other is not null && Value == other.Value;

    public override bool Equals(object? obj) =>
        obj is Duration<T> other && Equals(other);

    public override int GetHashCode() =>
        Value.GetHashCode();

    public static bool operator ==(Duration<T>? left, Duration<T>? right) =>
        Equals(left, right);

    public static bool operator !=(Duration<T>? left, Duration<T>? right) =>
        !Equals(left, right);

    public static Duration<T> operator +(Duration<T> left, Duration<T> right) =>
        new(left.Value + right.Value);

    public static Duration<T> operator -(Duration<T> left, Duration<T> right) =>
        new(left.Value - right.Value);

    public static Duration<T> operator -(Duration<T> value) =>
        new(-value.Value);

    public static Duration<T> operator *(Duration<T> value, T scalar) =>
        new(value.Value * scalar);

    public static Duration<T> operator /(Duration<T> value, T scalar) =>
        new(value.Value / scalar);

    public static Duration<T> AdditiveIdentity =>
        new(T.Zero);
}

public sealed class vDuration : Duration<vDuration, double>
{
    public vDuration(double value) : base(value) { }
}

public static class DurationOperators
{
    extension<LEFT_SELF, LEFT_C, LEFT_T, RIGHT_SELF, RIGHT_C, RIGHT_T>(Duration<LEFT_SELF, LEFT_C, LEFT_T>)
        where LEFT_SELF : Duration<LEFT_SELF, LEFT_C, LEFT_T>
        where LEFT_C : Coordinate<M.Duration>
        where LEFT_T : INumber<LEFT_T>
        where RIGHT_SELF : Duration<RIGHT_SELF, RIGHT_C, RIGHT_T>
        where RIGHT_C : Coordinate<M.Duration>
        where RIGHT_T : INumber<RIGHT_T>
    {
        public static LEFT_SELF operator +(
            Duration<LEFT_SELF, LEFT_C, LEFT_T> left,
            Duration<RIGHT_SELF, RIGHT_C, RIGHT_T> right) => left.Add(right);

        public static LEFT_SELF operator -(
            Duration<LEFT_SELF, LEFT_C, LEFT_T> left,
            Duration<RIGHT_SELF, RIGHT_C, RIGHT_T> right) => left.Subtract(right);
    }
}
