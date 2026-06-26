using System;

namespace TmsApi.Entities;

public class Certificate
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public required string CredentialUrl { get; set; }

    // Navigation properties
    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
}
