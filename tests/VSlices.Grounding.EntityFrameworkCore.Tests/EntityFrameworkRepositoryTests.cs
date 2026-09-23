using LanguageExt;
using Xunit;

namespace VSlices.Grounding.EntityFrameworkCore.Tests;

public sealed class EntityFrameworkRepositoryTests(PostgreSqlFixture database)
    : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task Direct_repository_preserves_CRUD_and_identity_semantics()
    {
        await database.ResetAsync();
        await using var context = database.CreateContext();

        var repository =
            EntityFrameworkRepository.Direct<TestDbContext, DirectRecord, Guid>(
                context,
                id => value => value.Id == id);

        var id = Guid.NewGuid();

        var created = await repository
            .Create(new DirectRecord { Id = id, Name = "created" })
            .RunAsync();

        Assert.Equal(id, created.Id);
        Assert.True(await repository.Any(id).RunAsync());

        var found = await repository
            .Read(id)
            .Run()
            .As()
            .RunAsync();

        Assert.True(found.IsSome);
        Assert.Equal("created", found.Match(static value => value.Name, static () => string.Empty));

        created.Name = "updated";
        var updated = await repository.Update(created).RunAsync();

        Assert.Equal("updated", updated.Name);

        var all = await repository.Read().RunAsync();
        Assert.Single(all);
        Assert.Equal("updated", all.Head.IfNone(new DirectRecord()).Name);

        await repository.Delete(updated).RunAsync();

        Assert.False(await repository.Any(id).RunAsync());
    }

    [Fact]
    public async Task Projected_repository_preserves_CRUD_and_identity_semantics()
    {
        await database.ResetAsync();
        await using var context = database.CreateContext();

        var repository =
            new EntityFrameworkRepository<TestDbContext, Record, Guid, RecordProjection>(
                context,
                static value => new RecordProjection
                {
                    Id = value.Id,
                    Name = value.Name
                },
                static projection => new Record(projection.Id, projection.Name),
                id => projection => projection.Id == id);

        var id = Guid.NewGuid();

        var created = await repository
            .Create(new Record(id, "created"))
            .RunAsync();

        Assert.Equal(new Record(id, "created"), created);
        Assert.True(await repository.Any(id).RunAsync());

        var found = await repository
            .Read(id)
            .Run()
            .As()
            .RunAsync();

        Assert.True(found.IsSome);
        Assert.Equal(
            new Record(id, "created"),
            found.Match(static value => value, static () => new Record(Guid.Empty, string.Empty)));

        var updated = await repository
            .Update(created with { Name = "updated" })
            .RunAsync();

        Assert.Equal(new Record(id, "updated"), updated);

        var all = await repository.Read().RunAsync();
        Assert.Single(all);
        Assert.Equal(new Record(id, "updated"), all.Head.IfNone(new Record(Guid.Empty, string.Empty)));

        await repository.Delete(updated).RunAsync();

        Assert.False(await repository.Any(id).RunAsync());
    }
}
