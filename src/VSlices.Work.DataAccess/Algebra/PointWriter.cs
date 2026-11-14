using LanguageExt;
namespace VSlices.Work.Algebra;

/// <summary>
/// Executable algebraic atom for writing a semantic point.
/// </summary>
public interface PointWriter<POINT>
{
    IO<Unit> Write(POINT point);
}

/// <summary>Projects point writing into a Feature's runtime Algebra.</summary>
public interface HasPointWriter<POINT>
{
    PointWriter<POINT> Writer { get; }
}
