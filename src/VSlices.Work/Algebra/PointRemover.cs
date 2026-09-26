namespace VSlices.Work;

/// <summary>
/// Executable algebraic atom for removing a semantic point by identity.
/// </summary>
public interface PointRemover<POINT, ID>
{
    IO<Unit> Remove(ID id);
}
