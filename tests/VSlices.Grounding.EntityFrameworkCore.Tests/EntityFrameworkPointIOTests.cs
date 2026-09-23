using LanguageExt;
using LanguageExt.Traits;
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
    public async Task Projected_point_grounding_interprets_a_Feature_owned_algebra()
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

        var interpreter =
            new RecordAlgebraIO(points, points, points);

        var id = Guid.NewGuid();

        var program =
            from _ in PointWriter.write<RecordAlgebra, Record>(
                new Record(id, "created"))
            from created in PointReader.read<RecordAlgebra, Record, Guid>(id)
            from __ in PointWriter.write<RecordAlgebra, Record>(
                new Record(id, "updated"))
            from updated in PointReader.read<RecordAlgebra, Record, Guid>(id)
            from ___ in PointRemover.remove<RecordAlgebra, Record, Guid>(id)
            from removed in PointReader.read<RecordAlgebra, Record, Guid>(id)
            select new PointEvidence(created, updated, removed);

        var evidence = await FreeAlgebra
            .interpret(program, interpreter)
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

public sealed record PointEvidence(
    Option<Record> Created,
    Option<Record> Updated,
    Option<Record> Removed);

public abstract record RecordOperation<A> : K<RecordAlgebra, A>;

public sealed record ReadRecord<A>(
    Guid Id,
    Func<Option<Record>, A> Next)
    : RecordOperation<A>;

public sealed record WriteRecord<A>(
    Record Point,
    Func<Unit, A> Next)
    : RecordOperation<A>;

public sealed record RemoveRecord<A>(
    Guid Id,
    Func<Unit, A> Next)
    : RecordOperation<A>;

public sealed class RecordAlgebra :
    Functor<RecordAlgebra>,
    PointReader<RecordAlgebra, Record, Guid>,
    PointWriter<RecordAlgebra, Record>,
    PointRemover<RecordAlgebra, Record, Guid>
{
    static K<RecordAlgebra, Option<Record>>
        PointReader<RecordAlgebra, Record, Guid>.Read(Guid id) =>
        new ReadRecord<Option<Record>>(id, static point => point);

    static K<RecordAlgebra, Unit>
        PointWriter<RecordAlgebra, Record>.Write(Record point) =>
        new WriteRecord<Unit>(point, static value => value);

    static K<RecordAlgebra, Unit>
        PointRemover<RecordAlgebra, Record, Guid>.Remove(Guid id) =>
        new RemoveRecord<Unit>(id, static value => value);

    static K<RecordAlgebra, B> Functor<RecordAlgebra>.Map<A, B>(
        Func<A, B> f,
        K<RecordAlgebra, A> ma) =>
        ma switch
        {
            ReadRecord<A>(var id, var next) =>
                new ReadRecord<B>(id, point => f(next(point))),
            WriteRecord<A>(var point, var next) =>
                new WriteRecord<B>(point, value => f(next(value))),
            RemoveRecord<A>(var id, var next) =>
                new RemoveRecord<B>(id, value => f(next(value))),
            _ => throw new NotSupportedException()
        };
}

public sealed class RecordAlgebraIO(
    PointReaderIO<Record, Guid> reader,
    PointWriterIO<Record> writer,
    PointRemoverIO<Record, Guid> remover)
    : AlgebraIO<RecordAlgebra>
{
    public IO<A> Interpret<A>(K<RecordAlgebra, A> operation) =>
        operation switch
        {
            ReadRecord<A> read =>
                reader.Read(read.Id).Map(read.Next),
            WriteRecord<A> write =>
                writer.Write(write.Point).Map(write.Next),
            RemoveRecord<A> remove =>
                remover.Remove(remove.Id).Map(remove.Next),
            _ => throw new NotSupportedException()
        };
}
