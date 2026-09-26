using System.Collections.Concurrent;
using LanguageExt;
using LanguageExt.Traits;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleFileRepo;

public sealed class InMemoryFileWork :
    AlgebraIO<AddFileAlgebra>,
    AlgebraIO<GetFileAlgebra>
{
    private readonly ConcurrentDictionary<SampleFileId, SampleFile> files = new();

    public int Count => files.Count;

    IO<A> AlgebraIO<AddFileAlgebra>.Interpret<A>(
        K<AddFileAlgebra, A> operation) =>
        operation switch
        {
            AddFileNextIdPart<A> next =>
                IO.lift(() =>
                    next.Next(
                        new SampleFileId(
                            Guid.NewGuid()))),
            AddFileWritePart<A> write =>
                IO.lift(() =>
                {
                    files[write.Point.Id] = write.Point;
                    return write.Next(unit);
                }),
            _ => throw new NotSupportedException()
        };

    IO<A> AlgebraIO<GetFileAlgebra>.Interpret<A>(
        K<GetFileAlgebra, A> operation) =>
        operation switch
        {
            GetFileReadPart<A> read =>
                IO.lift(() =>
                    read.Next(Read(read.Id))),
            _ => throw new NotSupportedException()
        };

    private Option<SampleFile> Read(SampleFileId id) =>
        files.TryGetValue(id, out var file)
            ? Some(file)
            : Option<SampleFile>.None;
}
