using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using SampleWorkflow.Work.Algebras;
using VSlices.Work;

namespace SampleWorkflow.Process;

public sealed class CreateAndGetTodo :
    Feature<
        CreateAndGetTodo,
        TodoAlgebra,
        CreateAndGetTodo.Request,
        CreateAndGetTodo.Response>
{
    public sealed record Request(TodoDetail Detail, bool Completed);
    public sealed record Response(Either<Error, Option<Todo>> Todo);

    public static Free<TodoAlgebra, Response> Get(Request request) =>
        from created in CreateTodo.Get(
            new CreateTodo.Request(request.Detail, request.Completed))
        from response in created.Todo.Match(
            Left: error =>
                Free.pure<TodoAlgebra, Response>(
                    new Response(
                        Either.Left<Error, Option<Todo>>(error))),
            Right: maybe =>
                maybe.Match(
                    Some: todo =>
                            GetTodo.Get(
                                new GetTodo.Request(todo.Id))
                            .Map(read =>
                                new Response(
                                    Either.Right<Error, Option<Todo>>(
                                        read.Todo))),
                    None: () =>
                        Free.pure<TodoAlgebra, Response>(
                            new Response(
                                Either.Right<Error, Option<Todo>>(
                                    Option<Todo>.None)))))
        select response;
}
