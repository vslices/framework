using System.Numerics;

namespace VSlices.Domain.ValueObjects;

public readonly partial struct Length2
{
    /// <summary>
    ///
    /// </summary>
    public const double ToKilometerRatio = 0.001;

    /// <summary>
    ///
    /// </summary>
    public const double ToMetresRatio = 1;

    /// <summary>
    ///
    /// </summary>
    public const double ToCentimetresRatio = 100;

    /// <summary>
    ///
    /// </summary>
    public const double ToMillimetresRatio = 1000;

    /// <summary>
    ///
    /// </summary>
    public double Kilometres => CanonValue * ToKilometerRatio;

    /// <summary>
    ///
    /// </summary>
    public double Metres => CanonValue * ToMetresRatio;

    /// <summary>
    ///
    /// </summary>
    public double Centimetres => CanonValue * ToCentimetresRatio;

    /// <summary>
    ///
    /// </summary>
    public double Millimetres => CanonValue * ToMillimetresRatio;

}

/// <summary>
///
/// </summary>
public static class Length2Conversion
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="m"></param>
    /// <returns></returns>
    public static Length2 Kilometres<T>(this T m)
        where T : INumberBase<T> =>
        Length2.New(double.CreateChecked(m) * Length2.ToKilometerRatio);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="m"></param>
    /// <returns></returns>
    public static Length2 Metres<T>(this T m)
        where T : INumberBase<T> =>
        Length2.New(double.CreateChecked(m) * Length2.ToMetresRatio);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="m"></param>
    /// <returns></returns>
    public static Length2 Centimetres<T>(this T m)
        where T : INumberBase<T> =>
        Length2.New(double.CreateChecked(m) * Length2.ToCentimetresRatio);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="m"></param>
    /// <returns></returns>
    public static Length2 Millimetres<T>(this T m)
        where T : INumberBase<T> =>
        Length2.New(double.CreateChecked(m) * Length2.ToMillimetresRatio);


}
