using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work.Algebras;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public sealed class DeleteTodo :
    Feature<TodoAlgebra, DeleteTodo.Request, DeleteTodo.Response>
{
    public sealed record Request(TodoId Id);
    public sealed record Response(Option<Todo> Todo);

    public static Free<TodoAlgebra, Response> Describe(Request request) =>
        from current in TodoAlgebra.Read(request.Id)
        from deleted in current.Match(
            Some: point =>
                from _ in TodoAlgebra.Remove(request.Id)
                select Some(point),
            None: static () =>
                TodoAlgebra.Pure(Option<Todo>.None))
        select new Response(deleted);
}
