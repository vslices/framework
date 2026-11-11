using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Functional.Tests;

public sealed class TestDbContext(DbContextOptions<TestDbContext> options)
    : DbContext(options)
{
    public DbSet<TestRecord> Records => Set<TestRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestRecord>(entity =>
        {
            entity.ToTable("records");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired();
            entity.Property(x => x.Quantity);
            entity.Property(x => x.Price).HasPrecision(18, 2);
        });
    }
}

public sealed class TestRecord
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }
}
