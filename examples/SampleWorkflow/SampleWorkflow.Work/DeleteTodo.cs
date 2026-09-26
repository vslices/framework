using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices;
using VSlices.Monads;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public sealed class DeleteTodo<RT> :
    Feature<DeleteTodo<RT>, RT, DeleteTodo<RT>.Request, DeleteTodo<RT>.Response>
    where RT : HasAlgebra<DeleteTodoAlgebra, RT>
{
    public sealed record Request(TodoId Id);
    public sealed record Response(Option<Todo> Todo);

    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => AlgebraEnv<DeleteTodoAlgebra, RT>
            .run(DeleteTodoWork.Program(request.Id))
            .Map(todo => new Response(todo)));
}

public static class DeleteTodoWork
{
    public static Free<DeleteTodoAlgebra, Option<Todo>> Program(TodoId id) =>
        from current in PointReader.read<DeleteTodoAlgebra, Todo, TodoId>(id)
        from deleted in current.Match(
            Some: point =>
                from _ in PointRemover.remove<DeleteTodoAlgebra, Todo, TodoId>(id)
                select Some(point),
            None: static () =>
                Free.pure<DeleteTodoAlgebra, Option<Todo>>(Option<Todo>.None))
        select deleted;
}

public abstract record DeleteTodoWorkPart<A> : K<DeleteTodoAlgebra, A>;
public sealed record DeleteTodoReadPart<A>(TodoId Id, Func<Option<Todo>, A> Next) : DeleteTodoWorkPart<A>;
public sealed record DeleteTodoRemovePart<A>(TodoId Id, Func<Unit, A> Next) : DeleteTodoWorkPart<A>;

public sealed class DeleteTodoAlgebra :
    Functor<DeleteTodoAlgebra>,
    PointReader<DeleteTodoAlgebra, Todo, TodoId>,
    PointRemover<DeleteTodoAlgebra, Todo, TodoId>
{
    static K<DeleteTodoAlgebra, Option<Todo>>
        PointReader<DeleteTodoAlgebra, Todo, TodoId>.Read(TodoId id) =>
        new DeleteTodoReadPart<Option<Todo>>(id, static point => point);

    static K<DeleteTodoAlgebra, Unit>
        PointRemover<DeleteTodoAlgebra, Todo, TodoId>.Remove(TodoId id) =>
        new DeleteTodoRemovePart<Unit>(id, static value => value);

    static K<DeleteTodoAlgebra, B> Functor<DeleteTodoAlgebra>.Map<A, B>(
        Func<A, B> f,
        K<DeleteTodoAlgebra, A> ma) =>
        ma switch
        {
            DeleteTodoReadPart<A>(var id, var next) =>
                new DeleteTodoReadPart<B>(id, point => f(next(point))),
            DeleteTodoRemovePart<A>(var id, var next) =>
                new DeleteTodoRemovePart<B>(id, value => f(next(value))),
            _ => throw new NotSupportedException()
        };
}
