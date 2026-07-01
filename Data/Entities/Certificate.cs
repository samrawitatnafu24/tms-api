namespace TmsApi.Entities;

public class Certificate
{
    public int Id { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    
    // Foreign Keys
    public int StudentId { get; set; }
    public int CourseId { get; set; }

    // Navigation Properties
    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
}
