using LanguageExt;
using VSlices.Work;

namespace SampleFileRepo;

public interface FileIdSource
{
    IO<SampleFileId> Next();
}

public sealed record FileAlgebra(
    PointReader<SampleFile, SampleFileId> Reader,
    PointWriter<SampleFile> Writer,
    FileIdSource Ids);
