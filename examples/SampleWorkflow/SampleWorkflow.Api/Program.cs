using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Grounding;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using static LanguageExt.Prelude;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<InMemoryTodoAlgebra>();
builder.Services.AddSingleton<ApiRuntime>(services =>
    new ApiRuntime(services.GetRequiredService<InMemoryTodoAlgebra>()));

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/todos", async (CreateTodoBody body, ApiRuntime runtime) =>
{
    var point = new Todo(
        new TodoId(body.Id),
        body.Title,
        body.Completed);

    var response = await CreateTodo<ApiRuntime>
        .Get()
        .RunFlow(runtime, new CreateTodo<ApiRuntime>.Request(point))
        .RunAsync();

    return response.Todo.Match<IResult>(
        todo => Results.Created(
            $"/todos/{todo.Id.Value}",
            TodoDto.From(todo)),
        () => Results.Conflict());
});

app.MapGet("/todos/{id:guid}", async (Guid id, ApiRuntime runtime) =>
{
    var response = await GetTodo<ApiRuntime>
        .Get()
        .RunFlow(runtime, new GetTodo<ApiRuntime>.Request(new TodoId(id)))
        .RunAsync();

    return response.Todo.Match<IResult>(
        todo => Results.Ok(TodoDto.From(todo)),
        Results.NotFound);
});

app.MapPut("/todos/{id:guid}", async (
    Guid id,
    UpdateTodoBody body,
    ApiRuntime runtime) =>
{
    var point = new Todo(
        new TodoId(id),
        body.Title,
        body.Completed);

    var response = await UpdateTodo<ApiRuntime>
        .Get()
        .RunFlow(runtime, new UpdateTodo<ApiRuntime>.Request(point))
        .RunAsync();

    return response.Todo.Match<IResult>(
        todo => Results.Ok(TodoDto.From(todo)),
        Results.NotFound);
});

app.MapDelete("/todos/{id:guid}", async (Guid id, ApiRuntime runtime) =>
{
    var response = await DeleteTodo<ApiRuntime>
        .Get()
        .RunFlow(runtime, new DeleteTodo<ApiRuntime>.Request(new TodoId(id)))
        .RunAsync();

    return response.Todo.Match<IResult>(
        todo => Results.Ok(TodoDto.From(todo)),
        Results.NotFound);
});

app.Run();

public sealed record ApiRuntime(AlgebraIO<TodoAlgebra> Todo)
    : HasAlgebra<TodoAlgebra, ApiRuntime>
{
    static K<Eff<ApiRuntime>, AlgebraIO<TodoAlgebra>>
        Has<Eff<ApiRuntime>, AlgebraIO<TodoAlgebra>>.Ask { get; } =
        liftEff<ApiRuntime, AlgebraIO<TodoAlgebra>>(runtime => runtime.Todo);
}

public sealed record CreateTodoBody(
    Guid Id,
    string Title,
    bool Completed);

public sealed record UpdateTodoBody(
    string Title,
    bool Completed);

public sealed record TodoDto(
    Guid Id,
    string Title,
    bool Completed)
{
    public static TodoDto From(Todo todo) =>
        new(todo.Id.Value, todo.Title, todo.Completed);
}
