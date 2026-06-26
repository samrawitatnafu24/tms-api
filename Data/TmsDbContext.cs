using Microsoft.EntityFrameworkCore;
using TmsApi.Entities; // Points to your Entities folder

namespace TmsApi.Data;

public class TmsDbContext : DbContext
{
    public TmsDbContext(DbContextOptions<TmsDbContext> options) : base(options)
    {
    }

    // Required tables for Exercise 1
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
}
