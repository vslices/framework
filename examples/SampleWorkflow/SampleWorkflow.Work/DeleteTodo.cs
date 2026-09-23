using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public sealed class DeleteTodo :
    Feature<DeleteTodo, DeleteTodo.Algebra, DeleteTodo.Request, DeleteTodo.Response>
{
    public sealed record Request(TodoId Id);
    public sealed record Response(Option<Todo> Todo);

    public abstract record WorkPart<A> : K<Algebra, A>;
    public sealed record ReadPart<A>(TodoId Id, Func<Option<Todo>, A> Next) : WorkPart<A>;
    public sealed record RemovePart<A>(TodoId Id, Func<Unit, A> Next) : WorkPart<A>;

    public sealed class Algebra :
        Functor<Algebra>,
        PointReader<Algebra, Todo, TodoId>,
        PointRemover<Algebra, Todo, TodoId>
    {
        static K<Algebra, Option<Todo>>
            PointReader<Algebra, Todo, TodoId>.Read(TodoId id) =>
            new ReadPart<Option<Todo>>(id, static point => point);

        static K<Algebra, Unit>
            PointRemover<Algebra, Todo, TodoId>.Remove(TodoId id) =>
            new RemovePart<Unit>(id, static value => value);

        static K<Algebra, B> Functor<Algebra>.Map<A, B>(
            Func<A, B> f,
            K<Algebra, A> ma) =>
            ma switch
            {
                ReadPart<A>(var id, var next) =>
                    new ReadPart<B>(id, point => f(next(point))),
                RemovePart<A>(var id, var next) =>
                    new RemovePart<B>(id, value => f(next(value))),
                _ => throw new NotSupportedException()
            };
    }

    public static Free<Algebra, Response> Get(Request request) =>
        from current in PointReader.read<Algebra, Todo, TodoId>(request.Id)
        from deleted in current.Match(
            Some: point =>
                from _ in PointRemover.remove<Algebra, Todo, TodoId>(request.Id)
                select Some(point),
            None: static () =>
                Free.pure<Algebra, Option<Todo>>(Option<Todo>.None))
        select new Response(deleted);
}
