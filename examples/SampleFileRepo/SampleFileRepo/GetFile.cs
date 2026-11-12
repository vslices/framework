using System.Runtime.CompilerServices;
using LanguageExt;
using VSlices.Monads;
using VSlices.Work;

namespace SampleFileRepo;

public sealed class GetFile :
    Feature<GetFile, FileAlgebra, GetFile.Request, GetFile.Response>
{
    public sealed record Request(SampleFileId Id);
    public sealed record Response
    {
        private Response(Option<SampleFile> file) => File = file;
        public Option<SampleFile> File { get; }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    private static extern Response NewResponse(Option<SampleFile> file);

    public static Eff<FileAlgebra, Response> Get(Request request) =>
        Eff<FileAlgebra, Response>.LiftIO(algebra =>
            algebra.Reader.ReadOrDefault(request.Id).Run().As()
                .Map(file => NewResponse(file)));
}
