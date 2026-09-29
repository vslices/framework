using LanguageExt;
using LanguageExt.Common;
using SampleFileRepo;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Monads;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleBFF;

/// <summary>
/// Cross-service Feature.
///
/// SampleFileRepo owns the stored file.
/// SampleWorkflow owns the Todo-to-resource association.
/// SampleBFF alone knows that a SampleFileId can be represented as a Todo ResourceReference.
/// </summary>
public sealed class AttachFileToTodo<RT> :
    Feature<
        AttachFileToTodo<RT>,
        RT,
        AttachFileToTodo<RT>.Request,
        AttachFileToTodo<RT>.Response>
    where RT :
        HasAlgebra<AddFileAlgebra, RT>,
        HasAlgebra<AddAttachmentReferenceAlgebra, RT>
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

    public static Flow<RT, Request, Response> Get() =>
        new((runtime, request) =>
            AddFile<RT>
                .Get()
                .RunFlow(
                    runtime,
                    new AddFile<RT>.Request(
                        request.Name,
                        request.Content))
                .Bind(stored =>
                    ResourceReference.Transformation
                        .RunFin(stored.File.Id.ToString())
                        .Match(
                            Succ: resource =>
                                AddAttachmentReference<RT>
                                    .Get()
                                    .RunFlow(
                                        runtime,
                                        new AddAttachmentReference<RT>.Request(
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
                                IO.pure(
                                    new Response(
                                        Either.Left<
                                            Error,
                                            Option<Attached>>(error))))));
}
