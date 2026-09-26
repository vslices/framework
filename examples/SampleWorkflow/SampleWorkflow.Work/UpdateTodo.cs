using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices.Monads;
using VSlices.Space;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public sealed class UpdateTodo<RT> :
    Feature<UpdateTodo<RT>, RT, UpdateTodo<RT>.Request, UpdateTodo<RT>.Response>
    where RT : HasAlgebra<UpdateTodoAlgebra, RT>
{
    public sealed record Request(TodoId Id, TodoDetail Detail, bool Completed);
    public sealed record Response(Either<Error, Option<Todo>> Todo);

    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => AlgebraEnv<UpdateTodoAlgebra, RT>
            .run(UpdateTodoWork.Program(request.Id, request.Detail, request.Completed))
            .Map(value => new Response(value)));
}

public static class UpdateTodoWork
{
    public static Free<UpdateTodoAlgebra, Either<Error, Option<Todo>>> Program(
        TodoId id,
        TodoDetail detail,
        bool completed) =>
        from current in PointReader.read<UpdateTodoAlgebra, Todo, TodoId>(id)
        from response in current.Match(
            Some: todo =>
                todo.Update(state => state with
                {
                    Detail = detail,
                    Completed = completed
                })
                .Match(
                    Succ: updated =>
                        from _ in PointWriter.write<UpdateTodoAlgebra, Todo>(updated)
                        select Either.Right<Error, Option<Todo>>(Some(updated)),
                    Fail: error =>
                        Free.pure<UpdateTodoAlgebra, Either<Error, Option<Todo>>>(
                            Either.Left<Error, Option<Todo>>(error))),
            None: static () =>
                Free.pure<UpdateTodoAlgebra, Either<Error, Option<Todo>>>(
                    Either.Right<Error, Option<Todo>>(Option<Todo>.None)))
        select response;
}

public abstract record UpdateTodoWorkPart<A> : K<UpdateTodoAlgebra, A>;
public sealed record UpdateTodoReadPart<A>(TodoId Id, Func<Option<Todo>, A> Next) : UpdateTodoWorkPart<A>;
public sealed record UpdateTodoWritePart<A>(Todo Point, Func<Unit, A> Next) : UpdateTodoWorkPart<A>;

public sealed class UpdateTodoAlgebra :
    Functor<UpdateTodoAlgebra>,
    PointReader<UpdateTodoAlgebra, Todo, TodoId>,
    PointWriter<UpdateTodoAlgebra, Todo>
{
    static K<UpdateTodoAlgebra, Option<Todo>>
        PointReader<UpdateTodoAlgebra, Todo, TodoId>.Read(TodoId id) =>
        new UpdateTodoReadPart<Option<Todo>>(id, static point => point);

    static K<UpdateTodoAlgebra, Unit>
        PointWriter<UpdateTodoAlgebra, Todo>.Write(Todo point) =>
        new UpdateTodoWritePart<Unit>(point, static value => value);

    static K<UpdateTodoAlgebra, B> Functor<UpdateTodoAlgebra>.Map<A, B>(
        Func<A, B> f,
        K<UpdateTodoAlgebra, A> ma) =>
        ma switch
        {
            UpdateTodoReadPart<A>(var id, var next) =>
                new UpdateTodoReadPart<B>(id, point => f(next(point))),
            UpdateTodoWritePart<A>(var point, var next) =>
                new UpdateTodoWritePart<B>(point, value => f(next(value))),
            _ => throw new NotSupportedException()
        };
}
