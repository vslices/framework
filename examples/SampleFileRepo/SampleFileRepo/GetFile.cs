using LanguageExt;
using LanguageExt.Traits;
using VSlices;
using VSlices.Monads;
using VSlices.Work;

namespace SampleFileRepo;

public sealed class GetFile<RT> :
    Feature<GetFile<RT>, RT, GetFile<RT>.Request, GetFile<RT>.Response>
    where RT : HasAlgebra<GetFileAlgebra, RT>
{
    public sealed record Request(SampleFileId Id);
    public sealed record Response(Option<SampleFile> File);

    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => AlgebraEnv<GetFileAlgebra, RT>
            .run(GetFileWork.Program(request.Id))
            .Map(file => new Response(file)));
}

public static class GetFileWork
{
    public static Free<GetFileAlgebra, Option<SampleFile>> Program(SampleFileId id) =>
        PointReader.read<GetFileAlgebra, SampleFile, SampleFileId>(id);
}

public abstract record GetFileWorkPart<A> : K<GetFileAlgebra, A>;
public sealed record GetFileReadPart<A>(
    SampleFileId Id,
    Func<Option<SampleFile>, A> Next) : GetFileWorkPart<A>;

public sealed class GetFileAlgebra :
    Functor<GetFileAlgebra>,
    PointReader<GetFileAlgebra, SampleFile, SampleFileId>
{
    static K<GetFileAlgebra, Option<SampleFile>>
        PointReader<GetFileAlgebra, SampleFile, SampleFileId>.Read(SampleFileId id) =>
        new GetFileReadPart<Option<SampleFile>>(id, static file => file);

    static K<GetFileAlgebra, B> Functor<GetFileAlgebra>.Map<A, B>(
        Func<A, B> f,
        K<GetFileAlgebra, A> ma) =>
        ma switch
        {
            GetFileReadPart<A>(var id, var next) =>
                new GetFileReadPart<B>(id, file => f(next(file))),
            _ => throw new NotSupportedException()
        };
}
