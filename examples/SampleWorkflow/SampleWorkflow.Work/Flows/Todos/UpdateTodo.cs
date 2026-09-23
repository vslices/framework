using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using VSlices.Space;
using VSlices.Work;
using static LanguageExt.Prelude;
using SampleWorkflow.Work.Algebras;

namespace SampleWorkflow.Work;

public sealed class UpdateTodo :
    Feature<TodoAlgebra,
            UpdateTodo.Request,
            UpdateTodo.Response>
{
    public sealed record Request(TodoId Id, TodoDetail Detail, bool Completed);
    public sealed record Response(Either<Error, Option<Todo>> Todo);
    
    public static Free<TodoAlgebra, Response> Describe(Request request) =>
        from current in TodoAlgebra.Read(request.Id)
        from response in current.Match(
            Some: todo =>
                todo.Update(state => state with
                {
                    Detail = request.Detail,
                    Completed = request.Completed
                })
                .Match(
                    Succ: updated =>
                        from _ in TodoAlgebra.Write(updated)
                        select new Response(
                            Either.Right<Error, Option<Todo>>(Some(updated))),
                    Fail: error =>
                        TodoAlgebra.Pure(
                            new Response(
                                Either.Left<Error, Option<Todo>>(error)))),
            None: static () =>
                TodoAlgebra.Pure(
                    new Response(
                        Either.Right<Error, Option<Todo>>(
                            Option<Todo>.None))))
        select response;
}
