using SampleWorkflow.Spaces;
using VSlices.Spaces;
using Xunit;

namespace SampleWorkflow.Process.Tests;

public sealed class EvolvableTransitionTests
{
    [Fact]
    public void Update_passes_previous_and_candidate_states_and_preserves_original_point()
    {
        var id = TodoId.Transform(Guid.NewGuid()).ThrowIfFail();
        var oldDetail = TodoDetail.Transform("before").ThrowIfFail();
        var newDetail = TodoDetail.Transform("after").ThrowIfFail();
        var todo = Todo.Transform(new Todo.Input(id, oldDetail, false)).ThrowIfFail();

        var updated = todo.Update(state => state with { Detail = newDetail }).ThrowIfFail();

        Assert.Equal("before", todo.Detail.Value);
        Assert.Equal("after", updated.Detail.Value);
        Assert.Equal(id.Value, updated.Id.Value);
        Assert.NotSame(todo, updated);
    }

    [Fact]
    public void Evolve_rejects_identity_change_relative_to_previous_state()
    {
        var firstId = TodoId.Transform(Guid.NewGuid()).ThrowIfFail();
        var secondId = TodoId.Transform(Guid.NewGuid()).ThrowIfFail();
        var detail = TodoDetail.Transform("same").ThrowIfFail();
        var first = Todo.Transform(new Todo.Input(firstId, detail, false)).ThrowIfFail();
        var second = Todo.Transform(new Todo.Input(secondId, detail, false)).ThrowIfFail();

        var rejected = Todo.Evolve(first.CurrentState, second.CurrentState);

        Assert.True(rejected.IsFail);
    }
}
