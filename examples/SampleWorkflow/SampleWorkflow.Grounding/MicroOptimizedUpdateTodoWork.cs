using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Grounding;

/// <summary>
/// Specialized realization for UpdateTodoMicroOptimized.
///
/// The semantic hierarchy remains visible down to Picostep, while the storage
/// realization uses only the operations required by this update workload.
/// </summary>
public sealed class MicroOptimizedUpdateTodoWork :
    AlgebraIO<UpdateTodoMicroOptimized.Algebra>
{
    private readonly GuidTodoTable points = new();

    public int Lookups { get; private set; }
    public int SemanticShortCircuits { get; private set; }
    public int Evolutions { get; private set; }
    public int Writes { get; private set; }

    public void Seed(Todo point) =>
        points.Set(point.Id.Value, point);

    public Option<Todo> Current(TodoId id)
    {
        var index = points.FindIndex(id.Value);

        return index >= 0
            ? Some(points.ValueAt(index))
            : None;
    }

    public IO<A> Interpret<A>(
        K<UpdateTodoMicroOptimized.Algebra, A> operation) =>
        operation switch
        {
            UpdateTodoMicroOptimized.ExecuteLinePart<A> execute =>
                IO.lift(() => execute.Next(Execute(execute))),
            _ => throw new NotSupportedException()
        };

    private UpdateTodoMicroOptimized.Response Execute<A>(
        UpdateTodoMicroOptimized.ExecuteLinePart<A> execute)
    {
        var quectostep =
            execute.Line
                .Process
                .Flow
                .Step
                .Substep
                .Microstep
                .Nanostep
                .Picostep
                .Femtostep
                .Attostep
                .Zeptostep
                .Yoctostep
                .Rontostep
                .Quectostep;

        Lookups++;

        var index = points.FindIndex(quectostep.Id.Value);

        if (index < 0)
        {
            return new UpdateTodoMicroOptimized.Response(
                Either.Right<Error, Option<Todo>>(
                    Option<Todo>.None));
        }

        var current = points.ValueAt(index);

        if (UpdateTodoMicroOptimized.SatisfiedBy(
            current,
            quectostep))
        {
            SemanticShortCircuits++;

            return new UpdateTodoMicroOptimized.Response(
                Either.Right<Error, Option<Todo>>(
                    Some(current)));
        }

        Evolutions++;

        return UpdateTodoMicroOptimized
            .Evolve(
                current,
                quectostep)
            .Match(
                Succ: updated =>
                {
                    points.Replace(index, updated);
                    Writes++;

                    return new UpdateTodoMicroOptimized.Response(
                        Either.Right<Error, Option<Todo>>(
                            Some(updated)));
                },
                Fail: error =>
                    new UpdateTodoMicroOptimized.Response(
                        Either.Left<Error, Option<Todo>>(error)));
    }

    /// <summary>
    /// Minimal mutable table specialized for Guid -> Todo point updates.
    /// It intentionally does not implement enumeration, removal, querying, or
    /// any Repository-like surface because this experiment does not require them.
    /// </summary>
    private sealed class GuidTodoTable
    {
        private Guid[] keys = new Guid[16];
        private Todo?[] values = new Todo?[16];
        private byte[] occupied = new byte[16];
        private int count;

        public int FindIndex(Guid key)
        {
            var mask = keys.Length - 1;
            var index = Hash(key) & mask;

            for (var probe = 0; probe < keys.Length; probe++)
            {
                if (occupied[index] == 0)
                {
                    return -1;
                }

                if (keys[index] == key)
                {
                    return index;
                }

                index = (index + 1) & mask;
            }

            return -1;
        }

        public Todo ValueAt(int index) =>
            values[index]
            ?? throw new InvalidOperationException(
                "Occupied Todo slot contained no value.");

        public void Replace(int index, Todo value) =>
            values[index] = value;

        public void Set(Guid key, Todo value)
        {
            if ((count + 1) * 10 >= keys.Length * 7)
            {
                Resize();
            }

            InsertOrReplace(key, value);
        }

        private void InsertOrReplace(Guid key, Todo value)
        {
            var mask = keys.Length - 1;
            var index = Hash(key) & mask;

            while (occupied[index] != 0)
            {
                if (keys[index] == key)
                {
                    values[index] = value;
                    return;
                }

                index = (index + 1) & mask;
            }

            keys[index] = key;
            values[index] = value;
            occupied[index] = 1;
            count++;
        }

        private void Resize()
        {
            var oldKeys = keys;
            var oldValues = values;
            var oldOccupied = occupied;

            keys = new Guid[oldKeys.Length * 2];
            values = new Todo?[oldValues.Length * 2];
            occupied = new byte[oldOccupied.Length * 2];
            count = 0;

            for (var index = 0; index < oldKeys.Length; index++)
            {
                if (oldOccupied[index] == 0)
                {
                    continue;
                }

                InsertOrReplace(
                    oldKeys[index],
                    oldValues[index]!);
            }
        }

        private static int Hash(Guid key) =>
            unchecked((int)((uint)key.GetHashCode() * 2654435761u));
    }
}
