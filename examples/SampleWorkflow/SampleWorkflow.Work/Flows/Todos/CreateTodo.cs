using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using VSlices.Work;
using SampleWorkflow.Work.Algebras;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public sealed class CreateTodo :
    Feature<CreateTodo, TodoAlgebra, CreateTodo.Request, CreateTodo.Response>
{
    public sealed record Request(TodoDetail Detail, bool Completed);
    public sealed record Response(Either<Error, Option<Todo>> Todo);
    
    public static Free<TodoAlgebra, Response> Get(Request request) =>
        from id in Free.lift(TodoAlgebra.NextId())
        from response in Todo.Transformation
            .RunFin(new Todo.Input(id, request.Detail, request.Completed))
            .Match(
                Succ: todo =>
                    from current in PointReader.read<TodoAlgebra, Todo, TodoId>(todo.Id)
                    from created in current.Match(
                        Some: static _ =>
                            Free.pure<TodoAlgebra, Option<Todo>>(Option<Todo>.None),
                        None: () =>
                            from _ in PointWriter.write<TodoAlgebra, Todo>(todo)
                            select Some(todo))
                    select new Response(
                        Either.Right<Error, Option<Todo>>(created)),
                Fail: error =>
                    Free.pure<TodoAlgebra, Response>(
                        new Response(
                            Either.Left<Error, Option<Todo>>(error))))
        select response;
}
