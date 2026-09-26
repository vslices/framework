using VSlices.Monads;
using VSlices.Work;

namespace SampleFileRepo;

public sealed class AddFile :
    Feature<AddFile, FileAlgebra, AddFile.Request, AddFile.Response>
{
    public sealed record Request(string Name, byte[] Content);
    public sealed record Response(SampleFile File);

    public static Flow<FileAlgebra, Request, Response> Get() =>
        new((algebra, request) =>
            algebra.Ids.Next()
                .Bind(id =>
                {
                    var file = new SampleFile(id, request.Name, [.. request.Content]);
                    return algebra.Writer.Write(file)
                        .Map(_ => new Response(file));
                }));
}
