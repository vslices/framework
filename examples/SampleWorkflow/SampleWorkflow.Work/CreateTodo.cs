using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using VSlices.Monads;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public sealed class CreateTodo :
    Feature<CreateTodo, TodoAlgebra, CreateTodo.Request, CreateTodo.Response>
{
    public sealed record Request(TodoDetail Detail, bool Completed);
    public sealed record Response(Either<Error, Option<Todo>> Todo);

    public static Flow<TodoAlgebra, Request, Response> Get() =>
        new((algebra, request) =>
            algebra.Ids.Next()
                .Bind(id =>
                    Todo.Transformation
                        .RunFin(new Todo.Input(id, request.Detail, request.Completed))
                        .Match(
                            Succ: todo =>
                                algebra.Reader.Read(todo.Id)
                                    .Bind(current =>
                                        current.Match(
                                            Some: static _ =>
                                                IO.pure(
                                                    new Response(
                                                        Either.Right<Error, Option<Todo>>(
                                                            Option<Todo>.None))),
                                            None: () =>
                                                algebra.Writer.Write(todo)
                                                    .Map(_ =>
                                                        new Response(
                                                            Either.Right<Error, Option<Todo>>(
                                                                Some(todo)))))),
                            Fail: error =>
                                IO.pure(
                                    new Response(
                                        Either.Left<Error, Option<Todo>>(error))))));
}
