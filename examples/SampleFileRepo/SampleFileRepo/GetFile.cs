using LanguageExt;
using VSlices.Monads;
using VSlices.Work;

namespace SampleFileRepo;

public sealed class GetFile :
    Feature<GetFile, FileAlgebra, GetFile.Request, GetFile.Response>
{
    public sealed record Request(SampleFileId Id);
    public sealed record Response(Option<SampleFile> File);

    public static Flow<FileAlgebra, Request, Response> Get() =>
        new((algebra, request) =>
            algebra.Reader.Read(request.Id)
                .Map(file => new Response(file)));
}
