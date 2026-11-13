namespace VSlices.Work.Algebra;

/// <summary>
/// Executable algebraic atom for removing a semantic point by identity.
/// </summary>
public interface PointRemover<POINT, ID>
{
    IO<Unit> Remove(ID id);
}

/// <summary>Projects point removal into a Feature's runtime Algebra.</summary>
public interface HasPointRemover<POINT, ID>
{
    PointRemover<POINT, ID> Remover { get; }
}
