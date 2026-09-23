using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices.Work;

namespace SampleWorkflow.Work;

public sealed class GetTodo :
    Feature<GetTodo, GetTodo.Algebra, GetTodo.Request, GetTodo.Response>
{
    public sealed record Request(TodoId Id);
    public sealed record Response(Option<Todo> Todo);

    public abstract record WorkPart<A> : K<Algebra, A>;
    public sealed record ReadPart<A>(TodoId Id, Func<Option<Todo>, A> Next) : WorkPart<A>;

    public sealed class Algebra :
        Functor<Algebra>,
        PointReader<Algebra, Todo, TodoId>
    {
        static K<Algebra, Option<Todo>>
            PointReader<Algebra, Todo, TodoId>.Read(TodoId id) =>
            new ReadPart<Option<Todo>>(id, static point => point);

        static K<Algebra, B> Functor<Algebra>.Map<A, B>(
            Func<A, B> f,
            K<Algebra, A> ma) =>
            ma switch
            {
                ReadPart<A>(var id, var next) =>
                    new ReadPart<B>(id, point => f(next(point))),
                _ => throw new NotSupportedException()
            };
    }

    public static Free<Algebra, Response> Get(Request request) =>
        from todo in PointReader.read<Algebra, Todo, TodoId>(request.Id)
        select new Response(todo);
}
