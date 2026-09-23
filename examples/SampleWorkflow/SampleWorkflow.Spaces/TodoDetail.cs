using VSlices.Arrows;
using VSlices.Space;
using VSlices.Space.Traits;

namespace SampleWorkflow.Spaces;

/// <summary>
/// Semantic detail carried by a Todo.
/// </summary>
public sealed record TodoDetail :
    DiscreteSpace<TodoDetail>,
    Transformable<TodoDetail.Input, TodoDetail>
{
    public readonly record struct Input(
        string Title,
        bool Completed);

    private TodoDetail(
        string title,
        bool completed)
    {
        Title = title;
        Completed = completed;
    }

    public string Title { get; }

    public bool Completed { get; }

    public static Req<Input, TodoDetail>.Full Transformation =>
        Req<Input, TodoDetail>.Transform<Input, TodoDetail>(
            input => new TodoDetail(
                input.Title,
                input.Completed));
}
