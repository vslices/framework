using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Grounding;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using static LanguageExt.Prelude;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<InMemoryTodoWork>();
builder.Services.AddSingleton<ApiRuntime>(services =>
    new ApiRuntime(services.GetRequiredService<InMemoryTodoWork>()));

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/todos", async (CreateTodoBody body, ApiRuntime runtime) =>
{
    var detail = TodoDetail.Transformation.RunFin(body.Detail);
    return await detail.Match<Task<IResult>>(
        Succ: async semanticDetail =>
        {
            var response = await CreateTodo<ApiRuntime>.Get()
                .RunFlow(runtime, new CreateTodo<ApiRuntime>.Request(semanticDetail, body.Completed))
                .RunAsync();
            return response.Todo.Match<IResult>(
                Left: error => Results.BadRequest(new { error = error.Message }),
                Right: todo => todo.Match<IResult>(
                    value => Results.Created($"/todos/{value.Id.Value}", TodoDto.From(value)),
                    () => Results.Conflict()));
        },
        Fail: error => Task.FromResult<IResult>(Results.BadRequest(new { error = error.Message })));
});

app.MapGet("/todos/{id:guid}", async (Guid id, ApiRuntime runtime) =>
{
    var semanticId = TodoId.Transformation.RunFin(id);
    return await semanticId.Match<Task<IResult>>(
        Succ: async todoId =>
        {
            var response = await GetTodo<ApiRuntime>.Get()
                .RunFlow(runtime, new GetTodo<ApiRuntime>.Request(todoId))
                .RunAsync();
            return response.Todo.Match<IResult>(
                todo => Results.Ok(TodoDto.From(todo)),
                () => Results.NotFound());
        },
        Fail: error => Task.FromResult<IResult>(Results.BadRequest(new { error = error.Message })));
});

app.MapPut("/todos/{id:guid}", async (Guid id, UpdateTodoBody body, ApiRuntime runtime) =>
{
    var input =
        from semanticId in TodoId.Transformation.RunFin(id)
        from detail in TodoDetail.Transformation.RunFin(body.Detail)
        select (semanticId, detail);

    return await input.Match<Task<IResult>>(
        Succ: async semantic =>
        {
            var response = await UpdateTodo<ApiRuntime>.Get()
                .RunFlow(runtime, new UpdateTodo<ApiRuntime>.Request(
                    semantic.semanticId, semantic.detail, body.Completed))
                .RunAsync();
            return response.Todo.Match<IResult>(
                Left: error => Results.BadRequest(new { error = error.Message }),
                Right: todo => todo.Match<IResult>(
                    value => Results.Ok(TodoDto.From(value)),
                    () => Results.NotFound()));
        },
        Fail: error => Task.FromResult<IResult>(Results.BadRequest(new { error = error.Message })));
});

app.MapDelete("/todos/{id:guid}", async (Guid id, ApiRuntime runtime) =>
{
    var semanticId = TodoId.Transformation.RunFin(id);
    return await semanticId.Match<Task<IResult>>(
        Succ: async todoId =>
        {
            var response = await DeleteTodo<ApiRuntime>.Get()
                .RunFlow(runtime, new DeleteTodo<ApiRuntime>.Request(todoId))
                .RunAsync();
            return response.Todo.Match<IResult>(
                todo => Results.Ok(TodoDto.From(todo)),
                () => Results.NotFound());
        },
        Fail: error => Task.FromResult<IResult>(Results.BadRequest(new { error = error.Message })));
});

app.Run();

public sealed record ApiRuntime(InMemoryTodoWork Work) :
    HasAlgebra<CreateTodoAlgebra, ApiRuntime>,
    HasAlgebra<GetTodoAlgebra, ApiRuntime>,
    HasAlgebra<UpdateTodoAlgebra, ApiRuntime>,
    HasAlgebra<DeleteTodoAlgebra, ApiRuntime>
{
    static K<Eff<ApiRuntime>, AlgebraIO<CreateTodoAlgebra>>
        Has<Eff<ApiRuntime>, AlgebraIO<CreateTodoAlgebra>>.Ask { get; } =
        liftEff<ApiRuntime, AlgebraIO<CreateTodoAlgebra>>(rt => (AlgebraIO<CreateTodoAlgebra>)rt.Work);

    static K<Eff<ApiRuntime>, AlgebraIO<GetTodoAlgebra>>
        Has<Eff<ApiRuntime>, AlgebraIO<GetTodoAlgebra>>.Ask { get; } =
        liftEff<ApiRuntime, AlgebraIO<GetTodoAlgebra>>(rt => (AlgebraIO<GetTodoAlgebra>)rt.Work);

    static K<Eff<ApiRuntime>, AlgebraIO<UpdateTodoAlgebra>>
        Has<Eff<ApiRuntime>, AlgebraIO<UpdateTodoAlgebra>>.Ask { get; } =
        liftEff<ApiRuntime, AlgebraIO<UpdateTodoAlgebra>>(rt => (AlgebraIO<UpdateTodoAlgebra>)rt.Work);

    static K<Eff<ApiRuntime>, AlgebraIO<DeleteTodoAlgebra>>
        Has<Eff<ApiRuntime>, AlgebraIO<DeleteTodoAlgebra>>.Ask { get; } =
        liftEff<ApiRuntime, AlgebraIO<DeleteTodoAlgebra>>(rt => (AlgebraIO<DeleteTodoAlgebra>)rt.Work);
}

public sealed record CreateTodoBody(string Detail, bool Completed);
public sealed record UpdateTodoBody(string Detail, bool Completed);

public sealed record TodoDto(Guid Id, string Detail, bool Completed)
{
    public static TodoDto From(Todo todo) =>
        new(todo.Id.Value, todo.Detail.Value, todo.Completed);
}
