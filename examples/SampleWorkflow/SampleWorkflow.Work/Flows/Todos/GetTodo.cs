using LanguageExt;
using SampleWorkflow.Spaces;
using VSlices.Work;
using SampleWorkflow.Work.Algebras;

namespace SampleWorkflow.Work;

public sealed class GetTodo :
    Feature<TodoAlgebra, GetTodo.Request, GetTodo.Response>
{
    public sealed record Request(TodoId Id);
    public sealed record Response(Option<Todo> Todo);

    public static Free<TodoAlgebra, Response> Describe(Request request) =>
        from todo in TodoAlgebra.Read(request.Id)
        select new Response(todo);
}
