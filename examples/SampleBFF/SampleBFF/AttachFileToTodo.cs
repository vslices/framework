using System.Runtime.CompilerServices;
using LanguageExt;
using LanguageExt.Common;
using SampleFileRepo;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;

namespace SampleBFF;

public sealed class AttachFileToTodo :
    Feature<AttachFileToTodo, AlgebraMix<FileAlgebra, TodoAlgebra>, AttachFileToTodo.Request, AttachFileToTodo.Response>
{
    public sealed record Request(TodoId TodoId, string Name, byte[] Content);
    public sealed record Attached(Todo Todo, SampleFile File);

    public sealed record Response
    {
        private Response(Either<Error, Option<Attached>> attachment) => Attachment = attachment;
        public Either<Error, Option<Attached>> Attachment { get; }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    private static extern Response NewResponse(Either<Error, Option<Attached>> attachment);

    public static Eff<AlgebraMix<FileAlgebra, TodoAlgebra>, Response> Get(Request request) =>
        Eff<AlgebraMix<FileAlgebra, TodoAlgebra>, Response>.LiftIO(algebra =>
            AddFile.Get(new AddFile.Request(request.Name, request.Content))
                .RunIO(algebra.A)
                .Bind(stored =>
                    ResourceReference.Transform(stored.File.Id.ToString()).Match(
                        Succ: resource =>
                            AddAttachmentReference.Get(
                                    new AddAttachmentReference.Request(request.TodoId, resource))
                                .RunIO(algebra.B)
                                .Map(associated =>
                                    associated.Todo.Match(
                                        Left: error => NewResponse(
                                            Either.Left<Error, Option<Attached>>(error)),
                                        Right: maybe => NewResponse(
                                            Either.Right<Error, Option<Attached>>(
                                                maybe.Map(todo => new Attached(todo, stored.File)))))),
                        Fail: error =>
                            IO.pure(NewResponse(
                                Either.Left<Error, Option<Attached>>(error))))));
}
