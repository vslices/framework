using System.Collections.Concurrent;
using LanguageExt;
using LanguageExt.Traits;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleFileRepo;

public sealed class InMemoryFileWork :
    AlgebraIO<AddFile.Algebra>,
    AlgebraIO<GetFile.Algebra>
{
    private readonly ConcurrentDictionary<SampleFileId, SampleFile> files = new();

    IO<A> AlgebraIO<AddFile.Algebra>.Interpret<A>(
        K<AddFile.Algebra, A> operation) =>
        operation switch
        {
            AddFile.NextIdPart<A> next =>
                IO.lift(() =>
                    next.Next(
                        new SampleFileId(
                            Guid.NewGuid()))),
            AddFile.WritePart<A> write =>
                IO.lift(() =>
                {
                    files[write.Point.Id] = write.Point;
                    return write.Next(unit);
                }),
            _ => throw new NotSupportedException()
        };

    IO<A> AlgebraIO<GetFile.Algebra>.Interpret<A>(
        K<GetFile.Algebra, A> operation) =>
        operation switch
        {
            GetFile.ReadPart<A> read =>
                IO.lift(() =>
                    read.Next(Read(read.Id))),
            _ => throw new NotSupportedException()
        };

    private Option<SampleFile> Read(SampleFileId id) =>
        files.TryGetValue(id, out var file)
            ? Some(file)
            : Option<SampleFile>.None;
}
