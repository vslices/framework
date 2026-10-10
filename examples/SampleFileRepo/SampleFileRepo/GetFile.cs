using LanguageExt;
using LanguageExt.Traits;
using VSlices.Work;

namespace SampleFileRepo;

public sealed class GetFile :
    Feature<GetFile.Algebra, GetFile.Request, GetFile.Response>
{
    public sealed record Request(SampleFileId Id);
    public sealed record Response(Option<SampleFile> File);

    public abstract record WorkPart<A> : K<Algebra, A>;

    public sealed record ReadPart<A>(
        SampleFileId Id,
        Func<Option<SampleFile>, A> Next) : WorkPart<A>;

    public sealed class Algebra :
        Functor<Algebra>,
        PointReader<Algebra, SampleFile, SampleFileId>
    {
        static K<Algebra, Option<SampleFile>>
            PointReader<Algebra, SampleFile, SampleFileId>.Read(
                SampleFileId id) =>
            new ReadPart<Option<SampleFile>>(
                id,
                static file => file);

        static K<Algebra, B> Functor<Algebra>.Map<A, B>(
            Func<A, B> f,
            K<Algebra, A> ma) =>
            ma switch
            {
                ReadPart<A>(var id, var next) =>
                    new ReadPart<B>(
                        id,
                        file => f(next(file))),
                _ => throw new NotSupportedException()
            };
    }

    public static Free<Algebra, Response> Describe(Request request) =>
        from file in PointReader.read<
            Algebra,
            SampleFile,
            SampleFileId>(request.Id)
        select new Response(file);
}
