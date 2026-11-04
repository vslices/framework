using ForkExt;
using VSlices.Spaces;
using VSlices.Spaces.Traits;

namespace SampleWorkflow.Spaces;

/// <summary>
/// Semantic textual detail carried by a Todo.
/// </summary>
public sealed record TodoDetail :
    DiscreteSpace<TodoDetail>,
    Transformable<string, TodoDetail>
{
    private TodoDetail(string value) =>
        Value = value;

    public string Value { get; }

    public static Fin<TodoDetail> Transform(string value) =>
        new TodoDetail(value);

    public override string ToString() =>
        Value;
}
