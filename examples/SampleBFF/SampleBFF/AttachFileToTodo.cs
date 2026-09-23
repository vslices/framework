using LanguageExt;
using LanguageExt.Common;
using SampleFileRepo;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using static LanguageExt.Prelude;
using Algebra = VSlices.Work.AlgebraSum<
    SampleFileRepo.AddFile.Algebra,
    SampleWorkflow.Work.AddAttachmentReference.Algebra>;

namespace SampleBFF;

/// <summary>
/// Cross-service Feature.
///
/// SampleFileRepo owns the stored file.
/// SampleWorkflow owns the Todo-to-resource association.
/// SampleBFF alone knows that a SampleFileId can be represented as a Todo ResourceReference.
/// </summary>
public sealed class AttachFileToTodo :
    Feature<
        Algebra,
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

    public static Free<Algebra, Response> Describe(Request request)
    {
        var addFile = Algebra.FromA(
            AddFile.Describe(
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
                        var associate = Algebra.FromB(
                            AddAttachmentReference.Describe(
                                new AddAttachmentReference.Request(
                                    request.TodoId,
                                    resource)));

                        return
                            from associated in associate
                            from mapped in associated.Todo.Match(
                                Left: error =>
                                    Free.pure<Algebra, Response>(
                                        new Response(
                                            Either.Left<
                                                Error,
                                                Option<Attached>>(error))),
                                Right: maybe =>
                                    maybe.Match(
                                        Some: todo =>
                                            Free.pure<Algebra, Response>(
                                                new Response(
                                                    Either.Right<
                                                        Error,
                                                        Option<Attached>>(
                                                            Some(
                                                                new Attached(
                                                                    todo,
                                                                    stored.File))))),
                                        None: () =>
                                            Free.pure<Algebra, Response>(
                                                new Response(
                                                    Either.Right<
                                                        Error,
                                                        Option<Attached>>(
                                                            Option<Attached>.None)))))
                            select mapped;
                    },
                    Fail: error =>
                        Free.pure<Algebra, Response>(
                            new Response(
                                Either.Left<
                                    Error,
                                    Option<Attached>>(error))))
            select response;
    }
}
