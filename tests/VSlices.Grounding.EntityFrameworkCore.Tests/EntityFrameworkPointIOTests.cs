using System.Runtime.CompilerServices;
using LanguageExt;
using VSlices.Monads;
using VSlices.Work;
using Xunit;

namespace VSlices.Grounding.EntityFrameworkCore.Tests;

public sealed class EntityFrameworkPointIOTests(PostgreSqlFixture database)
    : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task Direct_point_grounding_supports_optional_and_required_reads()
    {
        await database.ResetAsync();
        await using var context = database.CreateContext();

        var points =
            EntityFrameworkPointIO.Direct<TestDbContext, DirectRecord, Guid>(
                context,
                static point => point.Id,
                id => point => point.Id == id);

        var id = Guid.NewGuid();

        var missing = await points
            .ReadOrDefault(id)
            .Run()
            .As()
            .RunAsync(TestContext.Current.CancellationToken);

        Assert.True(missing.IsNone);

        await points
            .Write(new DirectRecord { Id = id, Name = "created" })
            .RunAsync(TestContext.Current.CancellationToken);

        var created = await points.Read(id).RunAsync(TestContext.Current.CancellationToken);
        Assert.Equal("created", created.Name);

        await points
            .Write(new DirectRecord { Id = id, Name = "updated" })
            .RunAsync(TestContext.Current.CancellationToken);

        var updated = await points.Read(id).RunAsync(TestContext.Current.CancellationToken);
        Assert.Equal("updated", updated.Name);

        await points.Remove(id).RunAsync(TestContext.Current.CancellationToken);

        var removed = await points
            .ReadOrDefault(id)
            .Run()
            .As()
            .RunAsync(TestContext.Current.CancellationToken);

        Assert.True(removed.IsNone);

        await Assert.ThrowsAsync<InvalidOperationException>(
            async () =>
            {
                _ = await points.Read(id).RunAsync(TestContext.Current.CancellationToken);
            });
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
            .Get(new ExerciseRecord.Request(id)).RunIO(algebra)
            .RunAsync(TestContext.Current.CancellationToken);

        Assert.Equal(
            new Record(id, "created"),
            evidence.Created);

        Assert.Equal(
            new Record(id, "updated"),
            evidence.Updated);

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

    public sealed record Response
    {
        private Response(Record created, Record updated, Option<Record> removed)
        {
            Created = created;
            Updated = updated;
            Removed = removed;
        }

        public Record Created { get; }
        public Record Updated { get; }
        public Option<Record> Removed { get; }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    private static extern Response NewResponse(
        Record created,
        Record updated,
        Option<Record> removed);

    public static Eff<RecordAlgebra, Response> Get(Request request) =>
        Eff<RecordAlgebra, Response>.LiftIO(algebra =>
            algebra.Writer.Write(new Record(request.Id, "created"))
                .Bind(_ => algebra.Reader.Read(request.Id))
                .Bind(created =>
                    algebra.Writer.Write(new Record(request.Id, "updated"))
                        .Bind(_ => algebra.Reader.Read(request.Id))
                        .Bind(updated =>
                            algebra.Remover.Remove(request.Id)
                                .Bind(_ =>
                                    algebra.Reader
                                        .ReadOrDefault(request.Id)
                                        .Run()
                                        .As())
                                .Map(removed =>
                                    NewResponse(
                                        created,
                                        updated,
                                        removed)))));
}
