using SampleWorkflow.Grounding;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<InMemoryTodoWork>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/todos", async (CreateTodoBody body, InMemoryTodoWork grounding) =>
{
    var detail = TodoDetail.Transformation.RunFin(body.Detail);

    return await detail.Match<Task<IResult>>(
        Succ: async semanticDetail =>
        {
            var response = await CreateTodo
                .Get()
                .RunFlow(
                    grounding.Algebra,
                    new CreateTodo.Request(
                        semanticDetail,
                        body.Completed))
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

app.MapGet("/todos/{id:guid}", async (Guid id, InMemoryTodoWork grounding) =>
{
    var semanticId = TodoId.Transformation.RunFin(id);

    return await semanticId.Match<Task<IResult>>(
        Succ: async todoId =>
        {
            var response = await GetTodo
                .Get()
                .RunFlow(
                    grounding.Algebra,
                    new GetTodo.Request(todoId))
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
    InMemoryTodoWork grounding) =>
{
    var input =
        from semanticId in TodoId.Transformation.RunFin(id)
        from detail in TodoDetail.Transformation.RunFin(body.Detail)
        select (semanticId, detail);

    return await input.Match<Task<IResult>>(
        Succ: async semantic =>
        {
            var response = await UpdateTodo
                .Get()
                .RunFlow(
                    grounding.Algebra,
                    new UpdateTodo.Request(
                        semantic.semanticId,
                        semantic.detail,
                        body.Completed))
                .RunAsync();

            return response.Todo.Match<IResult>(
                Left: error =>
                    Results.BadRequest(new { error = error.Message }),
                Right: todo =>
                    todo.Match<IResult>(
                        value => Results.Ok(TodoDto.From(value)),
                        () => Results.NotFound()));
        },
        Fail: error =>
            Task.FromResult<IResult>(
                Results.BadRequest(new { error = error.Message })));
});

app.MapDelete("/todos/{id:guid}", async (Guid id, InMemoryTodoWork grounding) =>
{
    var semanticId = TodoId.Transformation.RunFin(id);

    return await semanticId.Match<Task<IResult>>(
        Succ: async todoId =>
        {
            var response = await DeleteTodo
                .Get()
                .RunFlow(
                    grounding.Algebra,
                    new DeleteTodo.Request(todoId))
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

public sealed record CreateTodoBody(string Detail, bool Completed);
public sealed record UpdateTodoBody(string Detail, bool Completed);

public sealed record TodoDto(Guid Id, string Detail, bool Completed)
{
    public static TodoDto From(Todo todo) =>
        new(todo.Id.Value, todo.Detail.Value, todo.Completed);
}
