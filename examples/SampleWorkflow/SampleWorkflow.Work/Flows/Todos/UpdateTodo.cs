using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using VSlices.Space;
using VSlices.Work;
using static LanguageExt.Prelude;
using SampleWorkflow.Work.Algebras;

namespace SampleWorkflow.Work;

public sealed class UpdateTodo :
    Feature<UpdateTodo, 
            TodoAlgebra, 
            UpdateTodo.Request, 
            UpdateTodo.Response>
{
    public sealed record Request(TodoId Id, TodoDetail Detail, bool Completed);
    public sealed record Response(Either<Error, Option<Todo>> Todo);
    
    public static Free<TodoAlgebra, Response> Get(Request request) =>
        from current in PointReader.read<TodoAlgebra, Todo, TodoId>(request.Id)
        from response in current.Match(
            Some: todo =>
                todo.Update(state => state with
                {
                    Detail = request.Detail,
                    Completed = request.Completed
                })
                .Match(
                    Succ: updated =>
                        from _ in PointWriter.write<TodoAlgebra, Todo>(updated)
                        select new Response(
                            Either.Right<Error, Option<Todo>>(Some(updated))),
                    Fail: error =>
                        Free.pure<TodoAlgebra, Response>(
                            new Response(
                                Either.Left<Error, Option<Todo>>(error)))),
            None: static () =>
                Free.pure<TodoAlgebra, Response>(
                    new Response(
                        Either.Right<Error, Option<Todo>>(
                            Option<Todo>.None))))
        select response;
}
