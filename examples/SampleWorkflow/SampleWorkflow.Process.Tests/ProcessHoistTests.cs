using SampleWorkflow.Grounding;
using SampleWorkflow.Process;
using SampleWorkflow.Spaces;
using Xunit;

namespace SampleWorkflow.Process.Tests;

public sealed class ComposedFeatureFlowTests
{
    [Fact]
    public async Task Feature_composes_child_Features_over_the_same_algebra()
    {
        var detail = TodoDetail.Transformation
            .RunFin("composed through flow")
            .ThrowIfFail();

        var grounding = new InMemoryTodoWork();

        var response = await CreateAndGetTodo
            .Get()
            .RunFlow(
                grounding.Algebra,
                new CreateAndGetTodo.Request(
                    detail,
                    Completed: false))
            .RunAsync();

        var todo = response.Todo.Match(
            Left: error => throw error.ToException(),
            Right: maybe => maybe.IfNone(
                () => throw new InvalidOperationException(
                    "Expected the Process to return the created Todo.")));

        Assert.Equal("composed through flow", todo.Detail.Value);
        Assert.False(todo.Completed);
    }
}
