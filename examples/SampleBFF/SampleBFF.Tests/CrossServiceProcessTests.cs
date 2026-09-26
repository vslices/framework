using LanguageExt;
using LanguageExt.Traits;
using SampleFileRepo;
using SampleWorkflow.Grounding;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using Xunit;
using static LanguageExt.Prelude;

namespace SampleBFF.Tests;

public sealed class CrossServiceFeatureCompositionTests
{
    [Fact]
    public async Task BFF_Feature_composes_WorkFlows_owned_by_two_distinct_services()
    {
        var todoWork = new InMemoryTodoWork();
        var fileWork = new InMemoryFileWork();
        var runtime = new BffRuntime(todoWork, fileWork);

        var detail = TodoDetail.Transformation
            .RunFin("todo with external file")
            .ThrowIfFail();

        var created = await CreateTodo<BffRuntime>
            .Get()
            .RunFlow(
                runtime,
                new CreateTodo<BffRuntime>.Request(
                    detail,
                    Completed: false))
            .RunAsync();

        var todo = created.Todo.Match(
            Left: error => throw error.ToException(),
            Right: maybe => maybe.IfNone(
                () => throw new InvalidOperationException(
                    "Expected setup Todo to be created.")));

        var response = await AttachFileToTodo<BffRuntime>
            .Get()
            .RunFlow(
                runtime,
                new AttachFileToTodo<BffRuntime>.Request(
                    todo.Id,
                    "evidence.txt",
                    [1, 2, 3, 4]))
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

        var persistedTodo = await GetTodo<BffRuntime>
            .Get()
            .RunFlow(
                runtime,
                new GetTodo<BffRuntime>.Request(todo.Id))
            .RunAsync();

        var rereadTodo = persistedTodo.Todo.IfNone(
            () => throw new InvalidOperationException(
                "Expected the associated Todo to remain persisted."));

        Assert.Single(rereadTodo.Attachments);
        Assert.Equal(
            attached.File.Id.ToString(),
            rereadTodo.Attachments[0].Value);

        var persistedFile = await GetFile<BffRuntime>
            .Get()
            .RunFlow(
                runtime,
                new GetFile<BffRuntime>.Request(attached.File.Id))
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
        var runtime = new BffRuntime(todoWork, fileWork);

        var missingTodoId = TodoId.Transformation
            .RunFin(Guid.NewGuid())
            .ThrowIfFail();

        var response = await AttachFileToTodo<BffRuntime>
            .Get()
            .RunFlow(
                runtime,
                new AttachFileToTodo<BffRuntime>.Request(
                    missingTodoId,
                    "orphan.txt",
                    [9, 8, 7]))
            .RunAsync();

        var attachment = response.Attachment.Match(
            Left: error => throw error.ToException(),
            Right: maybe => maybe);

        Assert.True(attachment.IsNone);
        Assert.Equal(1, fileWork.Count);
    }
}

public sealed record BffRuntime(
    InMemoryTodoWork TodoWork,
    InMemoryFileWork FileWork) :
    HasAlgebra<CreateTodoAlgebra, BffRuntime>,
    HasAlgebra<GetTodoAlgebra, BffRuntime>,
    HasAlgebra<AddAttachmentReferenceAlgebra, BffRuntime>,
    HasAlgebra<AddFileAlgebra, BffRuntime>,
    HasAlgebra<GetFileAlgebra, BffRuntime>
{
    static K<Eff<BffRuntime>, AlgebraIO<CreateTodoAlgebra>>
        Has<Eff<BffRuntime>, AlgebraIO<CreateTodoAlgebra>>.Ask { get; } =
        liftEff<BffRuntime, AlgebraIO<CreateTodoAlgebra>>(rt => rt.TodoWork);

    static K<Eff<BffRuntime>, AlgebraIO<GetTodoAlgebra>>
        Has<Eff<BffRuntime>, AlgebraIO<GetTodoAlgebra>>.Ask { get; } =
        liftEff<BffRuntime, AlgebraIO<GetTodoAlgebra>>(rt => rt.TodoWork);

    static K<Eff<BffRuntime>, AlgebraIO<AddAttachmentReferenceAlgebra>>
        Has<Eff<BffRuntime>, AlgebraIO<AddAttachmentReferenceAlgebra>>.Ask { get; } =
        liftEff<BffRuntime, AlgebraIO<AddAttachmentReferenceAlgebra>>(rt => rt.TodoWork);

    static K<Eff<BffRuntime>, AlgebraIO<AddFileAlgebra>>
        Has<Eff<BffRuntime>, AlgebraIO<AddFileAlgebra>>.Ask { get; } =
        liftEff<BffRuntime, AlgebraIO<AddFileAlgebra>>(rt => rt.FileWork);

    static K<Eff<BffRuntime>, AlgebraIO<GetFileAlgebra>>
        Has<Eff<BffRuntime>, AlgebraIO<GetFileAlgebra>>.Ask { get; } =
        liftEff<BffRuntime, AlgebraIO<GetFileAlgebra>>(rt => rt.FileWork);
}
