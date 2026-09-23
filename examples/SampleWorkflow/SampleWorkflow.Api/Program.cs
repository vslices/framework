using LanguageExt;
using SampleWorkflow.Grounding;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<InMemoryTodoWork>();

builder.Services.AddSingleton<AlgebraIO<CreateTodo.Algebra>>(services =>
    services.GetRequiredService<InMemoryTodoWork>());
builder.Services.AddSingleton<AlgebraIO<GetTodo.Algebra>>(services =>
    services.GetRequiredService<InMemoryTodoWork>());
builder.Services.AddSingleton<AlgebraIO<UpdateTodo.Algebra>>(services =>
    services.GetRequiredService<InMemoryTodoWork>());
builder.Services.AddSingleton<AlgebraIO<DeleteTodo.Algebra>>(services =>
    services.GetRequiredService<InMemoryTodoWork>());

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/todos", async (
    CreateTodoBody body,
    AlgebraIO<CreateTodo.Algebra> interpreter) =>
{
    var detail = TodoDetail.Transformation.RunFin(body.Detail);

    return await detail.Match<Task<IResult>>(
        Succ: async semanticDetail =>
        {
            var response = await FreeAlgebra
                .interpret(
                    CreateTodo.Get(
                        new CreateTodo.Request(
                            semanticDetail,
                            body.Completed)),
                    interpreter)
                .RunAsync();

            return response.Todo.Match<IResult>(
                Left: error =>
                    Results.BadRequest(new { error = error.Message }),
                Right: todo =>
                    todo.Match<IResult>(
                        value => Results.Created(
                            $"/todos/{value.Id.Value}",
                            TodoDto.From(value)),
                        () => Results.Conflict()));
        },
        Fail: error =>
            Task.FromResult<IResult>(
                Results.BadRequest(new { error = error.Message })));
});

app.MapGet("/todos/{id:guid}", async (
    Guid id,
    AlgebraIO<GetTodo.Algebra> interpreter) =>
{
    var semanticId = TodoId.Transformation.RunFin(id);

    return await semanticId.Match<Task<IResult>>(
        Succ: async todoId =>
        {
            var response = await FreeAlgebra
                .interpret(
                    GetTodo.Get(new GetTodo.Request(todoId)),
                    interpreter)
                .RunAsync();

            return response.Todo.Match<IResult>(
                todo => Results.Ok(TodoDto.From(todo)),
                () => Results.NotFound());
        },
        Fail: error =>
            Task.FromResult<IResult>(
                Results.BadRequest(new { error = error.Message })));
});

app.MapPut("/todos/{id:guid}", async (
    Guid id,
    UpdateTodoBody body,
    AlgebraIO<UpdateTodo.Algebra> interpreter) =>
{
    var input =
        from semanticId in TodoId.Transformation.RunFin(id)
        from detail in TodoDetail.Transformation.RunFin(body.Detail)
        select (semanticId, detail);

    return await input.Match<Task<IResult>>(
        Succ: async semantic =>
        {
            var response = await FreeAlgebra
                .interpret(
                    UpdateTodo.Get(
                        new UpdateTodo.Request(
                            semantic.semanticId,
                            semantic.detail,
                            body.Completed)),
                    interpreter)
                .RunAsync();

            return response.Todo.Match<IResult>(
                Left: error =>
                    Results.BadRequest(new { error = error.Message }),
                Right: todo =>
                    todo.Match<IResult>(
                        value => Results.Ok(TodoDto.From(value)),
                        Results.NotFound));
        },
        Fail: error =>
            Task.FromResult<IResult>(
                Results.BadRequest(new { error = error.Message })));
});

app.MapDelete("/todos/{id:guid}", async (
    Guid id,
    AlgebraIO<DeleteTodo.Algebra> interpreter) =>
{
    var semanticId = TodoId.Transformation.RunFin(id);

    return await semanticId.Match<Task<IResult>>(
        Succ: async todoId =>
        {
            var response = await FreeAlgebra
                .interpret(
                    DeleteTodo.Get(new DeleteTodo.Request(todoId)),
                    interpreter)
                .RunAsync();

            return response.Todo.Match<IResult>(
                todo => Results.Ok(TodoDto.From(todo)),
                () => Results.NotFound());
        },
        Fail: error =>
            Task.FromResult<IResult>(
                Results.BadRequest(new { error = error.Message })));
});

app.Run();

public sealed record CreateTodoBody(
    string Detail,
    bool Completed);

public sealed record UpdateTodoBody(
    string Detail,
    bool Completed);

public sealed record TodoDto(
    Guid Id,
    string Detail,
    bool Completed)
{
    public static TodoDto From(Todo todo) =>
        new(
            todo.Id.Value,
            todo.Detail.Value,
            todo.Completed);
}
