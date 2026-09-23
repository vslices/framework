using VSlices.Arrows;
using VSlices.Space;
using VSlices.Space.Traits;

namespace SampleWorkflow.Spaces;

/// <summary>
/// Todo semantic point. Equality is owned by its identity.
/// </summary>
public sealed class Todo :
    DiscreteSpace<Todo>,
    Transformable<Todo.Input, Todo>
{
    public readonly record struct Input(
        TodoId Id,
        TodoDetail Detail);

    private Todo(
        TodoId id,
        TodoDetail detail)
    {
        Id = id;
        Detail = detail;
    }

    public TodoId Id { get; }

    public TodoDetail Detail { get; }

    public static Req<Input, Todo>.Full Transformation =>
        Req<Input, Todo>.Transform<Input, Todo>(
            input => new Todo(
                input.Id,
                input.Detail));

    public bool Equals(Todo? other) =>
        Id.Equals(other?.Id);

    public override bool Equals(object? obj) =>
        Equals(obj as Todo);

    public override int GetHashCode() =>
        Id.GetHashCode();

    public static bool operator ==(
        Todo? left,
        Todo? right) =>
        Equals(left, right);

    public static bool operator !=(
        Todo? left,
        Todo? right) =>
        !(left == right);

    public override string ToString() =>
        $"{Id}: {Detail.Title}";
}
