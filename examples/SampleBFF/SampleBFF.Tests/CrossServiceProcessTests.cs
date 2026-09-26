using SampleFileRepo;
using SampleWorkflow.Grounding;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using Xunit;

namespace SampleBFF.Tests;

public sealed class CrossServiceFeatureCompositionTests
{
    [Fact]
    public async Task BFF_composes_service_algebras_and_adapts_child_Flows()
    {
        var todo = new InMemoryTodoWork();
        var files = new InMemoryFileWork();

        var detail = TodoDetail.Transformation
            .RunFin("todo with external file")
            .ThrowIfFail();

        var created = await CreateTodo
            .Get()
            .RunFlow(
                todo.Algebra,
                new CreateTodo.Request(
                    detail,
                    Completed: false))
            .RunAsync();

        var createdTodo = created.Todo.Match(
            Left: error => throw error.ToException(),
            Right: maybe => maybe.IfNone(
                () => throw new InvalidOperationException(
                    "Expected setup Todo to be created.")));

        var algebra = new AlgebraSum<FileAlgebra, TodoAlgebra>(
            files.Algebra,
            todo.Algebra);

        var response = await AttachFileToTodo
            .Get()
            .RunFlow(
                algebra,
                new AttachFileToTodo.Request(
                    createdTodo.Id,
                    "evidence.txt",
                    [1, 2, 3, 4]))
            .RunAsync();

        var attached = response.Attachment.Match(
            Left: error => throw error.ToException(),
            Right: maybe => maybe.IfNone(
                () => throw new InvalidOperationException(
                    "Expected the BFF to attach the stored file.")));

        Assert.Equal(createdTodo.Id, attached.Todo.Id);
        Assert.Single(attached.Todo.Attachments);
        Assert.Equal(
            attached.File.Id.ToString(),
            attached.Todo.Attachments[0].Value);

        var rereadTodo = await GetTodo
            .Get()
            .RunFlow(
                todo.Algebra,
                new GetTodo.Request(createdTodo.Id))
            .RunAsync();

        Assert.Single(
            rereadTodo.Todo.IfNone(
                () => throw new InvalidOperationException()).Attachments);

        var rereadFile = await GetFile
            .Get()
            .RunFlow(
                files.Algebra,
                new GetFile.Request(attached.File.Id))
            .RunAsync();

        Assert.Equal(
            "evidence.txt",
            rereadFile.File.IfNone(
                () => throw new InvalidOperationException()).Name);
    }

    [Fact]
    public async Task BFF_preserves_existing_partial_failure_behavior()
    {
        var todo = new InMemoryTodoWork();
        var files = new InMemoryFileWork();

        var missingTodoId = TodoId.Transformation
            .RunFin(Guid.NewGuid())
            .ThrowIfFail();

        var algebra = new AlgebraSum<FileAlgebra, TodoAlgebra>(
            files.Algebra,
            todo.Algebra);

        var response = await AttachFileToTodo
            .Get()
            .RunFlow(
                algebra,
                new AttachFileToTodo.Request(
                    missingTodoId,
                    "orphan.txt",
                    [9, 8, 7]))
            .RunAsync();

        var attachment = response.Attachment.Match(
            Left: error => throw error.ToException(),
            Right: maybe => maybe);

        Assert.True(attachment.IsNone);
        Assert.Equal(1, files.Count);
    }
}
