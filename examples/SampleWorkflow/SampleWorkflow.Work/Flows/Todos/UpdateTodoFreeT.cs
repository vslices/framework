using LanguageExt;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work.Algebras;
using VSlices.Errors;
using VSlices.Space;
using VSlices.Work;

namespace SampleWorkflow.Work;

/// <summary>
/// Experimental UpdateTodo description using FreeT to compose Todo work with Fin semantics.
/// </summary>
public static class UpdateTodoFreeT
{
    public sealed record Request(
        TodoId Id,
        TodoDetail Detail,
        bool Completed);

    public static FreeT<TodoAlgebra, Fin, Todo> Describe(Request request) =>
        from current in FreeT.liftFree<TodoAlgebra, Fin, Option<Todo>>(
            TodoAlgebra.Read(request.Id))
        from todo in current.ToFin(
            new ResourceNotFound(
                $"Todo '{request.Id}' was not found."))
        from updated in todo.Update(
            state => state with
            {
                Detail = request.Detail,
                Completed = request.Completed
            })
        from _ in FreeT.liftFree<TodoAlgebra, Fin, Unit>(
            TodoAlgebra.Write(updated))
        select updated;
}
