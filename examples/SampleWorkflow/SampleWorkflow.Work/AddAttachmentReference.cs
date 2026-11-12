using System.Runtime.CompilerServices;
using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using VSlices.Monads;
using VSlices.Spaces;
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

    public sealed record Response
    {
        private Response(Either<Error, Option<Todo>> todo) => Todo = todo;
        public Either<Error, Option<Todo>> Todo { get; }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    private static extern Response NewResponse(Either<Error, Option<Todo>> todo);

    public static Eff<TodoAlgebra, Response> Get(Request request) =>
        Eff<TodoAlgebra, Response>.LiftIO(algebra =>
            algebra.Reader.ReadOrDefault(request.Id).Run().As()
                .Bind(current =>
                    current.Match(
                        Some: todo =>
                        {
                            if (todo.Attachments.Contains(request.Resource))
                            {
                                return IO.pure(
                                    NewResponse(
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
                                                NewResponse(
                                                    Either.Right<Error, Option<Todo>>(
                                                        Some(updated)))),
                                    Fail: error =>
                                        IO.pure(
                                            NewResponse(
                                                Either.Left<Error, Option<Todo>>(
                                                    error))));
                        },
                        None: static () =>
                            IO.pure(
                                NewResponse(
                                    Either.Right<Error, Option<Todo>>(
                                        Option<Todo>.None))))));
}
