using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using TmsApi.Entities;
using TmsApi.Data;
using TmsApi;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TmsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TmsDatabase"))
        .LogTo(Console.WriteLine, LogLevel.Information) // Logs SQL to the console
        .EnableSensitiveDataLogging());                // Displays raw parameter values

// =========================================================================
// 1. SERVICE REGISTRATION (Dependency Injection)
//builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddControllers();

// Register the strict security services required by the runtime pipeline
builder.Services.AddAuthentication("Training")
    .AddScheme<AuthenticationSchemeOptions, TrainingAuthHandler>("Training", null);
builder.Services.AddAuthorization();

// Register TmsDbContext scoped for incoming HTTP requests
builder.Services.AddDbContext<TmsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TmsDatabase")));

var app = builder.Build();

// =========================================================================
// 2. MIDDLEWARE PIPELINE CONFIGURATION (Ordering Matters Explicitly!)
// =========================================================================
app.UseMiddleware<RequestLoggingMiddleware>();

// Standard framework behaviors follow
app.UseExceptionHandler("/error"); // For Session 3 ProblemDetails integration
app.UseHttpsRedirection();

app.UseRouting();

// Security MUST intercept requests right after routing matches them, 
// and ALWAYS before endpoints run!
app.UseAuthentication();
app.UseAuthorization();

// =========================================================================
// 3. ENDPOINT DEFINITIONS
// =========================================================================
app.MapControllers();

// Secured placeholder endpoint for Session 1 evaluation
app.MapGet("/api/assessments/results", () => Results.Ok(new
{
    courseCode = "CS-101",
    studentId = "S-001",
    letterGrade = "A"
}))
.RequireAuthorization(); // Instantly turns away anonymous calls with a 401 response
// Seed test data at startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TmsDbContext>();
    
    // Applies any pending migrations and ensures the database exists
    context.Database.Migrate(); 

    // Check if the database is already seeded
    if (!context.Students.Any())
    {
        // 1. Define the collection of Students
        var students = new List<Student>
        {
            new() { RegistrationNumber = "TMS-2026-0001", Name = "Alice Smith", GPA = 3.8m, IsActive = true },
            new() { RegistrationNumber = "TMS-2026-0002", Name = "Bob Jones", GPA = 2.9m, IsActive = true },
            new() { RegistrationNumber = "TMS-2026-0003", Name = "Charlie Brown", GPA = 3.4m, IsActive = false },
            new() { RegistrationNumber = "TMS-2026-0004", Name = "Diana Prince", GPA = 3.9m, IsActive = true },
            new() { RegistrationNumber = "TMS-2026-0005", Name = "Evan Wright", GPA = 2.5m, IsActive = true }
        };
        context.Students.AddRange(students);

        // 2. Define the collection of Courses
        var courses = new List<Course>
        {
            new() { Code = "CS-101", Title = "Introduction to Computer Science", Capacity = 30 },
            new() { Code = "CS-201", Title = "Data Structures and Algorithms", Capacity = 25 },
            new() { Code = "MAT-101", Title = "Calculus I", Capacity = 40 }
        };
        context.Courses.AddRange(courses);
        
        // Save first to push rows to PostgreSQL and populate the database-generated Identity IDs
        context.SaveChanges(); 

        // 3. Define the collection of Enrollments using tracked index references
        var enrollments = new List<Enrollment>
        {
            new() { StudentId = students[0].Id, CourseId = courses[0].Id, Grade = 4.0m },
            new() { StudentId = students[0].Id, CourseId = courses[1].Id, Grade = 3.6m },
            new() { StudentId = students[1].Id, CourseId = courses[0].Id, Grade = 2.8m },
            new() { StudentId = students[3].Id, CourseId = courses[1].Id, Grade = 3.9m }
        };
        context.Enrollments.AddRange(enrollments);
        
        // Final save to insert relationships
        context.SaveChanges();
    }
}
app.Run();