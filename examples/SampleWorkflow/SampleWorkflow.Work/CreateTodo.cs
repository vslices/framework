using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public sealed class CreateTodo :
    Feature<CreateTodo, CreateTodo.Algebra, CreateTodo.Request, CreateTodo.Response>
{
    public sealed record Request(TodoDetail Detail, bool Completed);
    public sealed record Response(Either<Error, Option<Todo>> Todo);

    public abstract record WorkPart<A> : K<Algebra, A>;

    public sealed record NextIdPart<A>(Func<TodoId, A> Next) : WorkPart<A>;
    public sealed record ReadPart<A>(TodoId Id, Func<Option<Todo>, A> Next) : WorkPart<A>;
    public sealed record WritePart<A>(Todo Point, Func<Unit, A> Next) : WorkPart<A>;

    public sealed class Algebra :
        Functor<Algebra>,
        PointReader<Algebra, Todo, TodoId>,
        PointWriter<Algebra, Todo>
    {
        public static K<Algebra, TodoId> NextId() =>
            new NextIdPart<TodoId>(static value => value);

        static K<Algebra, Option<Todo>>
            PointReader<Algebra, Todo, TodoId>.Read(TodoId id) =>
            new ReadPart<Option<Todo>>(id, static point => point);

        static K<Algebra, Unit>
            PointWriter<Algebra, Todo>.Write(Todo point) =>
            new WritePart<Unit>(point, static value => value);

        static K<Algebra, B> Functor<Algebra>.Map<A, B>(
            Func<A, B> f,
            K<Algebra, A> ma) =>
            ma switch
            {
                NextIdPart<A>(var next) =>
                    new NextIdPart<B>(id => f(next(id))),
                ReadPart<A>(var id, var next) =>
                    new ReadPart<B>(id, point => f(next(point))),
                WritePart<A>(var point, var next) =>
                    new WritePart<B>(point, value => f(next(value))),
                _ => throw new NotSupportedException()
            };
    }

    private static Free<Algebra, TodoId> nextId() =>
        Free.lift(Algebra.NextId());

    public static Free<Algebra, Response> Get(Request request) =>
        from id in nextId()
        from response in Todo.Transformation
            .RunFin(new Todo.Input(id, request.Detail, request.Completed))
            .Match(
                Succ: todo =>
                    from current in PointReader.read<Algebra, Todo, TodoId>(todo.Id)
                    from created in current.Match(
                        Some: static _ =>
                            Free.pure<Algebra, Option<Todo>>(Option<Todo>.None),
                        None: () =>
                            from _ in PointWriter.write<Algebra, Todo>(todo)
                            select Some(todo))
                    select new Response(
                        Either.Right<Error, Option<Todo>>(created)),
                Fail: error =>
                    Free.pure<Algebra, Response>(
                        new Response(
                            Either.Left<Error, Option<Todo>>(error))))
        select response;
}
