using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace VSlices.Grounding.EntityFrameworkCore.Tests;

public sealed class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container =
        new PostgreSqlBuilder("postgres:17-alpine")
            .WithDatabase("vslices_grounding")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();
        await ResetAsync();
    }

    public async ValueTask DisposeAsync() =>
        await _container.DisposeAsync();

    public TestDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(_container.GetConnectionString())
            .Options;

        return new TestDbContext(options);
    }

    public async Task ResetAsync()
    {
        await using var context = CreateContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}
