using SampleFileRepo;
using SampleWorkflow.Grounding;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using Xunit;

namespace SampleBFF.Tests;

public sealed class CrossServiceProcessTests
{
    [Fact]
    public async Task BFF_composes_WorkFlows_owned_by_two_distinct_services()
    {
        var todoWork = new InMemoryTodoWork();
        var fileWork = new InMemoryFileWork();

        var detail = TodoDetail.Transformation
            .RunFin("todo with external file")
            .ThrowIfFail();

        var created = await FreeAlgebra
            .interpret(
                CreateTodo.Get(
                    new CreateTodo.Request(
                        detail,
                        Completed: false)),
                (AlgebraIO<CreateTodo.Algebra>)todoWork)
            .RunAsync();

        var todo = created.Todo.Match(
            Left: error => throw error.ToException(),
            Right: maybe => maybe.IfNone(
                () => throw new InvalidOperationException(
                    "Expected setup Todo to be created.")));

        var processInterpreter =
            new AlgebraSumIO<
                AddFile.Algebra,
                AddAttachmentReference.Algebra>(
                fileWork,
                todoWork);

        var response = await FreeAlgebra
            .interpret(
                AttachFileToTodo.Get(
                    new AttachFileToTodo.Request(
                        todo.Id,
                        "evidence.txt",
                        [1, 2, 3, 4])),
                processInterpreter)
            .RunAsync();

        var attached = response.Attachment.Match(
            Left: error => throw error.ToException(),
            Right: maybe => maybe.IfNone(
                () => throw new InvalidOperationException(
                    "Expected the BFF to attach the stored file.")));

        Assert.Equal(todo.Id, attached.Todo.Id);
        Assert.Single(attached.Todo.Attachments);
        Assert.Equal(
            attached.File.Id.ToString(),
            attached.Todo.Attachments[0].Value);

        var persistedTodo = await FreeAlgebra
            .interpret(
                GetTodo.Get(
                    new GetTodo.Request(todo.Id)),
                (AlgebraIO<GetTodo.Algebra>)todoWork)
            .RunAsync();

        var rereadTodo = persistedTodo.Todo.IfNone(
            () => throw new InvalidOperationException(
                "Expected the associated Todo to remain persisted."));

        Assert.Single(rereadTodo.Attachments);
        Assert.Equal(
            attached.File.Id.ToString(),
            rereadTodo.Attachments[0].Value);

        var persistedFile = await FreeAlgebra
            .interpret(
                GetFile.Get(
                    new GetFile.Request(attached.File.Id)),
                (AlgebraIO<GetFile.Algebra>)fileWork)
            .RunAsync();

        var rereadFile = persistedFile.File.IfNone(
            () => throw new InvalidOperationException(
                "Expected the file to remain persisted."));

        Assert.Equal("evidence.txt", rereadFile.Name);
        Assert.Equal([1, 2, 3, 4], rereadFile.Content);
    }

    [Fact]
    public async Task BFF_currently_leaves_the_file_stored_when_the_second_WorkFlow_cannot_associate()
    {
        var todoWork = new InMemoryTodoWork();
        var fileWork = new InMemoryFileWork();

        var missingTodoId = TodoId.Transformation
            .RunFin(Guid.NewGuid())
            .ThrowIfFail();

        var processInterpreter =
            new AlgebraSumIO<
                AddFile.Algebra,
                AddAttachmentReference.Algebra>(
                fileWork,
                todoWork);

        var response = await FreeAlgebra
            .interpret(
                AttachFileToTodo.Get(
                    new AttachFileToTodo.Request(
                        missingTodoId,
                        "orphan.txt",
                        [9, 8, 7])),
                processInterpreter)
            .RunAsync();

        var attachment = response.Attachment.Match(
            Left: error => throw error.ToException(),
            Right: maybe => maybe);

        Assert.True(attachment.IsNone);
        Assert.Equal(1, fileWork.Count);
    }
}
