using Microsoft.EntityFrameworkCore;
using TmsApi.Entities; // Points to your Entities folder

namespace TmsApi.Data;

public class TmsDbContext : DbContext
{
    public TmsDbContext(DbContextOptions<TmsDbContext> options) : base(options)
    {
    }

    // Existing 3 tables from Exercise 1
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    // Newly added 2 tables from your Entities folder
    public DbSet<Assessment> Assessments { get; set; }
    public DbSet<Certificate> Certificates { get; set; }
}
