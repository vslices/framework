using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using VSlices.Monads;
using VSlices.Space;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public sealed class UpdateTodo :
    Feature<UpdateTodo, TodoAlgebra, UpdateTodo.Request, UpdateTodo.Response>
{
    public sealed record Request(TodoId Id, TodoDetail Detail, bool Completed);
    public sealed record Response(Either<Error, Option<Todo>> Todo);

    public static Flow<TodoAlgebra, Request, Response> Get() =>
        new((algebra, request) =>
            algebra.Reader.Read(request.Id)
                .Bind(current =>
                    current.Match(
                        Some: todo =>
                            todo.Update(state => state with
                            {
                                Detail = request.Detail,
                                Completed = request.Completed
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
                                            Either.Left<Error, Option<Todo>>(error)))),
                        None: static () =>
                            IO.pure(
                                new Response(
                                    Either.Right<Error, Option<Todo>>(
                                        Option<Todo>.None))))));
}
