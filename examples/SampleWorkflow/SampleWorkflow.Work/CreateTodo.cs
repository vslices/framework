using LanguageExt;
using SampleWorkflow.Spaces;
using VSlices;
using VSlices.Monads;
using VSlices.Work;

namespace SampleWorkflow.Work;

public sealed class CreateTodo<RT> :
    Feature<CreateTodo<RT>, RT, CreateTodo<RT>.Request, CreateTodo<RT>.Response>
    where RT :
        HasAlgebra<TodoAlgebra, RT>,
        HasTodoIdGeneration<RT>
{
    public sealed record Request(TodoDetail Detail);

    public sealed record Response(Option<Todo> Todo);

    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => TodoIdGenerationEnv<RT>.next
            .Map(id => new Todo.Input(id, request.Detail))) >>
        (input => Todo.Transformation.RunFin(input)) >>
        (todo => AlgebraEnv<TodoAlgebra, RT>
            .run(TodoPrograms.Create(todo))
            .Map(value => new Response(value)));
}
