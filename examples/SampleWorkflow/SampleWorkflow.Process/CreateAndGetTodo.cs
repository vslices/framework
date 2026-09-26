using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Monads;
using VSlices.Work;

namespace SampleWorkflow.Process;

public sealed class CreateAndGetTodo<RT> :
    Feature<
        CreateAndGetTodo<RT>,
        RT,
        CreateAndGetTodo<RT>.Request,
        CreateAndGetTodo<RT>.Response>
    where RT :
        HasAlgebra<CreateTodoAlgebra, RT>,
        HasAlgebra<GetTodoAlgebra, RT>
{
    public sealed record Request(TodoDetail Detail, bool Completed);
    public sealed record Response(Either<Error, Option<Todo>> Todo);

    public static Flow<RT, Request, Response> Get() =>
        new((runtime, request) =>
            CreateTodo<RT>
                .Get()
                .RunFlow(
                    runtime,
                    new CreateTodo<RT>.Request(
                        request.Detail,
                        request.Completed))
                .Bind(created =>
                    created.Todo.Match(
                        Left: error =>
                            IO.pure(
                                new Response(
                                    Either.Left<Error, Option<Todo>>(error))),
                        Right: maybe =>
                            maybe.Match(
                                Some: todo =>
                                    GetTodo<RT>
                                        .Get()
                                        .RunFlow(
                                            runtime,
                                            new GetTodo<RT>.Request(todo.Id))
                                        .Map(read =>
                                            new Response(
                                                Either.Right<Error, Option<Todo>>(
                                                    read.Todo))),
                                None: () =>
                                    IO.pure(
                                        new Response(
                                            Either.Right<Error, Option<Todo>>(
                                                Option<Todo>.None)))))));
}
