using System.Collections.Concurrent;
using LanguageExt;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Grounding;

public sealed class InMemoryTodoWork :
    AlgebraIO<TodoAlgebra>,
    PointReader<Todo, TodoId>,
    PointWriter<Todo>,
    PointRemover<Todo, TodoId>,
    TodoIdSource
{
    private readonly ConcurrentDictionary<TodoId, Todo> points = new();

    public TodoAlgebra Algebra { get; }

    public InMemoryTodoWork() =>
        Algebra = new(
            Reader: this,
            Writer: this,
            Remover: this,
            Ids: this);

    public IO<TodoId> Next() =>
        IO.lift(Guid.NewGuid)
            .Bind(value =>
                TodoId.Transformation
                    .RunFin(value)
                    .Match(
                        Succ: IO.pure,
                        Fail: IO.fail<TodoId>));

    public IO<Option<Todo>> Read(TodoId id) =>
        IO.lift(() =>
            points.TryGetValue(id, out var point)
                ? Some(point)
                : Option<Todo>.None);

    public IO<Unit> Write(Todo point) =>
        IO.lift(() =>
        {
            points[point.Id] = point;
            return unit;
        });

    public IO<Unit> Remove(TodoId id) =>
        IO.lift(() =>
        {
            points.TryRemove(id, out _);
            return unit;
        });
}
