using VSlices.Arrows;
using VSlices.Space;
using VSlices.Space.Traits;

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

    public static Req<string, TodoDetail>.Full Transformation =>
        Req<string, TodoDetail>.Transform<string, TodoDetail>(
            value => new TodoDetail(value));

    public override string ToString() =>
        Value;
}
