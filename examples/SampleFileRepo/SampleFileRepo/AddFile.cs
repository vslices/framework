using LanguageExt;
using System.Runtime.CompilerServices;
using VSlices.Monads;
using VSlices.Work;

namespace SampleFileRepo;

public sealed class AddFile :
    Feature<AddFile, FileAlgebra, AddFile.Request, AddFile.Response>
{
    public sealed record Request(string Name, byte[] Content);
    public sealed record Response
    {
        private Response(SampleFile file) => File = file;
        public SampleFile File { get; }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    private static extern Response NewResponse(SampleFile file);

    public static Eff<FileAlgebra, Response> Get(Request request) =>
        Eff<FileAlgebra, Response>.LiftIO(algebra =>
            algebra.Ids.Next()
                .Bind(id =>
                {
                    var file = new SampleFile(id, request.Name, [.. request.Content]);
                    return algebra.Writer.Write(file)
                        .Map(_ => NewResponse(file));
                }));
}
