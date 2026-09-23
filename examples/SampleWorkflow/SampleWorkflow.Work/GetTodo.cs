using LanguageExt;
using SampleWorkflow.Spaces;
using VSlices.Monads;
using VSlices.Work;

namespace SampleWorkflow.Work;

public sealed class GetTodo<RT> :
    Feature<GetTodo<RT>, RT, GetTodo<RT>.Request, GetTodo<RT>.Response>
    where RT : HasAlgebra<TodoAlgebra, RT>
{
    public sealed record Request(TodoId Id);

    public sealed record Response(Option<Todo> Todo);

    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => AlgebraEnv<TodoAlgebra, RT>
            .run(TodoPrograms.Read(request.Id))
            .Map(todo => new Response(todo)));
}
