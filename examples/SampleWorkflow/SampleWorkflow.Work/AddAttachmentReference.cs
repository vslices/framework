using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using VSlices.Monads;
using VSlices.Space;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public sealed class AddAttachmentReference :
    Feature<
        AddAttachmentReference,
        TodoAlgebra,
        AddAttachmentReference.Request,
        AddAttachmentReference.Response>
{
    public sealed record Request(
        TodoId Id,
        ResourceReference Resource);

    public sealed record Response(
        Either<Error, Option<Todo>> Todo);

    public static Flow<TodoAlgebra, Request, Response> Get() =>
        new((algebra, request) =>
            algebra.Reader.Read(request.Id)
                .Bind(current =>
                    current.Match(
                        Some: todo =>
                        {
                            if (todo.Attachments.Contains(request.Resource))
                            {
                                return IO.pure(
                                    new Response(
                                        Either.Right<Error, Option<Todo>>(
                                            Some(todo))));
                            }

                            return todo.Update(state => state with
                                {
                                    Attachments =
                                        [.. state.Attachments, request.Resource]
                                })
                                .Match(
                                    Succ: updated =>
                                        algebra.Writer.Write(updated)
                                            .Map(_ =>
                                                new Response(
                                                    Either.Right<Error, Option<Todo>>(
                                                        Some(updated)))),
                                    Fail: error =>
                                        IO.pure(
                                            new Response(
                                                Either.Left<Error, Option<Todo>>(
                                                    error))));
                        },
                        None: static () =>
                            IO.pure(
                                new Response(
                                    Either.Right<Error, Option<Todo>>(
                                        Option<Todo>.None))))));
}
