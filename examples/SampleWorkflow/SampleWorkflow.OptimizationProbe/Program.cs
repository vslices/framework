using System.Diagnostics;
using System.Runtime.CompilerServices;
using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Grounding;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using static LanguageExt.Prelude;

const int iterations = 20_000;
const int warmup = 500;

var id = TodoId.Transformation
    .RunFin(Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"))
    .ThrowIfFail();

var detailA = TodoDetail.Transformation
    .RunFin("A")
    .ThrowIfFail();

var detailB = TodoDetail.Transformation
    .RunFin("B")
    .ThrowIfFail();

Console.WriteLine("=== structural size probe ===");
PrintSize<UpdateTodoMicroOptimized.Quectostep>("Quectostep");
PrintSize<UpdateTodoMicroOptimized.Rontostep>("Rontostep");
PrintSize<UpdateTodoMicroOptimized.Yoctostep>("Yoctostep");
PrintSize<UpdateTodoMicroOptimized.Zeptostep>("Zeptostep");
PrintSize<UpdateTodoMicroOptimized.Attostep>("Attostep");
PrintSize<UpdateTodoMicroOptimized.Femtostep>("Femtostep");
PrintSize<UpdateTodoMicroOptimized.Picostep>("Picostep");
PrintSize<UpdateTodoMicroOptimized.Nanostep>("Nanostep");
PrintSize<UpdateTodoMicroOptimized.Microstep>("Microstep");
PrintSize<UpdateTodoMicroOptimized.Substep>("Substep");
PrintSize<UpdateTodoMicroOptimized.Step>("Step");
PrintSize<UpdateTodoMicroOptimized.Flow>("Flow");
PrintSize<UpdateTodoMicroOptimized.Process>("Process");
PrintSize<UpdateTodoMicroOptimized.Line>("Line");
PrintSize<MicroOptimizedUpdateTodoWork.CompiledLine>("CompiledLine");
Console.WriteLine();

await WarmUp();

Console.WriteLine($"=== alternating state ({iterations:N0} executions) ===");
await MeasureAlternating();

Console.WriteLine();
Console.WriteLine($"=== stable intention ({iterations:N0} executions) ===");
await MeasureStable();

Console.WriteLine();
Console.WriteLine($"=== stable intention / prebuilt program ({iterations:N0} executions) ===");
await MeasureStablePrebuiltProgram();

Console.WriteLine();
Console.WriteLine($"=== stable intention / preinterpreted IO ({iterations:N0} executions) ===");
await MeasureStablePreinterpreted();

Console.WriteLine();
Console.WriteLine($"=== compiled micro hot path ({iterations:N0} executions) ===");
MeasureCompiledMicro();

return;

static void PrintSize<T>(string name)
    where T : struct =>
    Console.WriteLine($"{name,-12} {Unsafe.SizeOf<T>(),4} bytes");

async Task WarmUp()
{
    var original = CreateTodo(id, detailA, completed: false);

    var baseline = new ProbeBaselineUpdateTodoWork(original);

    var optimized = new OptimizedUpdateTodoWork();
    optimized.Seed(original);

    var micro = new MicroOptimizedUpdateTodoWork();
    micro.Seed(original);

    for (var index = 0; index < warmup; index++)
    {
        var target = (index & 1) == 0 ? detailB : detailA;
        var completed = (index & 1) == 0;

        await ExecuteBaseline(baseline, id, target, completed);
        await ExecuteOptimized(optimized, id, target, completed);
        await ExecuteMicro(micro, id, target, completed);
    }
}

async Task MeasureAlternating()
{
    var original = CreateTodo(id, detailA, completed: false);

    var baseline = new ProbeBaselineUpdateTodoWork(original);
    var optimized = new OptimizedUpdateTodoWork();
    var micro = new MicroOptimizedUpdateTodoWork();

    optimized.Seed(original);
    micro.Seed(original);

    await Measure(
        "baseline",
        async () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                var target = (index & 1) == 0 ? detailB : detailA;
                var completed = (index & 1) == 0;
                await ExecuteBaseline(baseline, id, target, completed);
            }
        });

    await Measure(
        "optimized",
        async () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                var target = (index & 1) == 0 ? detailB : detailA;
                var completed = (index & 1) == 0;
                await ExecuteOptimized(optimized, id, target, completed);
            }
        });

    await Measure(
        "micro",
        async () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                var target = (index & 1) == 0 ? detailB : detailA;
                var completed = (index & 1) == 0;
                await ExecuteMicro(micro, id, target, completed);
            }
        });

    Console.WriteLine(
        $"baseline ops : reads={baseline.Reads:N0} writes={baseline.Writes:N0}");

    Console.WriteLine(
        $"optimized ops: lookups={optimized.Lookups:N0} evolutions={optimized.Evolutions:N0} writes={optimized.Writes:N0}");

    Console.WriteLine(
        $"micro ops    : lookups={micro.Lookups:N0} short-circuits={micro.SemanticShortCircuits:N0} evolutions={micro.Evolutions:N0} writes={micro.Writes:N0}");
}

