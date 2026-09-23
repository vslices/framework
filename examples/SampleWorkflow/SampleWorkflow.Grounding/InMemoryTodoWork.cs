using System.Collections.Concurrent;
using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Grounding;

public sealed class InMemoryTodoWork :
    AlgebraIO<CreateTodo.Algebra>,
    AlgebraIO<GetTodo.Algebra>,
    AlgebraIO<UpdateTodo.Algebra>,
    AlgebraIO<DeleteTodo.Algebra>
{
    private readonly ConcurrentDictionary<TodoId, Todo> points = new();

    IO<A> AlgebraIO<CreateTodo.Algebra>.Interpret<A>(
        K<CreateTodo.Algebra, A> operation) =>
        operation switch
        {
            CreateTodo.NextIdPart<A> next =>
                IO.lift(Guid.NewGuid)
                    .Bind(value => TodoId.Transformation
                        .RunFin(value)
                        .Match(
                            Succ: id => IO.pure(next.Next(id)),
                            Fail: IO.fail<A>)),
            CreateTodo.ReadPart<A> read =>
                IO.lift(() => read.Next(Read(read.Id))),
            CreateTodo.WritePart<A> write =>
                IO.lift(() =>
                {
                    points[write.Point.Id] = write.Point;
                    return write.Next(unit);
                }),
            _ => throw new NotSupportedException()
        };

    IO<A> AlgebraIO<GetTodo.Algebra>.Interpret<A>(
        K<GetTodo.Algebra, A> operation) =>
        operation switch
        {
            GetTodo.ReadPart<A> read =>
                IO.lift(() => read.Next(Read(read.Id))),
            _ => throw new NotSupportedException()
        };

    IO<A> AlgebraIO<UpdateTodo.Algebra>.Interpret<A>(
        K<UpdateTodo.Algebra, A> operation) =>
        operation switch
        {
            UpdateTodo.ReadPart<A> read =>
                IO.lift(() => read.Next(Read(read.Id))),
            UpdateTodo.WritePart<A> write =>
                IO.lift(() =>
                {
                    points[write.Point.Id] = write.Point;
                    return write.Next(unit);
                }),
            _ => throw new NotSupportedException()
        };

    IO<A> AlgebraIO<DeleteTodo.Algebra>.Interpret<A>(
        K<DeleteTodo.Algebra, A> operation) =>
        operation switch
        {
            DeleteTodo.ReadPart<A> read =>
                IO.lift(() => read.Next(Read(read.Id))),
            DeleteTodo.RemovePart<A> remove =>
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
