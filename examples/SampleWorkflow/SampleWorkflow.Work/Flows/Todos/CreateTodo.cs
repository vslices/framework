using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using VSlices.Work;
using SampleWorkflow.Work.Algebras;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public sealed class CreateTodo :
    Feature<TodoAlgebra, CreateTodo.Request, CreateTodo.Response>
{
    public sealed record Request(TodoDetail Detail, bool Completed);
    public sealed record Response(Either<Error, Option<Todo>> Todo);
    
    public static Free<TodoAlgebra, Response> Describe(Request request) =>
        from id in TodoAlgebra.NextId()
        from response in Todo.Transformation
            .RunFin(new Todo.Input(id, request.Detail, request.Completed))
            .Match(
                Succ: todo =>
                    from current in TodoAlgebra.Read(todo.Id)
                    from created in current.Match(
                        Some: static _ =>
                            TodoAlgebra.Pure(Option<Todo>.None),
                        None: () =>
                            from _ in TodoAlgebra.Write(todo)
                            select Some(todo))
                    select new Response(
                        Either.Right<Error, Option<Todo>>(created)),
                Fail: error =>
                    TodoAlgebra.Pure(
                        new Response(
                            Either.Left<Error, Option<Todo>>(error))))
        select response;
}
