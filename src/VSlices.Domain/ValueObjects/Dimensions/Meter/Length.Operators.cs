namespace VSlices.Domain.ValueObjects;

public readonly partial struct Length2
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Length2 operator *(Length2 lhs, double rhs) =>
        new(lhs.CanonValue * rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Length2 operator *(double lhs, Length2 rhs) =>
        rhs * lhs;

    /// <summary>
    ///
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Length2 operator -(Length2 self) =>
        new(-self.CanonValue);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Length2 operator +(Length2 lhs, Length2 rhs) =>
        new(lhs.CanonValue + rhs.CanonValue);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Length2 operator -(Length2 lhs, Length2 rhs) =>
        new(lhs.CanonValue - lhs.CanonValue);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Length2 operator /(Length2 lhs, double rhs) =>
        new(lhs.CanonValue / rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static double operator /(Length2 lhs, Length2 rhs) =>
        lhs.CanonValue / rhs.CanonValue;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(Length2 lhs, Length2 rhs) =>
        lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(Length2 lhs, Length2 rhs) =>
        !lhs.Equals(rhs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(Length2 lhs, Length2 rhs) =>
        lhs.CanonValue > rhs.CanonValue;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(Length2 lhs, Length2 rhs) =>
        lhs.CanonValue < rhs.CanonValue;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(Length2 lhs, Length2 rhs) =>
        lhs.CanonValue >= rhs.CanonValue;

    /// <summary>
    ///
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(Length2 lhs, Length2 rhs) =>
        lhs.CanonValue <= rhs.CanonValue;

}