async Task MeasureStable()
{
    var original = CreateTodo(id, detailA, completed: false);

    var baseline = new ProbeBaselineUpdateTodoWork(original);
    var optimized = new OptimizedUpdateTodoWork();
    var micro = new MicroOptimizedUpdateTodoWork();

    optimized.Seed(original);
    micro.Seed(original);

    await Measure(
        "baseline",
        async () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                await ExecuteBaseline(
                    baseline,
                    id,
                    detailB,
                    completed: true);
            }
        });

    await Measure(
        "optimized",
        async () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                await ExecuteOptimized(
                    optimized,
                    id,
                    detailB,
                    completed: true);
            }
        });

    await Measure(
        "micro",
        async () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                await ExecuteMicro(
                    micro,
                    id,
                    detailB,
                    completed: true);
            }
        });

    Console.WriteLine(
        $"baseline ops : reads={baseline.Reads:N0} writes={baseline.Writes:N0}");

    Console.WriteLine(
        $"optimized ops: lookups={optimized.Lookups:N0} evolutions={optimized.Evolutions:N0} writes={optimized.Writes:N0}");

    Console.WriteLine(
        $"micro ops    : lookups={micro.Lookups:N0} short-circuits={micro.SemanticShortCircuits:N0} evolutions={micro.Evolutions:N0} writes={micro.Writes:N0}");
}

async Task MeasureStablePrebuiltProgram()
{
    var original = CreateTodo(id, detailA, completed: false);

    var baseline = new ProbeBaselineUpdateTodoWork(original);
    var optimized = new OptimizedUpdateTodoWork();
    var micro = new MicroOptimizedUpdateTodoWork();

    optimized.Seed(original);
    micro.Seed(original);

    var baselineProgram =
        UpdateTodo.Get(
            new UpdateTodo.Request(
                id,
                detailB,
                Completed: true));

    var optimizedProgram =
        UpdateTodoOptimized.Get(
            new UpdateTodoOptimized.Request(
                id,
                detailB,
                Completed: true));

    var microProgram =
        UpdateTodoMicroOptimized.Get(
            new UpdateTodoMicroOptimized.Request(
                id,
                detailB,
                Completed: true));

    await Measure(
        "baseline",
        async () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                _ = await FreeAlgebra
                    .interpret(baselineProgram, baseline)
                    .RunAsync();
            }
        });

    await Measure(
        "optimized",
        async () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                _ = await FreeAlgebra
                    .interpret(optimizedProgram, optimized)
                    .RunAsync();
            }
        });

    await Measure(
        "micro",
        async () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                _ = await FreeAlgebra
                    .interpret(microProgram, micro)
                    .RunAsync();
            }
        });
}

async Task MeasureStablePreinterpreted()
{
    var original = CreateTodo(id, detailA, completed: false);

    var baseline = new ProbeBaselineUpdateTodoWork(original);
    var optimized = new OptimizedUpdateTodoWork();
    var micro = new MicroOptimizedUpdateTodoWork();

    optimized.Seed(original);
    micro.Seed(original);

    var baselineIO =
        FreeAlgebra.interpret(
            UpdateTodo.Get(
                new UpdateTodo.Request(
                    id,
                    detailB,
                    Completed: true)),
            baseline);

    var optimizedIO =
        FreeAlgebra.interpret(
            UpdateTodoOptimized.Get(
                new UpdateTodoOptimized.Request(
                    id,
                    detailB,
                    Completed: true)),
            optimized);

    var microIO =
        FreeAlgebra.interpret(
            UpdateTodoMicroOptimized.Get(
                new UpdateTodoMicroOptimized.Request(
                    id,
                    detailB,
                    Completed: true)),
            micro);

    _ = await baselineIO.RunAsync();
    _ = await optimizedIO.RunAsync();
    _ = await microIO.RunAsync();

    await Measure(
        "baseline",
        async () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                _ = await baselineIO.RunAsync();
            }
        });

    await Measure(
        "optimized",
        async () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                _ = await optimizedIO.RunAsync();
            }
        });

    await Measure(
        "micro",
        async () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                _ = await microIO.RunAsync();
            }
        });
}

