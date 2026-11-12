using System.Runtime.CompilerServices;
using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;

namespace SampleWorkflow.Process;

public sealed class CreateAndGetTodo :
    Feature<CreateAndGetTodo, TodoAlgebra, CreateAndGetTodo.Request, CreateAndGetTodo.Response>
{
    public sealed record Request(TodoDetail Detail, bool Completed);

    public sealed record Response
    {
        private Response(Either<Error, Option<Todo>> todo) => Todo = todo;
        public Either<Error, Option<Todo>> Todo { get; }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    private static extern Response NewResponse(Either<Error, Option<Todo>> todo);

    public static Eff<TodoAlgebra, Response> Get(Request request) =>
        Eff<TodoAlgebra, Response>.LiftIO(algebra =>
            CreateTodo.Get(new CreateTodo.Request(request.Detail, request.Completed))
                .RunIO(algebra)
                .Bind(created =>
                    created.Todo.Match(
                        Left: error =>
                            IO.pure(NewResponse(Either.Left<Error, Option<Todo>>(error))),
                        Right: maybe => maybe.Match(
                            Some: todo =>
                                GetTodo.Get(new GetTodo.Request(todo.Id))
                                    .RunIO(algebra)
                                    .Map(read => NewResponse(
                                        Either.Right<Error, Option<Todo>>(read.Todo))),
                            None: () =>
                                IO.pure(NewResponse(
                                    Either.Right<Error, Option<Todo>>(Option<Todo>.None)))))));
}
