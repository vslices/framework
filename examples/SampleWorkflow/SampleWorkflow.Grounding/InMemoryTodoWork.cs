using System.Collections.Concurrent;
using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Grounding;

public sealed class InMemoryTodoWork :
    AlgebraIO<CreateTodoAlgebra>,
    AlgebraIO<GetTodoAlgebra>,
    AlgebraIO<UpdateTodoAlgebra>,
    AlgebraIO<DeleteTodoAlgebra>,
    AlgebraIO<AddAttachmentReferenceAlgebra>
{
    private readonly ConcurrentDictionary<TodoId, Todo> points = new();

    IO<A> AlgebraIO<CreateTodoAlgebra>.Interpret<A>(
        K<CreateTodoAlgebra, A> operation) =>
        operation switch
        {
            CreateTodoNextIdPart<A> next =>
                IO.lift(Guid.NewGuid)
                    .Bind(value => TodoId.Transformation
                        .RunFin(value)
                        .Match(
                            Succ: id => IO.pure(next.Next(id)),
                            Fail: IO.fail<A>)),
            CreateTodoReadPart<A> read =>
                IO.lift(() => read.Next(Read(read.Id))),
            CreateTodoWritePart<A> write =>
                IO.lift(() =>
                {
                    points[write.Point.Id] = write.Point;
                    return write.Next(unit);
                }),
            _ => throw new NotSupportedException()
        };

    IO<A> AlgebraIO<GetTodoAlgebra>.Interpret<A>(
        K<GetTodoAlgebra, A> operation) =>
        operation switch
        {
            GetTodoReadPart<A> read =>
                IO.lift(() => read.Next(Read(read.Id))),
            _ => throw new NotSupportedException()
        };

    IO<A> AlgebraIO<UpdateTodoAlgebra>.Interpret<A>(
        K<UpdateTodoAlgebra, A> operation) =>
        operation switch
        {
            UpdateTodoReadPart<A> read =>
                IO.lift(() => read.Next(Read(read.Id))),
            UpdateTodoWritePart<A> write =>
                IO.lift(() =>
                {
                    points[write.Point.Id] = write.Point;
                    return write.Next(unit);
                }),
            _ => throw new NotSupportedException()
        };

    IO<A> AlgebraIO<DeleteTodoAlgebra>.Interpret<A>(
        K<DeleteTodoAlgebra, A> operation) =>
        operation switch
        {
            DeleteTodoReadPart<A> read =>
                IO.lift(() => read.Next(Read(read.Id))),
            DeleteTodoRemovePart<A> remove =>
                IO.lift(() =>
                {
                    points.TryRemove(remove.Id, out _);
                    return remove.Next(unit);
                }),
            _ => throw new NotSupportedException()
        };

    IO<A> AlgebraIO<AddAttachmentReferenceAlgebra>.Interpret<A>(
        K<AddAttachmentReferenceAlgebra, A> operation) =>
        operation switch
        {
            AddAttachmentReferenceReadPart<A> read =>
                IO.lift(() => read.Next(Read(read.Id))),
            AddAttachmentReferenceWritePart<A> write =>
                IO.lift(() =>
                {
                    points[write.Point.Id] = write.Point;
                    return write.Next(unit);
                }),
            _ => throw new NotSupportedException()
        };

    private Option<Todo> Read(TodoId id) =>
        points.TryGetValue(id, out var point)
            ? Some(point)
            : Option<Todo>.None;
}
