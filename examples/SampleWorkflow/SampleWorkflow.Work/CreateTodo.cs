using System.Runtime.CompilerServices;
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
    public sealed record Response
    {
        private Response(Either<Error, Option<Todo>> todo) => Todo = todo;
        public Either<Error, Option<Todo>> Todo { get; }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    private static extern Response NewResponse(Either<Error, Option<Todo>> todo);

    public static Eff<TodoAlgebra, Response> Get(Request request) =>
        Eff<TodoAlgebra, Response>.LiftIO(algebra =>
            algebra.Ids.Next()
                .Bind(id =>
                    Todo.Transform(new Todo.Input(id, request.Detail, request.Completed))
                        .Match(
                            Succ: todo =>
                                algebra.Reader.ReadOrDefault(todo.Id).Run().As()
                                    .Bind(current =>
                                        current.Match(
                                            Some: static _ =>
                                                IO.pure(
                                                    NewResponse(
                                                        Either.Right<Error, Option<Todo>>(
                                                            Option<Todo>.None))),
                                            None: () =>
                                                algebra.Writer.Write(todo)
                                                    .Map(_ =>
                                                        NewResponse(
                                                            Either.Right<Error, Option<Todo>>(
                                                                Some(todo)))))),
                            Fail: error =>
                                IO.pure(
                                    NewResponse(
                                        Either.Left<Error, Option<Todo>>(error))))));
}
