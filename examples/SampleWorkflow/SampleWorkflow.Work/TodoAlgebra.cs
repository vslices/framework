using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices.Work;

namespace SampleWorkflow.Work;

public abstract record TodoOperation<A> : K<TodoAlgebra, A>;

public sealed record ReadTodo<A>(
    TodoId Id,
    Func<Option<Todo>, A> Next)
    : TodoOperation<A>;

public sealed record WriteTodo<A>(
    Todo Point,
    Func<Unit, A> Next)
    : TodoOperation<A>;

public sealed record RemoveTodo<A>(
    TodoId Id,
    Func<Unit, A> Next)
    : TodoOperation<A>;

public sealed class TodoAlgebra :
    Functor<TodoAlgebra>,
    PointReader<TodoAlgebra, Todo, TodoId>,
    PointWriter<TodoAlgebra, Todo>,
    PointRemover<TodoAlgebra, Todo, TodoId>
{
    static K<TodoAlgebra, Option<Todo>>
        PointReader<TodoAlgebra, Todo, TodoId>.Read(TodoId id) =>
        new ReadTodo<Option<Todo>>(id, static point => point);

    static K<TodoAlgebra, Unit>
        PointWriter<TodoAlgebra, Todo>.Write(Todo point) =>
        new WriteTodo<Unit>(point, static value => value);

    static K<TodoAlgebra, Unit>
        PointRemover<TodoAlgebra, Todo, TodoId>.Remove(TodoId id) =>
        new RemoveTodo<Unit>(id, static value => value);

    static K<TodoAlgebra, B> Functor<TodoAlgebra>.Map<A, B>(
        Func<A, B> f,
        K<TodoAlgebra, A> ma) =>
        ma switch
        {
            ReadTodo<A>(var id, var next) =>
                new ReadTodo<B>(id, point => f(next(point))),

            WriteTodo<A>(var point, var next) =>
                new WriteTodo<B>(point, value => f(next(value))),

            RemoveTodo<A>(var id, var next) =>
                new RemoveTodo<B>(id, value => f(next(value))),

            _ => throw new NotSupportedException(
                $"Unknown {nameof(TodoAlgebra)} operation.")
        };
}
