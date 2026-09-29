using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices;
using VSlices.Monads;
using VSlices.Work;

namespace SampleWorkflow.Work;

public sealed class GetTodo<RT> :
    Feature<GetTodo<RT>, RT, GetTodo<RT>.Request, GetTodo<RT>.Response>
    where RT : HasAlgebra<GetTodoAlgebra, RT>
{
    public sealed record Request(TodoId Id);
    public sealed record Response(Option<Todo> Todo);

    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => AlgebraEnv<GetTodoAlgebra, RT>
            .run(GetTodoWork.Program(request.Id))
            .Map(todo => new Response(todo)));
}

public static class GetTodoWork
{
    public static Free<GetTodoAlgebra, Option<Todo>> Program(TodoId id) =>
        PointReader.read<GetTodoAlgebra, Todo, TodoId>(id);
}

public abstract record GetTodoWorkPart<A> : K<GetTodoAlgebra, A>;
public sealed record GetTodoReadPart<A>(TodoId Id, Func<Option<Todo>, A> Next) : GetTodoWorkPart<A>;

public sealed class GetTodoAlgebra :
    Functor<GetTodoAlgebra>,
    PointReader<GetTodoAlgebra, Todo, TodoId>
{
    static K<GetTodoAlgebra, Option<Todo>>
        PointReader<GetTodoAlgebra, Todo, TodoId>.Read(TodoId id) =>
        new GetTodoReadPart<Option<Todo>>(id, static point => point);

    static K<GetTodoAlgebra, B> Functor<GetTodoAlgebra>.Map<A, B>(
        Func<A, B> f,
        K<GetTodoAlgebra, A> ma) =>
        ma switch
        {
            GetTodoReadPart<A>(var id, var next) =>
                new GetTodoReadPart<B>(id, point => f(next(point))),
            _ => throw new NotSupportedException()
        };
}
