using System.Collections.Concurrent;
using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices.Work;
using VSlices.Space.Traits;
using SampleWorkflow.Work.Algebras;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Grounding;

public sealed class InMemoryTodoWork : AlgebraIO<TodoAlgebra>
{
    private readonly ConcurrentDictionary<TodoId, Todo> points = new();

    IO<A> AlgebraIO<TodoAlgebra>.Interpret<A>(
        K<TodoAlgebra, A> operation) =>
        operation switch
        {
            NextTodoIdPart<TodoAlgebra, A> next =>
                IO.lift(Guid.NewGuid)
                  .Bind(value => Transformable
                      .Transform<Guid, TodoId>(value)
                      .Match(
                          Succ: id => IO.pure(next.Next(id)),
                          Fail: IO.fail<A>)),
            ReadTodoPart<TodoAlgebra, A> read =>
                IO.lift(() => read.Next(Read(read.Id))),
            WriteTodoPart<TodoAlgebra, A> write =>
                IO.lift(() =>
                {
                    points[write.Point.Id] = write.Point;
                    return write.Next(unit);
                }),
            RemoveTodoPart<TodoAlgebra, A> remove =>
                IO.lift(() =>
                {
                    points.TryRemove(remove.Id, out _);
                    return remove.Next(unit);
                }),
            _ => throw new NotSupportedException()
        };
    
    private Option<Todo> Read(TodoId id) =>
        points.TryGetValue(id, out var point)
            ? Some(point)
            : Option<Todo>.None;
}
