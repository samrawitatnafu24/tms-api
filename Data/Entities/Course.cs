using System.Collections.Generic;

namespace TmsApi.Entities;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    
    // Add this line back to satisfy your Program.cs seeding logic
    public int Capacity { get; set; }

    // Required for Exercise 6 (Migration) and Exercise 9 (Bulk Archive & Soft Delete)
    public bool IsArchived { get; set; }

    // Navigation Properties
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
    public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
}
