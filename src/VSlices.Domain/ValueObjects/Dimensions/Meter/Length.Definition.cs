namespace VSlices.Domain.ValueObjects;

/// <summary>
/// Backed by meters
/// Scalar is double
/// </summary>
public readonly partial struct Length2 :
    DomainType<Length2, double>,
    QuantitySpace<Length2, double>
{
    /// <summary>
    ///
    /// </summary>
    public double CanonValue { get; }

    private Length2(double value) =>
        CanonValue = value;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public double To() =>
        CanonValue;

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Length2 other) =>
        CanonValue.Equals(other.CanonValue);

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    public bool Equals(Length2 other, double epsilon) =>
        Math.Abs(other.CanonValue - CanonValue) < epsilon;

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) =>
        obj is Length2 length && Equals(length);
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() =>
        CanonValue.GetHashCode();

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public int CompareTo(object? obj) =>
        obj switch
        {
            null => 1,
            Length2 other => CompareTo(other),
            _ => throw new ArgumentException($"must be of type {nameof(Length)}")
        };

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Length2 other) =>
        CanonValue.CompareTo(other.CanonValue);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{CanonValue} m";

    /// <summary>
    ///
    /// </summary>
    /// <param name="repr"></param>
    /// <returns></returns>
    public static Length2 New(double repr) => new(repr);

    /// <summary>
    ///
    /// </summary>
    public static Length2 AdditiveIdentity { get; } = new Length2(0);

}
