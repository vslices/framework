using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Grounding;

/// <summary>
/// Fuses the UpdateTodoOptimized Line into one lookup, one semantic evolution,
/// and one in-place dictionary value replacement.
/// </summary>
public sealed class OptimizedUpdateTodoWork :
    AlgebraIO<UpdateTodoOptimized.Algebra>
{
    private readonly Dictionary<Guid, Todo> points = new();

    public int Lookups { get; private set; }
    public int Evolutions { get; private set; }
    public int Writes { get; private set; }

    public void Seed(Todo point) =>
        points[point.Id.Value] = point;

    public Option<Todo> Current(TodoId id) =>
        points.TryGetValue(id.Value, out var point)
            ? Some(point)
            : None;

    public IO<A> Interpret<A>(
        K<UpdateTodoOptimized.Algebra, A> operation) =>
        operation switch
        {
            UpdateTodoOptimized.ExecuteLinePart<A> execute =>
                IO.lift(() => execute.Next(Execute(execute))),
            _ => throw new NotSupportedException()
        };

    private Either<Error, Option<Todo>> Execute<A>(
        UpdateTodoOptimized.ExecuteLinePart<A> execute)
    {
        var step =
            execute.Line
                .Process
                .Flow
                .Step;

        Lookups++;

        ref var current =
            ref CollectionsMarshal.GetValueRefOrNullRef(
                points,
                step.Id.Value);

        if (Unsafe.IsNullRef(ref current))
        {
            return Either.Right<Error, Option<Todo>>(
                Option<Todo>.None);
        }

        Evolutions++;

        Todo? accepted = null;

        var result = execute.Evolve(current)
            .Match<Either<Error, Option<Todo>>>(
                Succ: updated =>
                {
                    accepted = updated;

                    return Either.Right<Error, Option<Todo>>(
                        Some(updated));
                },
                Fail: error =>
                    Either.Left<Error, Option<Todo>>(error));

        if (accepted is not null)
        {
            current = accepted;
            Writes++;
        }

        return result;
    }
}
