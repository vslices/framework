using LanguageExt;
using SampleWorkflow.Spaces;
using VSlices.Monads;
using VSlices.Work;

namespace SampleWorkflow.Work;

public sealed class CreateTodo<RT> :
    Feature<CreateTodo<RT>, RT, CreateTodo<RT>.Request, CreateTodo<RT>.Response>
    where RT : HasAlgebra<TodoAlgebra, RT>
{
    public sealed record Request(Todo Todo);

    public sealed record Response(Option<Todo> Todo);

    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => AlgebraEnv<TodoAlgebra, RT>
            .run(TodoPrograms.Create(request.Todo))
            .Map(todo => new Response(todo)));
}
