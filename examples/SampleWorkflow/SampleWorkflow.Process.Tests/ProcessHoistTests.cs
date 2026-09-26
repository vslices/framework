using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Grounding;
using SampleWorkflow.Process;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using Xunit;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Process.Tests;

public sealed class ComposedFeatureFlowTests
{
    [Fact]
    public async Task Feature_composes_child_Features_through_Flow()
    {
        var detail = TodoDetail.Transformation
            .RunFin("composed through flow")
            .ThrowIfFail();

        var service = new InMemoryTodoWork();
        var runtime = new ProcessRuntime(service);

        var response = await CreateAndGetTodo<ProcessRuntime>
            .Get()
            .RunFlow(
                runtime,
                new CreateAndGetTodo<ProcessRuntime>.Request(
                    detail,
                    Completed: false))
            .RunAsync();

        var todo = response.Todo
            .Match(
                Left: error => throw error.ToException(),
                Right: maybe => maybe.IfNone(
                    () => throw new InvalidOperationException(
                        "Expected the Process to return the created Todo.")));

        Assert.Equal("composed through flow", todo.Detail.Value);
        Assert.False(todo.Completed);
    }
}

public sealed record ProcessRuntime(InMemoryTodoWork Work) :
    HasAlgebra<CreateTodoAlgebra, ProcessRuntime>,
    HasAlgebra<GetTodoAlgebra, ProcessRuntime>
{
    static K<Eff<ProcessRuntime>, AlgebraIO<CreateTodoAlgebra>>
        Has<Eff<ProcessRuntime>, AlgebraIO<CreateTodoAlgebra>>.Ask { get; } =
        liftEff<ProcessRuntime, AlgebraIO<CreateTodoAlgebra>>(rt => rt.Work);

    static K<Eff<ProcessRuntime>, AlgebraIO<GetTodoAlgebra>>
        Has<Eff<ProcessRuntime>, AlgebraIO<GetTodoAlgebra>>.Ask { get; } =
        liftEff<ProcessRuntime, AlgebraIO<GetTodoAlgebra>>(rt => rt.Work);
}
