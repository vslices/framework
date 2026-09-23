using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work.Algebras;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public sealed class DeleteTodo :
    Feature<DeleteTodo, TodoAlgebra, DeleteTodo.Request, DeleteTodo.Response>
{
    public sealed record Request(TodoId Id);
    public sealed record Response(Option<Todo> Todo);

    public static Free<TodoAlgebra, Response> Get(Request request) =>
        from current in PointReader.read<TodoAlgebra, Todo, TodoId>(request.Id)
        from deleted in current.Match(
            Some: point =>
                from _ in PointRemover.remove<TodoAlgebra, Todo, TodoId>(request.Id)
                select Some(point),
            None: static () =>
                Free.pure<TodoAlgebra, Option<Todo>>(Option<Todo>.None))
        select new Response(deleted);
}
