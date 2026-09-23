using LanguageExt;
using SampleWorkflow.Spaces;
using VSlices.Monads;
using VSlices.Work;

namespace SampleWorkflow.Work;

public sealed class DeleteTodo<RT> :
    Feature<DeleteTodo<RT>, RT, DeleteTodo<RT>.Request, DeleteTodo<RT>.Response>
    where RT : HasAlgebra<TodoAlgebra, RT>
{
    public sealed record Request(TodoId Id);

    public sealed record Response(Option<Todo> Todo);

    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => AlgebraEnv<TodoAlgebra, RT>
            .run(TodoPrograms.Delete(request.Id))
            .Map(todo => new Response(todo)));
}
