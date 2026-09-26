using LanguageExt;
using SampleWorkflow.Spaces;
using VSlices.Monads;
using VSlices.Work;

namespace SampleWorkflow.Work;

public sealed class GetTodo :
    Feature<GetTodo, TodoAlgebra, GetTodo.Request, GetTodo.Response>
{
    public sealed record Request(TodoId Id);
    public sealed record Response(Option<Todo> Todo);

    public static Flow<TodoAlgebra, Request, Response> Get() =>
        new((algebra, request) =>
            algebra.Reader.Read(request.Id)
                .Map(todo => new Response(todo)));
}
