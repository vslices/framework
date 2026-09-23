namespace VSlices.Space.Finance;

/// <summary>
/// Identifies a nominal currency vocabulary.
///
/// Currency is deliberately not a measurement Coordinate: unlike meters and
/// kilometers, two currencies do not have a fixed type-level conversion scale.
/// </summary>
public interface Currency
{
    static abstract string Code { get; }
    static abstract string Name { get; }
    static abstract string Symbol { get; }
    static abstract int Decimals { get; }
}
