using MedicalAppts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAppts.Infrastructure.Configuration
{
    public class DoctorsConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Specialty)
                .IsRequired()
                .HasConversion<string>();

            builder.HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId);

            builder.HasMany(d => d.DoctorSchedules)
                .WithOne(ds => ds.Doctor)
                .HasForeignKey(ds => ds.DoctorId);
        }
    }
}
