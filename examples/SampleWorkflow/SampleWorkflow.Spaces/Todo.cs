using System.Runtime.CompilerServices;
using VSlices.Arrows;
using VSlices.Space;
using VSlices.Space.Traits;

namespace SampleWorkflow.Spaces;

/// <summary>
/// Todo semantic point. Equality is owned by its identity and accepted state can evolve.
/// </summary>
public sealed class Todo :
    DiscreteSpace<Todo>,
    Transformable<Todo.Input, Todo>,
    Evolvable<Todo, Todo.State>
{
    public readonly record struct Input(
        TodoId Id,
        TodoDetail Detail,
        bool Completed);

    public sealed record State
    {
        private State(
            TodoId id,
            TodoDetail detail,
            bool completed)
        {
            Id = id;
            Detail = detail;
            Completed = completed;
        }

        public TodoId Id { get; }

        public TodoDetail Detail { get; init; }

        public bool Completed { get; init; }
    }

    private Todo(State state) =>
        CurrentState = state;

    public State CurrentState { get; }

    public TodoId Id =>
        CurrentState.Id;

    public TodoDetail Detail =>
        CurrentState.Detail;

    public bool Completed =>
        CurrentState.Completed;

    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    private static extern State NewState(
        TodoId id,
        TodoDetail detail,
        bool completed);

    public static Req<Input, Todo>.Full Transformation =>
        Req<Input, Todo>.Transform<Input, Todo>(
            input => new Todo(
                NewState(
                    input.Id,
                    input.Detail,
                    input.Completed)));

    public static Req<State, Todo>.Full Evolution =>
        Req<State, Todo>.Transform<State, Todo>(
            state => new Todo(state));

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
        $"{Id}: {Detail}";
}
