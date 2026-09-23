using VSlices.Space;

namespace SampleWorkflow.Spaces;

public readonly record struct TodoId(Guid Value) : DiscreteSpace<TodoId>;

public sealed record Todo(
    TodoId Id,
    string Title,
    bool Completed) : DiscreteSpace<Todo>;
