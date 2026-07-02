using System.Collections.Generic;

namespace TmsApi.Entities;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public decimal GPA { get; set; }
    public bool IsActive { get; set; }
     public uint RowVersion { get; set; }
     public bool IsDeleted { get; set; }

    // Navigation Properties
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
}
