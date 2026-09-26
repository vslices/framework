using LanguageExt;
using VSlices.Monads;
using VSlices.Work;
using Xunit;

namespace VSlices.Grounding.EntityFrameworkCore.Tests;

public sealed class EntityFrameworkPointIOTests(PostgreSqlFixture database)
    : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task Direct_point_grounding_reads_writes_and_removes_without_Repository_semantics()
    {
        await database.ResetAsync();
        await using var context = database.CreateContext();

        var points =
            EntityFrameworkPointIO.Direct<TestDbContext, DirectRecord, Guid>(
                context,
                static point => point.Id,
                id => point => point.Id == id);

        var id = Guid.NewGuid();

        var missing = await points.Read(id).RunAsync();
        Assert.True(missing.IsNone);

        await points
            .Write(new DirectRecord { Id = id, Name = "created" })
            .RunAsync();

        var created = await points.Read(id).RunAsync();
        Assert.Equal(
            "created",
            created.Match(static point => point.Name, static () => string.Empty));

        await points
            .Write(new DirectRecord { Id = id, Name = "updated" })
            .RunAsync();

        var updated = await points.Read(id).RunAsync();
        Assert.Equal(
            "updated",
            updated.Match(static point => point.Name, static () => string.Empty));

        await points.Remove(id).RunAsync();

        var removed = await points.Read(id).RunAsync();
        Assert.True(removed.IsNone);
    }

    [Fact]
    public async Task Projected_point_grounding_can_form_an_executable_algebra_for_a_Feature()
    {
        await database.ResetAsync();
        await using var context = database.CreateContext();

        var points =
            new EntityFrameworkPointIO<TestDbContext, Record, Guid, RecordProjection>(
                context,
                static point => new RecordProjection
                {
                    Id = point.Id,
                    Name = point.Name
                },
                static projection => new Record(
                    projection.Id,
                    projection.Name),
                static point => point.Id,
                id => projection => projection.Id == id);

        var algebra = new RecordAlgebra(
            Reader: points,
            Writer: points,
            Remover: points);

        var id = Guid.NewGuid();

        var evidence = await ExerciseRecord
            .Get()
            .RunFlow(
                algebra,
                new ExerciseRecord.Request(id))
            .RunAsync();

        Assert.Equal(
            new Record(id, "created"),
            evidence.Created.Match(
                static point => point,
                static () => new Record(Guid.Empty, string.Empty)));

        Assert.Equal(
            new Record(id, "updated"),
            evidence.Updated.Match(
                static point => point,
                static () => new Record(Guid.Empty, string.Empty)));

        Assert.True(evidence.Removed.IsNone);
    }
}

public sealed record RecordAlgebra(
    PointReader<Record, Guid> Reader,
    PointWriter<Record> Writer,
    PointRemover<Record, Guid> Remover);

public sealed class ExerciseRecord :
    Feature<ExerciseRecord, RecordAlgebra, ExerciseRecord.Request, ExerciseRecord.Response>
{
    public sealed record Request(Guid Id);

    public sealed record Response(
        Option<Record> Created,
        Option<Record> Updated,
        Option<Record> Removed);

    public static Flow<RecordAlgebra, Request, Response> Get() =>
        new((algebra, request) =>
            algebra.Writer.Write(new Record(request.Id, "created"))
                .Bind(_ => algebra.Reader.Read(request.Id))
                .Bind(created =>
                    algebra.Writer.Write(new Record(request.Id, "updated"))
                        .Bind(_ => algebra.Reader.Read(request.Id))
                        .Bind(updated =>
                            algebra.Remover.Remove(request.Id)
                                .Bind(_ => algebra.Reader.Read(request.Id))
                                .Map(removed =>
                                    new Response(
                                        created,
                                        updated,
                                        removed)))));
}
