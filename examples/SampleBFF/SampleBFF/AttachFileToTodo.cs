using LanguageExt;
using LanguageExt.Common;
using SampleFileRepo;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices;
using VSlices.Monads;
using VSlices.Work;

namespace SampleBFF;

public sealed class AttachFileToTodo :
    Feature<
        AttachFileToTodo,
        AlgebraSum<FileAlgebra, TodoAlgebra>,
        AttachFileToTodo.Request,
        AttachFileToTodo.Response>
{
    public sealed record Request(
        TodoId TodoId,
        string Name,
        byte[] Content);

    public sealed record Attached(
        Todo Todo,
        SampleFile File);

    public sealed record Response(
        Either<Error, Option<Attached>> Attachment);

    public static Flow<
        AlgebraSum<FileAlgebra, TodoAlgebra>,
        Request,
        Response> Get()
    {
        var addFile = AddFile
            .Get()
            .MapRuntime<AlgebraSum<FileAlgebra, TodoAlgebra>>(sum => sum.A)
            .MapRequest<Request>(request =>
                new AddFile.Request(
                    request.Name,
                    request.Content));

        return addFile.Bind(stored =>
            ResourceReference.Transformation
                .RunFin(stored.File.Id.ToString())
                .Match(
                    Succ: resource =>
                        AddAttachmentReference
                            .Get()
                            .MapRuntime<AlgebraSum<FileAlgebra, TodoAlgebra>>(sum => sum.B)
                            .MapRequest<Request>(request =>
                                new AddAttachmentReference.Request(
                                    request.TodoId,
                                    resource))
                            .Map(associated =>
                                associated.Todo.Match(
                                    Left: error =>
                                        new Response(
                                            Either.Left<
                                                Error,
                                                Option<Attached>>(error)),
                                    Right: maybe =>
                                        new Response(
                                            Either.Right<
                                                Error,
                                                Option<Attached>>(
                                                maybe.Map(todo =>
                                                    new Attached(
                                                        todo,
                                                        stored.File)))))),
                    Fail: error =>
                        Flow<
                            AlgebraSum<FileAlgebra, TodoAlgebra>,
                            Request>.Pure(
                            new Response(
                                Either.Left<
                                    Error,
                                    Option<Attached>>(error)))));
    }
}
