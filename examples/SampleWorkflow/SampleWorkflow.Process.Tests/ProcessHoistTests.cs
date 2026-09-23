using SampleWorkflow.Grounding;
using SampleWorkflow.Process;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using SampleWorkflow.Work.Algebras;
using VSlices.Work;
using Xunit;

namespace SampleWorkflow.Process.Tests;

public sealed class ComposedFeatureModuleAlgebraTests
{
    [Fact]
    public async Task Feature_composes_child_WorkFlows_in_the_shared_module_algebra()
    {
        var detail = TodoDetail.Transformation
            .RunFin("composed through shared module algebra")
            .ThrowIfFail();

        var service = new InMemoryTodoWork();

        var response = await FreeAlgebra
            .interpret(
                CreateAndGetTodo.Describe(
                    new CreateAndGetTodo.Request(
                        detail,
                        Completed: false)),
                (AlgebraIO<TodoAlgebra>)service)
            .RunAsync();

        var todo = response.Todo
            .Match(
                Left: error => throw error.ToException(),
                Right: maybe => maybe.IfNone(
                    () => throw new InvalidOperationException(
                        "Expected the Process to return the created Todo.")));

        Assert.Equal("composed through shared module algebra", todo.Detail.Value);
        Assert.False(todo.Completed);
    }
}
