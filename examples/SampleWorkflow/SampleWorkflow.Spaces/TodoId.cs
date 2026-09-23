using VSlices.Arrows;
using VSlices.Space;
using VSlices.Space.Traits;

namespace SampleWorkflow.Spaces;

/// <summary>
/// Semantic identity of a Todo point.
/// </summary>
public sealed record TodoId :
    DiscreteSpace<TodoId>,
    Transformable<Guid, TodoId>
{
    private TodoId(Guid value) =>
        Value = value;

    public Guid Value { get; }

    public static Req<Guid, TodoId>.Full Transformation =>
        Req<Guid, TodoId>.Transform<Guid, TodoId>(
            value => new TodoId(value));

    public override string ToString() =>
        Value.ToString();
}
