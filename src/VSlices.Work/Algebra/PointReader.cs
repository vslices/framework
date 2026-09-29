namespace VSlices.Work;

/// <summary>
/// Executable algebraic atom for reading a semantic point by identity.
/// </summary>
public interface PointReader<POINT, ID>
{
    IO<Option<POINT>> Read(ID id);
}
