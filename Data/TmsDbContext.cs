using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TmsApi.Entities;

namespace TmsApi.Data;

public class TmsDbContext : DbContext
{
    public TmsDbContext(DbContextOptions<TmsDbContext> options) : base(options) { }

    // Restoring the core tables required by Program.cs and your other Controllers
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Enrollment> Enrollments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Checkpoint Item #5: Discover entity configuration classes dynamically
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TmsDbContext).Assembly);

        base.OnModelCreating(modelBuilder);

        // Exercise 8: Configure Shadow Property and Concurrency Token for Student
        modelBuilder.Entity<Student>(builder =>
        {
            // Shadow Property for background database auditing
            builder.Property<DateTime>("LastUpdated")
                   .HasDefaultValueSql("NOW()")
                   .IsRequired();

            // PostgreSQL optimistic concurrency token using native xmin column
            builder.Property(s => s.RowVersion)
                   .IsRowVersion();
        });

        // Exercise 9: Global Query Filter for Soft Delete
        modelBuilder.Entity<Student>()
                    .HasQueryFilter(s => !s.IsDeleted);
    }

    // Exercise 8: Automatically update the LastUpdated audit stamp on saves
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Metadata.FindProperty("LastUpdated") != null)
            {
                entry.Property("LastUpdated").CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
