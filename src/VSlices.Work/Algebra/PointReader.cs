using LanguageExt;

namespace VSlices.Work;

/// <summary>
/// Executable algebraic atom for reading a semantic point by identity.
/// </summary>
/// <remarks>
/// <see cref="ReadOrDefault"/> is the only operation a Grounding must realize.
/// <see cref="Read"/> adds LINQ-like required-value semantics: absence is no
/// longer an expected branch and therefore fails exceptionally.
/// </remarks>
public interface PointReader<POINT, ID>
{
    /// <summary>
    /// Reads a point when it exists.
    /// </summary>
    OptionT<IO, POINT> ReadOrDefault(ID id);

    /// <summary>
    /// Reads a point that is expected to exist.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no point exists for the supplied identity.
    /// </exception>
    IO<POINT> Read(ID id) =>
        ReadOrDefault(id)
            .Run()
            .As()
            .Map(point =>
                point.IfNone(
                    static () =>
                        throw new InvalidOperationException(
                            "Sequence contains no matching element.")));
}

/// <summary>
/// Makes the default <see cref="PointReader{POINT, ID}.Read"/> implementation
/// available on concrete PointReader implementations.
///
/// A concrete implementation may declare its own Read method when required
/// point lookup needs more specific failure or retrieval semantics. Such an
/// override should preserve the meaning that the point is required to exist.
/// Normal C# member resolution gives that instance method precedence over this
/// extension.
/// </summary>
public static class PointReaderExtensions
{
    public static IO<POINT> Read<POINT, ID>(
        this PointReader<POINT, ID> reader,
        ID id) =>
        reader.Read(id);
}
