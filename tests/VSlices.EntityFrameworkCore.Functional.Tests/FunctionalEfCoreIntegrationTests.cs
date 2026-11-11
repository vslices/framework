using EntityFrameworkCore.Functional;
using LanguageExt;
using LanguageExt.Traits;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCore.Functional.Tests;

public sealed class FunctionalEfCoreIntegrationTests(PostgreSqlFixture database)
    : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task Query_terminals_and_aggregates_execute_against_postgresql()
    {
        await database.ResetAsync();
        await using var context = database.CreateContext();

        await context.AddRangeIO(
            new[]
            {
                new TestRecord { Name = "alpha", Quantity = 2, Price = 10.50m },
                new TestRecord { Name = "beta", Quantity = 4, Price = 20.25m },
                new TestRecord { Name = "gamma", Quantity = 6, Price = 30.75m }
            }).RunAsync();
        await context.SaveChangesIO().RunAsync();

        Assert.True(await context.Records.AnyIO(x => x.Name == "beta").RunAsync());
        Assert.True(await context.Records.AllIO(x => x.Quantity > 0).RunAsync());
        Assert.Equal(3, await context.Records.CountIO().RunAsync());
        Assert.Equal(3L, await context.Records.LongCountIO().RunAsync());

        var first = await context.Records
            .OrderBy(x => x.Id)
            .FirstIO()
            .RunAsync();

        var single = await context.Records
            .SingleIO(x => x.Name == "gamma")
            .RunAsync();

        var none = await context.Records
            .Where(x => x.Name == "missing")
            .FirstOrNoneIO()
            .Run()
            .As()
            .RunAsync();

        var sequence = await context.Records
            .OrderBy(x => x.Id)
            .ToSeqIO()
            .RunAsync();

        Assert.Equal("alpha", first.Name);
        Assert.Equal("gamma", single.Name);
        Assert.True(none.IsNone);
        Assert.Equal(3, sequence.Count);

        Assert.Equal(12, await context.Records.Select(x => x.Quantity).SumIO().RunAsync());
        Assert.Equal(4d, await context.Records.Select(x => x.Quantity).AverageIO().RunAsync());
        Assert.Equal(2, await context.Records.Select(x => x.Quantity).MinIO().RunAsync());
        Assert.Equal(6, await context.Records.Select(x => x.Quantity).MaxIO().RunAsync());
        Assert.Equal(61.50m, await context.Records.Select(x => x.Price).SumIO().RunAsync());
        Assert.Equal(20.50m, await context.Records.Select(x => x.Price).AverageIO().RunAsync());
    }

    [Fact]
    public async Task Context_mutations_and_find_preserve_ef_semantics()
    {
        await database.ResetAsync();
        await using var context = database.CreateContext();

        var record = new TestRecord
        {
            Name = "created",
            Quantity = 1,
            Price = 5m
        };

        var added = await context.AddIO(record).RunAsync();
        Assert.Equal(EntityState.Added, added.State);
        await context.SaveChangesIO().RunAsync();

        context.ChangeTracker.Clear();

        var found = await context.Records
            .FindOrNoneIO(record.Id)
            .Run()
            .As()
            .RunAsync();

        Assert.True(found.IsSome);

        // Find correctly attaches the resolved instance. Clear it before exercising
        // UpdateIO with the original detached instance of the same identity.
        context.ChangeTracker.Clear();

        record.Name = "updated";
        var updated = await context.UpdateIO(record).RunAsync();
        Assert.Equal(EntityState.Modified, updated.State);
        await context.SaveChangesIO().RunAsync();

        context.ChangeTracker.Clear();
        Assert.Equal(
            "updated",
            await context.Records
                .Where(x => x.Id == record.Id)
                .Select(x => x.Name)
                .SingleAsync(TestContext.Current.CancellationToken));

        var removed = await context.RemoveIO(record).RunAsync();
        Assert.Equal(EntityState.Deleted, removed.State);
        await context.SaveChangesIO().RunAsync();

        Assert.False(await context.Records.AnyAsync(
            x => x.Id == record.Id,
            TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Execute_update_and_delete_run_as_database_operations()
    {
        await database.ResetAsync();
        await using var context = database.CreateContext();

        await context.AddRangeIO(
            new[]
            {
                new TestRecord { Name = "keep", Quantity = 1, Price = 1m },
                new TestRecord { Name = "change", Quantity = 2, Price = 2m },
                new TestRecord { Name = "delete", Quantity = 3, Price = 3m }
            }).RunAsync();
        await context.SaveChangesIO().RunAsync();
        context.ChangeTracker.Clear();

        var updated = await context.Records
            .Where(x => x.Name == "change")
            .ExecuteUpdateIO(setters =>
                setters.SetProperty(x => x.Quantity, x => x.Quantity + 10))
            .RunAsync();

        var deleted = await context.Records
            .Where(x => x.Name == "delete")
            .ExecuteDeleteIO()
            .RunAsync();

        Assert.Equal(1, updated);
        Assert.Equal(1, deleted);
        Assert.Equal(
            12,
            await context.Records
                .Where(x => x.Name == "change")
                .Select(x => x.Quantity)
                .SingleAsync());
        Assert.False(await context.Records.AnyAsync(
            x => x.Name == "delete",
            TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Transactions_commit_and_rollback_real_database_state()
    {
        await database.ResetAsync();

        await using (var context = database.CreateContext())
        {
            var transaction = await context.BeginTransactionIO().RunAsync();

            await context.AddIO(
                new TestRecord { Name = "rolled-back", Quantity = 1, Price = 1m })
                .RunAsync();
            await context.SaveChangesIO().RunAsync();
            await transaction.RollbackIO().RunAsync();
            await transaction.DisposeIO().RunAsync();
        }

        await using (var verification = database.CreateContext())
        {
            Assert.False(await verification.Records.AnyAsync(
                x => x.Name == "rolled-back",
                TestContext.Current.CancellationToken));
        }

        await using (var context = database.CreateContext())
        {
            var transaction = await context.BeginTransactionIO().RunAsync();

            await context.AddIO(
                new TestRecord { Name = "committed", Quantity = 2, Price = 2m })
                .RunAsync();
            await context.SaveChangesIO().RunAsync();
            await transaction.CommitIO().RunAsync();
            await transaction.DisposeIO().RunAsync();
        }

        await using var finalVerification = database.CreateContext();
        Assert.True(await finalVerification.Records.AnyAsync(
            x => x.Name == "committed",
            TestContext.Current.CancellationToken));
    }
}
