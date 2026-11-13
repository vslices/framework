namespace VSlices.Work;

/// <summary>
/// Describes a Work runtime that makes point reading available to a Feature.
/// Unlike <see cref="PointReader{POINT, ID}"/>, this is a capability projection,
/// not the reader implementation itself.
/// </summary>
public interface HasPointReader<POINT, ID>
{
    PointReader<POINT, ID> Reader { get; }
}

/// <summary>Exposes point writing to a Feature through its Algebra.</summary>
public interface HasPointWriter<POINT>
{
    PointWriter<POINT> Writer { get; }
}

/// <summary>Exposes point removal to a Feature through its Algebra.</summary>
public interface HasPointRemover<POINT, ID>
{
    PointRemover<POINT, ID> Remover { get; }
}
