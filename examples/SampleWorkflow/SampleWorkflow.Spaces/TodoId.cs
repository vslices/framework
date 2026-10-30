using LanguageExt;
using VSlices.Spaces;
using VSlices.Spaces.Traits;

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

    public static Fin<TodoId> Transform(Guid value) =>
        new TodoId(value);

    public override string ToString() =>
        Value.ToString();
}
