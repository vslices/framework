using LanguageExt;
using LanguageExt.Common;
using SampleFileRepo;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;

namespace SampleBFF;

/// <summary>
/// Cross-service product process.
///
/// SampleFileRepo owns the stored file.
/// SampleWorkflow owns the Todo-to-resource association.
/// SampleBFF alone knows that a SampleFileId can be represented as a Todo ResourceReference.
/// </summary>
public sealed class AttachFileToTodo :
    WorkProcess<
        AttachFileToTodo,
        AlgebraSum<AddFile.Algebra, AddAttachmentReference.Algebra>,
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

    public static Free<
        AlgebraSum<AddFile.Algebra, AddAttachmentReference.Algebra>,
        Response> Get(Request request)
    {
        var addFile = FreeAlgebra.hoist<
            InjectLeft<AddFile.Algebra, AddAttachmentReference.Algebra>,
            AddFile.Algebra,
            AlgebraSum<AddFile.Algebra, AddAttachmentReference.Algebra>,
            AddFile.Response>(
                AddFile.Get(
                    new AddFile.Request(
                        request.Name,
                        request.Content)));

        return
            from stored in addFile
            from response in ResourceReference.Transformation
                .RunFin(stored.File.Id.ToString())
                .Match(
                    Succ: resource =>
                    {
                        var associate = FreeAlgebra.hoist<
                            InjectRight<AddFile.Algebra, AddAttachmentReference.Algebra>,
                            AddAttachmentReference.Algebra,
                            AlgebraSum<AddFile.Algebra, AddAttachmentReference.Algebra>,
                            AddAttachmentReference.Response>(
                                AddAttachmentReference.Get(
                                    new AddAttachmentReference.Request(
                                        request.TodoId,
                                        resource)));

                        return
                            from associated in associate
                            from mapped in associated.Todo.Match(
                                Left: error =>
                                    Free.pure<
                                        AlgebraSum<
                                            AddFile.Algebra,
                                            AddAttachmentReference.Algebra>,
                                        Response>(
                                            new Response(
                                                Either.Left<
                                                    Error,
                                                    Option<Attached>>(error))),
                                Right: maybe =>
                                    maybe.Match(
                                        Some: todo =>
                                            Free.pure<
                                                AlgebraSum<
                                                    AddFile.Algebra,
                                                    AddAttachmentReference.Algebra>,
                                                Response>(
                                                    new Response(
                                                        Either.Right<
                                                            Error,
                                                            Option<Attached>>(
                                                                Some(
                                                                    new Attached(
                                                                        todo,
                                                                        stored.File))))),
                                        None: () =>
                                            Free.pure<
                                                AlgebraSum<
                                                    AddFile.Algebra,
                                                    AddAttachmentReference.Algebra>,
                                                Response>(
                                                    new Response(
                                                        Either.Right<
                                                            Error,
                                                            Option<Attached>>(
                                                                Option<Attached>.None)))))
                            select mapped;
                    },
                    Fail: error =>
                        Free.pure<
                            AlgebraSum<
                                AddFile.Algebra,
                                AddAttachmentReference.Algebra>,
                            Response>(
                                new Response(
                                    Either.Left<
                                        Error,
                                        Option<Attached>>(error))))
            select response;
    }
}
