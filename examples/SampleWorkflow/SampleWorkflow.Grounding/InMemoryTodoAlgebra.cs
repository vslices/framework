using System.Collections.Concurrent;
using LanguageExt;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Grounding;

public sealed class InMemoryTodoAlgebra : AlgebraIO<TodoAlgebra>
{
    private readonly ConcurrentDictionary<TodoId, Todo> points = new();

    public IO<A> Interpret<A>(K<TodoAlgebra, A> operation) =>
        operation switch
        {
            ReadTodo<A> read =>
                IO.lift(() =>
                    read.Next(
                        points.TryGetValue(read.Id, out var point)
                            ? Some(point)
                            : Option<Todo>.None)),

            WriteTodo<A> write =>
                IO.lift(() =>
                {
                    points[write.Point.Id] = write.Point;
                    return write.Next(unit);
                }),

            RemoveTodo<A> remove =>
                IO.lift(() =>
                {
                    points.TryRemove(remove.Id, out _);
                    return remove.Next(unit);
                }),

            _ => throw new NotSupportedException(
                $"Unknown {nameof(TodoAlgebra)} operation.")
        };
}
