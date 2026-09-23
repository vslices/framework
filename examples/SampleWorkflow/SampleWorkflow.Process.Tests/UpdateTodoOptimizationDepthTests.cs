using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Grounding;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using Xunit;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Process.Tests;

public sealed class UpdateTodoOptimizationDepthTests
{
    [Fact]
    public async Task All_three_variants_preserve_observable_update_semantics()
    {
        var id = TodoId.Transformation
            .RunFin(Guid.Parse("72c40949-4755-465c-a341-80b02795125f"))
            .ThrowIfFail();

        var before = TodoDetail.Transformation
            .RunFin("before")
            .ThrowIfFail();

        var after = TodoDetail.Transformation
            .RunFin("after")
            .ThrowIfFail();

        var original = CreateTodo(id, before, completed: false);

        var baseline = new BaselineUpdateTodoWork(original);
        var optimized = new OptimizedUpdateTodoWork();
        var micro = new MicroOptimizedUpdateTodoWork();

        optimized.Seed(original);
        micro.Seed(original);

        var baselineResponse = await FreeAlgebra
            .interpret(
                UpdateTodo.Get(
                    new UpdateTodo.Request(
                        id,
                        after,
                        Completed: true)),
                baseline)
            .RunAsync();

        var optimizedResponse = await FreeAlgebra
            .interpret(
                UpdateTodoOptimized.Get(
                    new UpdateTodoOptimized.Request(
                        id,
                        after,
                        Completed: true)),
                optimized)
            .RunAsync();

        var microResponse = await FreeAlgebra
            .interpret(
                UpdateTodoMicroOptimized.Get(
                    new UpdateTodoMicroOptimized.Request(
                        id,
                        after,
                        Completed: true)),
                micro)
            .RunAsync();

        var expected = Observe(baselineResponse.Todo);
        var fused = Observe(optimizedResponse.Todo);
        var deepest = Observe(microResponse.Todo);

        Assert.Equal(expected, fused);
        Assert.Equal(expected, deepest);

        Assert.Equal(1, baseline.Reads);
        Assert.Equal(1, baseline.Writes);

        Assert.Equal(1, optimized.Lookups);
        Assert.Equal(1, optimized.Evolutions);
        Assert.Equal(1, optimized.Writes);

        Assert.Equal(1, micro.Lookups);
        Assert.Equal(0, micro.SemanticShortCircuits);
        Assert.Equal(1, micro.Evolutions);
        Assert.Equal(1, micro.Writes);
    }

    [Fact]
    public async Task All_three_variants_preserve_missing_point_semantics()
    {
        var id = TodoId.Transformation
            .RunFin(Guid.Parse("22a5480a-2553-4a56-8e29-f7dd334dce59"))
            .ThrowIfFail();

        var detail = TodoDetail.Transformation
            .RunFin("missing")
            .ThrowIfFail();

        var baseline = new BaselineUpdateTodoWork();
        var optimized = new OptimizedUpdateTodoWork();
        var micro = new MicroOptimizedUpdateTodoWork();

        var baselineResponse = await FreeAlgebra
            .interpret(
                UpdateTodo.Get(
                    new UpdateTodo.Request(
                        id,
                        detail,
                        Completed: true)),
                baseline)
            .RunAsync();

        var optimizedResponse = await FreeAlgebra
            .interpret(
                UpdateTodoOptimized.Get(
                    new UpdateTodoOptimized.Request(
                        id,
                        detail,
                        Completed: true)),
                optimized)
            .RunAsync();

        var microResponse = await FreeAlgebra
            .interpret(
                UpdateTodoMicroOptimized.Get(
                    new UpdateTodoMicroOptimized.Request(
                        id,
                        detail,
                        Completed: true)),
                micro)
            .RunAsync();

        Assert.Equal(
            Observe(baselineResponse.Todo),
            Observe(optimizedResponse.Todo));

        Assert.Equal(
            Observe(baselineResponse.Todo),
            Observe(microResponse.Todo));

        Assert.Equal(1, baseline.Reads);
        Assert.Equal(0, baseline.Writes);

        Assert.Equal(1, optimized.Lookups);
        Assert.Equal(0, optimized.Evolutions);
        Assert.Equal(0, optimized.Writes);

        Assert.Equal(1, micro.Lookups);
        Assert.Equal(0, micro.Evolutions);
        Assert.Equal(0, micro.Writes);
    }

