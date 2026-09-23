using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices.Work;

namespace SampleWorkflow.Work.Algebras;

public abstract record TodoWorkPart<A> : K<TodoAlgebra, A>;

public sealed record NextTodoIdPart<A>(
    Func<TodoId, A> Next)
    : TodoWorkPart<A>;

public sealed record ReadTodoPart<A>(
    TodoId Id,
    Func<Option<Todo>, A> Next)
    : TodoWorkPart<A>;

public sealed record WriteTodoPart<A>(
    Todo Point,
    Func<Unit, A> Next)
    : TodoWorkPart<A>;

public sealed record RemoveTodoPart<A>(
    TodoId Id, 
    Func<Unit, A> Next) 
    : TodoWorkPart<A>;

public sealed class TodoAlgebra :
    Functor<TodoAlgebra>,
    PointReader<TodoAlgebra, Todo, TodoId>,
    PointWriter<TodoAlgebra, Todo>,
    PointRemover<TodoAlgebra, Todo, TodoId>
{
    public static K<TodoAlgebra, TodoId> NextId() =>
        new NextTodoIdPart<TodoId>(static value => value);

    static K<TodoAlgebra, Option<Todo>>
        PointReader<TodoAlgebra, Todo, TodoId>.Read(TodoId id) =>
        new ReadTodoPart<Option<Todo>>(id, static point => point);

    static K<TodoAlgebra, Unit>
        PointWriter<TodoAlgebra, Todo>.Write(Todo point) =>
        new WriteTodoPart<Unit>(point, static value => value);

    static K<TodoAlgebra, Unit>
        PointRemover<TodoAlgebra, Todo, TodoId>.Remove(TodoId id) =>
        new RemoveTodoPart<Unit>(id, static value => value);

    static K<TodoAlgebra, B> Functor<TodoAlgebra>.Map<A, B>(
        Func<A, B> f,
        K<TodoAlgebra, A> ma) =>
        ma switch
        {
            NextTodoIdPart<A>(var next) =>
                new NextTodoIdPart<B>(id => f(next(id))),
            ReadTodoPart<A>(var id, var next) =>
                new ReadTodoPart<B>(id, point => f(next(point))),
            WriteTodoPart<A>(var point, var next) =>
                new WriteTodoPart<B>(point, value => f(next(value))),
            _ => throw new NotSupportedException()
        };
}
