using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices;
using VSlices.Monads;
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

    public static Flow<TodoAlgebra, Request, Response> Get()
    {
        var create = CreateTodo
            .Get()
            .MapRequest((Request request) =>
                new CreateTodo.Request(
                    request.Detail,
                    request.Completed));

        return create.Bind(created =>
            created.Todo.Match(
                Left: error =>
                    Flow<TodoAlgebra, Request>.Pure(
                        new Response(
                            Either.Left<Error, Option<Todo>>(error))),
                Right: maybe =>
                    maybe.Match(
                        Some: todo =>
                            GetTodo
                                .Get()
                                .MapRequest((Request _) =>
                                    new GetTodo.Request(todo.Id))
                                .Map(read =>
                                    new Response(
                                        Either.Right<Error, Option<Todo>>(
                                            read.Todo))),
                        None: () =>
                            Flow<TodoAlgebra, Request>.Pure(
                                new Response(
                                    Either.Right<Error, Option<Todo>>(
                                        Option<Todo>.None))))));
    }
}
