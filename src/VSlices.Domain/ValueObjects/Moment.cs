using LanguageExt;
using static VSlices.Arrows.Req<
    VSlices.Domain.ValueObjects.Moment.Input,
    VSlices.Domain.ValueObjects.Moment>;

namespace VSlices.Domain.ValueObjects;

// TODO: Considerar un alias para DomainType + AffineSpace,
/// <summary>
/// 
/// </summary>
public sealed class Moment :
    DomainType<Moment, Moment.Repr>,
    AffineSpace<Moment, Time, double>,
    Transform<Moment, Moment.Input>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Value"></param>
    public readonly record struct Repr(DateTimeOffset Value);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Value"></param>
    public readonly record struct Input(DateTimeOffset Value);

    private readonly DateTimeOffset _v;

    private Moment(DateTimeOffset v) =>
        _v = v;

    /// <summary>
    /// 
    /// </summary>
    public static Req<Input, Moment, Input, Moment> Invariants =>
        Transform((Input i) => new Moment(i.Value));

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Repr To() => new(_v);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Moment? other) =>
        other is not null &&
        _v.Equals(other._v);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) =>
        Equals(obj as Moment);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() =>
        _v.GetHashCode();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Moment Now => new(DateTimeOffset.UtcNow);
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(Moment? left, Moment? right) =>
        Equals(left, right);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(Moment? left, Moment? right) =>
        !(left == right);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Moment operator +(Moment left, Time right) =>
        new(left._v.AddSeconds(right.Seconds));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Time operator -(Moment left, Moment right) =>
        new((left._v - right._v).TotalSeconds);

}
