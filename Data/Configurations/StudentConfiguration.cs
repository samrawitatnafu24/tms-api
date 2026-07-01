using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TmsApi.Entities; 

namespace TmsApi.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(s => s.Id);

        // Name is required and limited to 100 characters max
        builder.Property(s => s.Name)
            .HasMaxLength(100)
            .IsRequired();

        // Registration number is required and limited to 20 characters max
        builder.Property(s => s.RegistrationNumber)
            .HasMaxLength(20)
            .IsRequired();
    }
}
