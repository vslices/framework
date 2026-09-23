namespace VSlices.Work;

/// <summary>
/// Realizes point-reading work against a concrete external world.
/// </summary>
public interface PointReaderIO<POINT, ID>
{
    IO<Option<POINT>> Read(ID id);
}

/// <summary>
/// Realizes point-writing work against a concrete external world.
/// </summary>
public interface PointWriterIO<POINT>
{
    IO<Unit> Write(POINT point);
}

/// <summary>
/// Realizes point-removal work against a concrete external world.
/// </summary>
public interface PointRemoverIO<POINT, ID>
{
    IO<Unit> Remove(ID id);
}
