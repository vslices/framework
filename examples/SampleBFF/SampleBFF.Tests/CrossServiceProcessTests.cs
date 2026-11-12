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

        var detail = TodoDetail.Transform("todo with external file")
            .ThrowIfFail();

        var created = await CreateTodo
            .Get(new CreateTodo.Request(
                    detail,
                    Completed: false)).RunIO(todo.Algebra)
            .RunAsync();

        var createdTodo = created.Todo.Match(
            Left: error => throw error.ToException(),
            Right: maybe => maybe.IfNone(
                () => throw new InvalidOperationException(
                    "Expected setup Todo to be created.")));

        var algebra = new AlgebraMix<FileAlgebra, TodoAlgebra>(
            files.Algebra,
            todo.Algebra);

        var response = await AttachFileToTodo
            .Get(new AttachFileToTodo.Request(
                    createdTodo.Id,
                    "evidence.txt",
                    [1, 2, 3, 4])).RunIO(algebra)
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
            .Get(new GetTodo.Request(createdTodo.Id)).RunIO(todo.Algebra)
            .RunAsync();

        Assert.Single(
            rereadTodo.Todo.IfNone(
                () => throw new InvalidOperationException()).Attachments);

        var rereadFile = await GetFile
            .Get(new GetFile.Request(attached.File.Id)).RunIO(files.Algebra)
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

        var missingTodoId = TodoId.Transform(Guid.NewGuid())
            .ThrowIfFail();

        var algebra = new AlgebraMix<FileAlgebra, TodoAlgebra>(
            files.Algebra,
            todo.Algebra);

        var response = await AttachFileToTodo
            .Get(new AttachFileToTodo.Request(
                    missingTodoId,
                    "orphan.txt",
                    [9, 8, 7])).RunIO(algebra)
            .RunAsync();

        var attachment = response.Attachment.Match(
            Left: error => throw error.ToException(),
            Right: maybe => maybe);

        Assert.True(attachment.IsNone);
        Assert.Equal(1, files.Count);
    }
}
