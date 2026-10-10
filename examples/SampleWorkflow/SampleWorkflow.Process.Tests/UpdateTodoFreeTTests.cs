using SampleWorkflow.Grounding;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using SampleWorkflow.Work.Algebras;
using VSlices.Space.Traits;
using VSlices.Work;
using Xunit;

namespace SampleWorkflow.Process.Tests;

public sealed class UpdateTodoFreeTTests
{
    [Fact]
    public async Task FreeT_UpdateTodo_composes_read_semantic_evolution_and_write()
    {
        var work = new InMemoryTodoWork();

        var before = Transformable
            .Transform<string, TodoDetail>("before")
            .ThrowIfFail();

        var after = Transformable
            .Transform<string, TodoDetail>("after")
            .ThrowIfFail();

        var created = await FreeAlgebra
            .interpret(
                CreateTodo.Describe(
                    new CreateTodo.Request(
                        before,
                        Completed: false)),
                (AlgebraIO<TodoAlgebra>)work)
            .RunAsync();

        var todo = created.Todo
            .Match(
                Left: error => throw error.ToException(),
                Right: maybe => maybe.IfNone(
                    () => throw new InvalidOperationException(
                        "Expected setup Todo to be created.")));

        var result = await FreeTAlgebra
            .interpret(
                UpdateTodoFreeT.Describe(
                    new UpdateTodoFreeT.Request(
                        todo.Id,
                        after,
                        Completed: true)),
                work)
            .RunAsync();

        var updated = result.ThrowIfFail();

        Assert.Equal(todo.Id, updated.Id);
        Assert.Equal("after", updated.Detail.Value);
        Assert.True(updated.Completed);

        var persisted = await FreeAlgebra
            .interpret(
                GetTodo.Describe(
                    new GetTodo.Request(todo.Id)),
                (AlgebraIO<TodoAlgebra>)work)
            .RunAsync();

        var reread = persisted.Todo.IfNone(
            () => throw new InvalidOperationException(
                "Expected the updated Todo to remain persisted."));

        Assert.Equal(updated.Detail, reread.Detail);
        Assert.Equal(updated.Completed, reread.Completed);
    }

    [Fact]
    public async Task FreeT_UpdateTodo_returns_semantic_failure_when_Todo_is_missing()
    {
        var work = new InMemoryTodoWork();

        var id = Transformable
            .Transform<Guid, TodoId>(Guid.NewGuid())
            .ThrowIfFail();

        var detail = Transformable
            .Transform<string, TodoDetail>("missing")
            .ThrowIfFail();

        var result = await FreeTAlgebra
            .interpret(
                UpdateTodoFreeT.Describe(
                    new UpdateTodoFreeT.Request(
                        id,
                        detail,
                        Completed: true)),
                work)
            .RunAsync();

        Assert.True(result.IsFail);
        Assert.Contains(
            id.ToString(),
            result.Match(
                Succ: static _ => string.Empty,
                Fail: static error => error.Message));
    }
}
