namespace VSlices.Domain.ValueObjects;

public readonly partial struct Length2
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Length2 Add(Length2 v) =>
        this + v;

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Length2 Subtract(Length2 v) =>
        this - v;

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Length2 Multiply(double v) =>
        this * v;

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Length2 Divide(double v) =>
        this / v;

}
