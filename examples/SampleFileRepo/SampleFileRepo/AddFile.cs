using LanguageExt;
using LanguageExt.Traits;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleFileRepo;

public sealed class AddFile :
    Feature<AddFile, AddFile.Algebra, AddFile.Request, AddFile.Response>
{
    public sealed record Request(
        string Name,
        byte[] Content);

    public sealed record Response(SampleFile File);

    public abstract record WorkPart<A> : K<Algebra, A>;

    public sealed record NextIdPart<A>(
        Func<SampleFileId, A> Next) : WorkPart<A>;

    public sealed record WritePart<A>(
        SampleFile Point,
        Func<Unit, A> Next) : WorkPart<A>;

    public sealed class Algebra :
        Functor<Algebra>,
        PointWriter<Algebra, SampleFile>
    {
        public static K<Algebra, SampleFileId> NextId() =>
            new NextIdPart<SampleFileId>(static id => id);

        static K<Algebra, Unit>
            PointWriter<Algebra, SampleFile>.Write(SampleFile point) =>
            new WritePart<Unit>(point, static value => value);

        static K<Algebra, B> Functor<Algebra>.Map<A, B>(
            Func<A, B> f,
            K<Algebra, A> ma) =>
            ma switch
            {
                NextIdPart<A>(var next) =>
                    new NextIdPart<B>(id => f(next(id))),
                WritePart<A>(var point, var next) =>
                    new WritePart<B>(point, value => f(next(value))),
                _ => throw new NotSupportedException()
            };
    }

    private static Free<Algebra, SampleFileId> nextId() =>
        Free.lift(Algebra.NextId());

    public static Free<Algebra, Response> Get(Request request) =>
        from id in nextId()
        let file = new SampleFile(
            id,
            request.Name,
            [.. request.Content])
        from _ in PointWriter.write<Algebra, SampleFile>(file)
        select new Response(file);
}
