namespace VSlices.Work;

/// <summary>
/// Executable algebraic atom for writing a semantic point.
/// </summary>
public interface PointWriter<POINT>
{
    IO<Unit> Write(POINT point);
}
