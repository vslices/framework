using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using LanguageExt;
using LanguageExt.Traits;
using Microsoft.EntityFrameworkCore;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using SampleWorkflow.Work.Algebras;
using Testcontainers.PostgreSql;
using VSlices.Grounding.EntityFrameworkCore;
using VSlices.Work;
using VSlices.Space.Traits;

namespace SampleWorkflow.Benchmarks;

/// <summary>
/// Measures the current UpdateTodo WorkFlow end-to-end against a real PostgreSQL 17
/// instance started through Testcontainers.
///
/// The measured path is:
///
/// UpdateTodo.Describe
///     -> Free&lt;TodoAlgebra, Response&gt;
///     -> AlgebraIO&lt;TodoAlgebra&gt;
///     -> EntityFrameworkPointIO
///     -> Npgsql
///     -> PostgreSQL
/// </summary>
[MemoryDiagnoser]
[ShortRunJob]
public class UpdateTodoPostgreSqlBenchmark
{
    private const int OperationsPerInvoke = 10;

    private PostgreSqlContainer container = null!;
    private TodoBenchmarkDbContext context = null!;
    private PostgreSqlTodoWork grounding = null!;
    private TodoId id = null!;
    private TodoDetail detailA = null!;
    private TodoDetail detailB = null!;
    private int sequence;

    [GlobalSetup]
    public async Task Setup()
    {
        container =
            new PostgreSqlBuilder("postgres:17-alpine")
                .WithDatabase("sample_workflow_benchmark")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();

        await container.StartAsync();

        var options =
            new DbContextOptionsBuilder<TodoBenchmarkDbContext>()
                .UseNpgsql(container.GetConnectionString())
                .Options;

        context = new TodoBenchmarkDbContext(options);

        await context.Database.EnsureCreatedAsync();

        id = Transformable
            .Transform<Guid, TodoId>(
                Guid.Parse("7cb513f6-9e70-42ff-8568-62210c33d0cb"))
            .ThrowIfFail();

        detailA = Transformable
            .Transform<string, TodoDetail>("benchmark-a")
            .ThrowIfFail();

        detailB = Transformable
            .Transform<string, TodoDetail>("benchmark-b")
            .ThrowIfFail();

        var initial = Transformable
            .Transform<Todo.Input, Todo>(
                new Todo.Input(
                    id,
                    detailA,
                    Completed: false))
            .ThrowIfFail();

        context.Todos.Add(TodoProjection.From(initial));
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var points =
            new EntityFrameworkPointIO<
                TodoBenchmarkDbContext,
                Todo,
                TodoId,
                TodoProjection>(
                context,
                TodoProjection.From,
                TodoProjection.ToSemantic,
                static todo => todo.Id,
                semanticId =>
                    projection => projection.Id == semanticId.Value);

        grounding = new PostgreSqlTodoWork(points);
    }

    [Benchmark(
        Description = "Current UpdateTodo -> PostgreSQL",
        OperationsPerInvoke = OperationsPerInvoke)]
    public async Task<UpdateTodo.Response> UpdateTodo_Current_PostgreSql()
    {
        UpdateTodo.Response last = null!;

        for (var index = 0; index < OperationsPerInvoke; index++)
        {
            var next = sequence++;
            var useB = (next & 1) == 0;

            var request =
                new UpdateTodo.Request(
                    id,
                    useB ? detailB : detailA,
                    Completed: useB);

            last = await FreeAlgebra
                .interpret(
                    UpdateTodo.Describe(request),
                    grounding)
                .RunAsync();
        }

        var succeeded = last.Todo.Match(
            Left: static _ => false,
            Right: static todo => todo.IsSome);

        if (!succeeded)
        {
            throw new InvalidOperationException(
                "The benchmarked UpdateTodo execution did not return a Todo.");
        }

        return last;
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        if (context is not null)
        {
            await context.DisposeAsync();
        }

        if (container is not null)
        {
            await container.DisposeAsync();
        }
    }
}

public sealed class TodoBenchmarkDbContext(
    DbContextOptions<TodoBenchmarkDbContext> options)
    : DbContext(options)
{
    public DbSet<TodoProjection> Todos =>
        Set<TodoProjection>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoProjection>(
            todo =>
            {
                todo.HasKey(static value => value.Id);
                todo.Property(static value => value.Detail)
                    .IsRequired();
                todo.Property(static value => value.Attachments)
                    .IsRequired();
            });
    }
}

public sealed class TodoProjection
{
    public Guid Id { get; set; }

    public string Detail { get; set; } = string.Empty;

    public bool Completed { get; set; }

    public string[] Attachments { get; set; } = [];

    public static TodoProjection From(Todo todo) =>
        new()
        {
            Id = todo.Id.Value,
            Detail = todo.Detail.Value,
            Completed = todo.Completed,
            Attachments =
                todo.Attachments
                    .Select(static resource => resource.Value)
                    .ToArray()
        };

    public static Todo ToSemantic(TodoProjection projection)
    {
        var semanticId = Transformable
            .Transform<Guid, TodoId>(projection.Id)
            .ThrowIfFail();

        var detail = Transformable
            .Transform<string, TodoDetail>(projection.Detail)
            .ThrowIfFail();

        var attachments = projection.Attachments
            .Select(
                static value =>
                    Transformable
                        .Transform<string, ResourceReference>(value)
                        .ThrowIfFail())
            .ToArray();

        return Transformable
            .Transform<Todo.Input, Todo>(
                new Todo.Input(
                    semanticId,
                    detail,
                    projection.Completed)
                {
                    Attachments = attachments
                })
            .ThrowIfFail();
    }
}

public sealed class PostgreSqlTodoWork(
    EntityFrameworkPointIO<
        TodoBenchmarkDbContext,
        Todo,
        TodoId,
        TodoProjection> points)
    : AlgebraIO<TodoAlgebra>
{
    public IO<A> Interpret<A>(
        K<TodoAlgebra, A> operation) =>
        operation switch
        {
            NextTodoIdPart<TodoAlgebra, A> next =>
                IO.lift(
                    () =>
                        next.Next(
                            Transformable
                                .Transform<Guid, TodoId>(Guid.NewGuid())
                                .ThrowIfFail())),

            ReadTodoPart<TodoAlgebra, A> read =>
                points.Read(read.Id)
                    .Map(read.Next),

            WriteTodoPart<TodoAlgebra, A> write =>
                points.Write(write.Point)
                    .Map(write.Next),

            RemoveTodoPart<TodoAlgebra, A> remove =>
                points.Remove(remove.Id)
                    .Map(remove.Next),

            _ => throw new NotSupportedException(
                $"Unsupported {nameof(TodoAlgebra)} operation: {operation.GetType().Name}.")
        };
}
