using System.Collections.Concurrent;
using LanguageExt;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleFileRepo;

public sealed class InMemoryFileWork :
    AlgebraIO<FileAlgebra>,
    PointReader<SampleFile, SampleFileId>,
    PointWriter<SampleFile>,
    FileIdSource
{
    private readonly ConcurrentDictionary<SampleFileId, SampleFile> files = new();

    public InMemoryFileWork() =>
        Algebra = new(
            Reader: this,
            Writer: this,
            Ids: this);

    public FileAlgebra Algebra { get; }

    public int Count => files.Count;

    public IO<SampleFileId> Next() =>
        IO.lift(() => new SampleFileId(Guid.NewGuid()));

    public IO<Option<SampleFile>> Read(SampleFileId id) =>
        IO.lift(() =>
            files.TryGetValue(id, out var file)
                ? Some(file)
                : Option<SampleFile>.None);

    public IO<Unit> Write(SampleFile point) =>
        IO.lift(() =>
        {
            files[point.Id] = point;
            return unit;
        });
}
