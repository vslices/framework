using System.Runtime.CompilerServices;
using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using VSlices.Monads;
using VSlices.Spaces;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public sealed class UpdateTodo :
    Feature<UpdateTodo, TodoAlgebra, UpdateTodo.Request, UpdateTodo.Response>
{
    public sealed record Request(TodoId Id, TodoDetail Detail, bool Completed);
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
                            todo.Update(state => state with
                            {
                                Detail = request.Detail,
                                Completed = request.Completed
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
                                            Either.Left<Error, Option<Todo>>(error)))),
                        None: static () =>
                            IO.pure(
                                NewResponse(
                                    Either.Right<Error, Option<Todo>>(
                                        Option<Todo>.None))))));
}
