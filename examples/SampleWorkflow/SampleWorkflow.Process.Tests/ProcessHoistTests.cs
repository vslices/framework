using SampleWorkflow.Grounding;
using SampleWorkflow.Process;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using Xunit;

namespace SampleWorkflow.Process.Tests;

public sealed class ComposedFeatureHoistTests
{
    [Fact]
    public async Task Feature_composes_WorkFlow_algebras_by_hoisting_child_Features()
    {
        var detail = TodoDetail.Transformation
            .RunFin("composed through hoist")
            .ThrowIfFail();

        var service = new InMemoryTodoWork();

        var interpreter =
            new AlgebraSumIO<CreateTodo.Algebra, GetTodo.Algebra>(
                service,
                service);

        var response = await FreeAlgebra
            .interpret(
                CreateAndGetTodo.Get(
                    new CreateAndGetTodo.Request(
                        detail,
                        Completed: false)),
                interpreter)
            .RunAsync();

        var todo = response.Todo
            .Match(
                Left: error => throw error.ToException(),
                Right: maybe => maybe.IfNone(
                    () => throw new InvalidOperationException(
                        "Expected the Process to return the created Todo.")));

        Assert.Equal("composed through hoist", todo.Detail.Value);
        Assert.False(todo.Completed);
    }
}