void MeasureCompiledMicro()
{
    var alternatingGrounding =
        new MicroOptimizedUpdateTodoWork();

    alternatingGrounding.Seed(
        CreateTodo(
            id,
            detailA,
            completed: false));

    var toA =
        alternatingGrounding.Compile(
            new UpdateTodoMicroOptimized.Request(
                id,
                detailA,
                Completed: false));

    var toB =
        alternatingGrounding.Compile(
            new UpdateTodoMicroOptimized.Request(
                id,
                detailB,
                Completed: true));

    MeasureSync(
        "micro-alt",
        () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                _ = ((index & 1) == 0 ? toB : toA).Run();
            }
        });

    Console.WriteLine(
        $"micro-alt ops: lookups={alternatingGrounding.Lookups:N0} short-circuits={alternatingGrounding.SemanticShortCircuits:N0} evolutions={alternatingGrounding.Evolutions:N0} writes={alternatingGrounding.Writes:N0}");

    var stableGrounding =
        new MicroOptimizedUpdateTodoWork();

    stableGrounding.Seed(
        CreateTodo(
            id,
            detailA,
            completed: false));

    var stable =
        stableGrounding.Compile(
            new UpdateTodoMicroOptimized.Request(
                id,
                detailB,
                Completed: true));

    _ = stable.Run();

    MeasureSync(
        "micro-stable",
        () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                _ = stable.Run();
            }
        });

    Console.WriteLine(
        $"micro-stable ops: lookups={stableGrounding.Lookups:N0} short-circuits={stableGrounding.SemanticShortCircuits:N0} evolutions={stableGrounding.Evolutions:N0} writes={stableGrounding.Writes:N0}");

    var stableTodo =
        stableGrounding
            .Current(id)
            .IfNone(
                () => throw new InvalidOperationException(
                    "Expected compiled stable Todo."));

    var stableStep =
        UpdateTodoMicroOptimized.Lower(
            UpdateTodoMicroOptimized.Describe(
                new UpdateTodoMicroOptimized.Request(
                    id,
                    detailB,
                    Completed: true)));

    MeasureSync(
        "lookup-only",
        () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                _ = stableGrounding.Current(id);
            }
        });

    MeasureSync(
        "predicate",
        () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                _ = UpdateTodoMicroOptimized.SatisfiedBy(
                    stableTodo,
                    stableStep);
            }
        });

    MeasureSync(
        "response",
        () =>
        {
            for (var index = 0; index < iterations; index++)
            {
                _ = UpdateTodoMicroOptimized.Response.Present(
                    stableTodo);
            }
        });
}

static void MeasureSync(
    string name,
    Action action)
{
    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();

    var before = GC.GetAllocatedBytesForCurrentThread();
    var stopwatch = Stopwatch.StartNew();

    action();

    stopwatch.Stop();
    var allocated =
        GC.GetAllocatedBytesForCurrentThread() - before;

    Console.WriteLine(
        $"{name,-12} {stopwatch.Elapsed.TotalMilliseconds,10:N2} ms | {allocated / (double)iterations,10:N1} B/op");
}

static async Task Measure(
    string name,
    Func<Task> action)
{
    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();

    var before = GC.GetTotalAllocatedBytes(precise: true);
    var stopwatch = Stopwatch.StartNew();

    await action();

    stopwatch.Stop();
    var allocated =
        GC.GetTotalAllocatedBytes(precise: true) - before;

    Console.WriteLine(
        $"{name,-10} {stopwatch.Elapsed.TotalMilliseconds,10:N2} ms | {allocated / (double)iterations,10:N1} B/op");
}

static async Task ExecuteBaseline(
    ProbeBaselineUpdateTodoWork grounding,
    TodoId id,
    TodoDetail detail,
    bool completed)
{
    _ = await FreeAlgebra
        .interpret(
            UpdateTodo.Get(
                new UpdateTodo.Request(
                    id,
                    detail,
                    completed)),
            grounding)
        .RunAsync();
}

static async Task ExecuteOptimized(
    OptimizedUpdateTodoWork grounding,
    TodoId id,
    TodoDetail detail,
    bool completed)
{
    _ = await FreeAlgebra
        .interpret(
            UpdateTodoOptimized.Get(
                new UpdateTodoOptimized.Request(
                    id,
                    detail,
                    completed)),
            grounding)
        .RunAsync();
}

static async Task ExecuteMicro(
    MicroOptimizedUpdateTodoWork grounding,
    TodoId id,
    TodoDetail detail,
    bool completed)
{
    _ = await FreeAlgebra
        .interpret(
            UpdateTodoMicroOptimized.Get(
                new UpdateTodoMicroOptimized.Request(
                    id,
                    detail,
                    completed)),
            grounding)
        .RunAsync();
}

static Todo CreateTodo(
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

sealed class ProbeBaselineUpdateTodoWork :
    AlgebraIO<UpdateTodo.Algebra>
{
    private readonly Dictionary<TodoId, Todo> points = new();

    public ProbeBaselineUpdateTodoWork(
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
