using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices.Work;

namespace SampleWorkflow.Work.Algebras;

public sealed record NextTodoIdPart<F, A>(
    Func<TodoId, A> Next)
    : K<F, A>;

public sealed record ReadTodoPart<F, A>(
    TodoId Id,
    Func<Option<Todo>, A> Next)
    : K<F, A>;

public sealed record WriteTodoPart<F, A>(
    Todo Point,
    Func<Unit, A> Next)
    : K<F, A>;

public sealed record RemoveTodoPart<F, A>(
    TodoId Id, 
    Func<Unit, A> Next) 
    : K<F, A>;

public sealed class TodoAlgebra :
    Functor<TodoAlgebra>,
    PointReader<TodoAlgebra, Todo, TodoId>,
    PointWriter<TodoAlgebra, Todo>,
    PointRemover<TodoAlgebra, Todo, TodoId>
{
    public static Free<TodoAlgebra, TodoId> NextId() =>
        Free.lift(
            new NextTodoIdPart<TodoAlgebra, TodoId>(
                static value => value));

    public static Free<TodoAlgebra, Option<Todo>> Read(TodoId id) =>
        PointReader.read<TodoAlgebra, Todo, TodoId>(id);

    public static Free<TodoAlgebra, Unit> Write(Todo point) =>
        PointWriter.write<TodoAlgebra, Todo>(point);

    public static Free<TodoAlgebra, Unit> Remove(TodoId id) =>
        PointRemover.remove<TodoAlgebra, Todo, TodoId>(id);

    public static Free<TodoAlgebra, A> Pure<A>(A value) =>
        Free.pure<TodoAlgebra, A>(value);

    static K<TodoAlgebra, Option<Todo>>
        PointReader<TodoAlgebra, Todo, TodoId>.Read(TodoId id) =>
        new ReadTodoPart<TodoAlgebra, Option<Todo>>(id, static point => point);

    static K<TodoAlgebra, Unit>
        PointWriter<TodoAlgebra, Todo>.Write(Todo point) =>
        new WriteTodoPart<TodoAlgebra, Unit>(point, static value => value);

    static K<TodoAlgebra, Unit>
        PointRemover<TodoAlgebra, Todo, TodoId>.Remove(TodoId id) =>
        new RemoveTodoPart<TodoAlgebra, Unit>(id, static value => value);

    static K<TodoAlgebra, B> Functor<TodoAlgebra>.Map<A, B>(
        Func<A, B> f,
        K<TodoAlgebra, A> ma) =>
        ma switch
        {
            NextTodoIdPart<TodoAlgebra, A>(var next) =>
                new NextTodoIdPart<TodoAlgebra, B>(id => f(next(id))),
            ReadTodoPart<TodoAlgebra, A>(var id, var next) =>
                new ReadTodoPart<TodoAlgebra, B>(id, point => f(next(point))),
            WriteTodoPart<TodoAlgebra, A>(var point, var next) =>
                new WriteTodoPart<TodoAlgebra, B>(point, value => f(next(value))),
            RemoveTodoPart<TodoAlgebra, A>(var id, var next) =>
                new RemoveTodoPart<TodoAlgebra, B>(id, value => f(next(value))),
            _ => throw new NotSupportedException()
        };
}

