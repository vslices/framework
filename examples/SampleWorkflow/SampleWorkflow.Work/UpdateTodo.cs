using LanguageExt;
using SampleWorkflow.Spaces;
using VSlices;
using VSlices.Monads;
using VSlices.Work;

namespace SampleWorkflow.Work;

public sealed class UpdateTodo<RT> :
    Feature<UpdateTodo<RT>, RT, UpdateTodo<RT>.Request, UpdateTodo<RT>.Response>
    where RT : HasAlgebra<TodoAlgebra, RT>
{
    public sealed record Request(
        TodoId Id,
        TodoDetail Detail);

    public sealed record Response(Option<Todo> Todo);

    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => Todo.Transformation.RunFin(
            new Todo.Input(
                request.Id,
                request.Detail))) >>
        (todo => AlgebraEnv<TodoAlgebra, RT>
            .run(TodoPrograms.Update(todo))
            .Map(value => new Response(value)));
}
