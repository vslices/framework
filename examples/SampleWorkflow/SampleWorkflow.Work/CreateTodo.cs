using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices.Monads;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public sealed class CreateTodo<RT> :
    Feature<CreateTodo<RT>, RT, CreateTodo<RT>.Request, CreateTodo<RT>.Response>
    where RT : HasAlgebra<CreateTodoAlgebra, RT>
{
    public sealed record Request(TodoDetail Detail, bool Completed);
    public sealed record Response(Either<Error, Option<Todo>> Todo);

    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => AlgebraEnv<CreateTodoAlgebra, RT>
            .run(CreateTodoWork.Program(request))
            .Map(static response => response));
}

public static class CreateTodoWork
{
    private static Free<CreateTodoAlgebra, TodoId> nextId() =>
        Free.lift(CreateTodoAlgebra.NextId());

    public static Free<CreateTodoAlgebra, CreateTodo<RT>.Response> Program<RT>(
        CreateTodo<RT>.Request request)
        where RT : HasAlgebra<CreateTodoAlgebra, RT> =>
        from id in nextId()
        from response in Todo.Transformation
            .RunFin(new Todo.Input(id, request.Detail, request.Completed))
            .Match(
                Succ: todo =>
                    from current in PointReader.read<CreateTodoAlgebra, Todo, TodoId>(todo.Id)
                    from created in current.Match(
                        Some: static _ =>
                            Free.pure<CreateTodoAlgebra, Option<Todo>>(Option<Todo>.None),
                        None: () =>
                            from _ in PointWriter.write<CreateTodoAlgebra, Todo>(todo)
                            select Some(todo))
                    select new CreateTodo<RT>.Response(
                        Either.Right<Error, Option<Todo>>(created)),
                Fail: error =>
                    Free.pure<CreateTodoAlgebra, CreateTodo<RT>.Response>(
                        new CreateTodo<RT>.Response(
                            Either.Left<Error, Option<Todo>>(error))))
        select response;
}

public abstract record CreateTodoWorkPart<A> : K<CreateTodoAlgebra, A>;
public sealed record CreateTodoNextIdPart<A>(Func<TodoId, A> Next) : CreateTodoWorkPart<A>;
public sealed record CreateTodoReadPart<A>(TodoId Id, Func<Option<Todo>, A> Next) : CreateTodoWorkPart<A>;
public sealed record CreateTodoWritePart<A>(Todo Point, Func<Unit, A> Next) : CreateTodoWorkPart<A>;

public sealed class CreateTodoAlgebra :
    Functor<CreateTodoAlgebra>,
    PointReader<CreateTodoAlgebra, Todo, TodoId>,
    PointWriter<CreateTodoAlgebra, Todo>
{
    public static K<CreateTodoAlgebra, TodoId> NextId() =>
        new CreateTodoNextIdPart<TodoId>(static value => value);

    static K<CreateTodoAlgebra, Option<Todo>>
        PointReader<CreateTodoAlgebra, Todo, TodoId>.Read(TodoId id) =>
        new CreateTodoReadPart<Option<Todo>>(id, static point => point);

    static K<CreateTodoAlgebra, Unit>
        PointWriter<CreateTodoAlgebra, Todo>.Write(Todo point) =>
        new CreateTodoWritePart<Unit>(point, static value => value);

    static K<CreateTodoAlgebra, B> Functor<CreateTodoAlgebra>.Map<A, B>(
        Func<A, B> f,
        K<CreateTodoAlgebra, A> ma) =>
        ma switch
        {
            CreateTodoNextIdPart<A>(var next) =>
                new CreateTodoNextIdPart<B>(id => f(next(id))),
            CreateTodoReadPart<A>(var id, var next) =>
                new CreateTodoReadPart<B>(id, point => f(next(point))),
            CreateTodoWritePart<A>(var point, var next) =>
                new CreateTodoWritePart<B>(point, value => f(next(value))),
            _ => throw new NotSupportedException()
        };
}