public sealed class TodoAlgebraOptimized :
    Functor<TodoAlgebraOptimized>,
    PointReader<TodoAlgebraOptimized, Todo, TodoId>,
    PointWriter<TodoAlgebraOptimized, Todo>,
    PointRemover<TodoAlgebraOptimized, Todo, TodoId>
{
    public static K<TodoAlgebraOptimized, TodoId> NextId() =>
        new NextTodoIdPart<TodoAlgebraOptimized, TodoId>(static value => value);

    static K<TodoAlgebraOptimized, Option<Todo>>
        PointReader<TodoAlgebraOptimized, Todo, TodoId>.Read(TodoId id) =>
        new ReadTodoPart<TodoAlgebraOptimized, Option<Todo>>(id, static point => point);

    static K<TodoAlgebraOptimized, Unit>
        PointWriter<TodoAlgebraOptimized, Todo>.Write(Todo point) =>
        new WriteTodoPart<TodoAlgebraOptimized, Unit>(point, static value => value);

    static K<TodoAlgebraOptimized, Unit>
        PointRemover<TodoAlgebraOptimized, Todo, TodoId>.Remove(TodoId id) =>
        new RemoveTodoPart<TodoAlgebraOptimized, Unit>(id, static value => value);

    static K<TodoAlgebraOptimized, B> Functor<TodoAlgebraOptimized>.Map<A, B>(
        Func<A, B> f,
        K<TodoAlgebraOptimized, A> ma) =>
        ma switch
        {
            NextTodoIdPart<TodoAlgebraOptimized, A>(var next) =>
                new NextTodoIdPart<TodoAlgebraOptimized, B>(id => f(next(id))),
            ReadTodoPart<TodoAlgebraOptimized, A>(var id, var next) =>
                new ReadTodoPart<TodoAlgebraOptimized, B>(id, point => f(next(point))),
            WriteTodoPart<TodoAlgebraOptimized, A>(var point, var next) =>
                new WriteTodoPart<TodoAlgebraOptimized, B>(point, value => f(next(value))),
            RemoveTodoPart<TodoAlgebraOptimized, A>(var id, var next) =>
                new RemoveTodoPart<TodoAlgebraOptimized, B>(id, value => f(next(value))),
            _ => throw new NotSupportedException()
        };
}

public sealed class TodoAlgebraMicroOptimized :
    Functor<TodoAlgebraMicroOptimized>,
    PointReader<TodoAlgebraMicroOptimized, Todo, TodoId>,
    PointWriter<TodoAlgebraMicroOptimized, Todo>,
    PointRemover<TodoAlgebraMicroOptimized, Todo, TodoId>
{
    public static K<TodoAlgebraMicroOptimized, TodoId> NextId() =>
        new NextTodoIdPart<TodoAlgebraMicroOptimized, TodoId>(static value => value);

    static K<TodoAlgebraMicroOptimized, Option<Todo>>
        PointReader<TodoAlgebraMicroOptimized, Todo, TodoId>.Read(TodoId id) =>
        new ReadTodoPart<TodoAlgebraMicroOptimized, Option<Todo>>(id, static point => point);

    static K<TodoAlgebraMicroOptimized, Unit>
        PointWriter<TodoAlgebraMicroOptimized, Todo>.Write(Todo point) =>
        new WriteTodoPart<TodoAlgebraMicroOptimized, Unit>(point, static value => value);

    static K<TodoAlgebraMicroOptimized, Unit>
        PointRemover<TodoAlgebraMicroOptimized, Todo, TodoId>.Remove(TodoId id) =>
        new RemoveTodoPart<TodoAlgebraMicroOptimized, Unit>(id, static value => value);

    static K<TodoAlgebraMicroOptimized, B> Functor<TodoAlgebraMicroOptimized>.Map<A, B>(
        Func<A, B> f,
        K<TodoAlgebraMicroOptimized, A> ma) =>
        ma switch
        {
            NextTodoIdPart<TodoAlgebraMicroOptimized, A>(var next) =>
                new NextTodoIdPart<TodoAlgebraMicroOptimized, B>(id => f(next(id))),
            ReadTodoPart<TodoAlgebraMicroOptimized, A>(var id, var next) =>
                new ReadTodoPart<TodoAlgebraMicroOptimized, B>(id, point => f(next(point))),
            WriteTodoPart<TodoAlgebraMicroOptimized, A>(var point, var next) =>
                new WriteTodoPart<TodoAlgebraMicroOptimized, B>(point, value => f(next(value))),
            RemoveTodoPart<TodoAlgebraMicroOptimized, A>(var id, var next) =>
                new RemoveTodoPart<TodoAlgebraMicroOptimized, B>(id, value => f(next(value))),
            _ => throw new NotSupportedException()
        };
}
