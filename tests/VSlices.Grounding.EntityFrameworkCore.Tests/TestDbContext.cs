using Microsoft.EntityFrameworkCore;

namespace VSlices.Grounding.EntityFrameworkCore.Tests;

public sealed class TestDbContext(DbContextOptions<TestDbContext> options)
    : DbContext(options)
{
    public DbSet<DirectRecord> DirectRecords => Set<DirectRecord>();

    public DbSet<RecordProjection> Records => Set<RecordProjection>();
}

public sealed class DirectRecord
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}

public sealed class RecordProjection
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}

public sealed record Record(Guid Id, string Name);