    [Fact]
    public async Task Micro_optimized_variant_can_short_circuit_a_semantically_satisfied_update()
    {
        var id = TodoId.Transformation
            .RunFin(Guid.Parse("dc9380ee-8ddc-413b-b78c-1151570f7e93"))
            .ThrowIfFail();

        var detail = TodoDetail.Transformation
            .RunFin("already there")
            .ThrowIfFail();

        var original = CreateTodo(id, detail, completed: true);

        var baseline = new BaselineUpdateTodoWork(original);
        var optimized = new OptimizedUpdateTodoWork();
        var micro = new MicroOptimizedUpdateTodoWork();

        optimized.Seed(original);
        micro.Seed(original);

        var baselineResponse = await FreeAlgebra
            .interpret(
                UpdateTodo.Get(
                    new UpdateTodo.Request(
                        id,
                        detail,
                        Completed: true)),
                baseline)
            .RunAsync();

        var optimizedResponse = await FreeAlgebra
            .interpret(
                UpdateTodoOptimized.Get(
                    new UpdateTodoOptimized.Request(
                        id,
                        detail,
                        Completed: true)),
                optimized)
            .RunAsync();

        var microResponse = await FreeAlgebra
            .interpret(
                UpdateTodoMicroOptimized.Get(
                    new UpdateTodoMicroOptimized.Request(
                        id,
                        detail,
                        Completed: true)),
                micro)
            .RunAsync();

        Assert.Equal(
            Observe(baselineResponse.Todo),
            Observe(optimizedResponse.Todo));

        Assert.Equal(
            Observe(baselineResponse.Todo),
            Observe(microResponse.Todo));

        Assert.Equal(1, baseline.Reads);
        Assert.Equal(1, baseline.Writes);

        Assert.Equal(1, optimized.Lookups);
        Assert.Equal(1, optimized.Evolutions);
        Assert.Equal(1, optimized.Writes);

        Assert.Equal(1, micro.Lookups);
        Assert.Equal(1, micro.SemanticShortCircuits);
        Assert.Equal(0, micro.Evolutions);
        Assert.Equal(0, micro.Writes);
    }

    private static Todo CreateTodo(
        TodoId id,
        TodoDetail detail,
        bool completed) =>
        Todo.Transformation
            .RunFin(
                new Todo.Input(
                    id,
                    detail,
                    completed))
            .ThrowIfFail();

    private static ObservedTodo Observe(
        Either<LanguageExt.Common.Error, Option<Todo>> result) =>
        result.Match(
            Left: error =>
                new ObservedTodo(
                    Kind: "error",
                    Error: error.Message,
                    Id: null,
                    Detail: null,
                    Completed: null),
            Right: maybe =>
                maybe.Match(
                    Some: todo =>
                        new ObservedTodo(
                            Kind: "todo",
                            Error: null,
                            Id: todo.Id.Value,
                            Detail: todo.Detail.Value,
                            Completed: todo.Completed),
                    None: () =>
                        new ObservedTodo(
                            Kind: "none",
                            Error: null,
                            Id: null,
                            Detail: null,
                            Completed: null)));

    private sealed record ObservedTodo(
        string Kind,
        string? Error,
        Guid? Id,
        string? Detail,
        bool? Completed);
}

public sealed class BaselineUpdateTodoWork :
    AlgebraIO<UpdateTodo.Algebra>
{
    private readonly Dictionary<TodoId, Todo> points = new();

    public BaselineUpdateTodoWork(
        params Todo[] initial)
    {
        foreach (var point in initial)
        {
            points[point.Id] = point;
        }
    }

    public int Reads { get; private set; }
    public int Writes { get; private set; }

    public IO<A> Interpret<A>(
        K<UpdateTodo.Algebra, A> operation) =>
        operation switch
        {
            UpdateTodo.ReadPart<A> read =>
                IO.lift(() =>
                {
                    Reads++;

                    return read.Next(
                        points.TryGetValue(read.Id, out var point)
                            ? Some(point)
                            : Option<Todo>.None);
                }),
            UpdateTodo.WritePart<A> write =>
                IO.lift(() =>
                {
                    Writes++;
                    points[write.Point.Id] = write.Point;
                    return write.Next(unit);
                }),
            _ => throw new NotSupportedException()
        };
}
