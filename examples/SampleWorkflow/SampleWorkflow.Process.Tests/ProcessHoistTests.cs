using LanguageExt;
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
        var detail = TodoDetail.Transform("composed through flow")
            .ThrowIfFail();

        var grounding = new InMemoryTodoWork();

        var response = await CreateAndGetTodo
            .Get(new CreateAndGetTodo.Request(
                    detail,
                    Completed: false)).RunIO(grounding.Algebra)
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
