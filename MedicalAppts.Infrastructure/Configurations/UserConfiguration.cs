using MedicalAppts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAppts.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(b => b.Email)
                .HasMaxLength(30);

            builder.Property(b => b.PasswordHash)
                .IsRequired();

            builder.Property(b => b.UserStatus)
                .IsRequired();

            builder.Property(b => b.UserRole)
                .IsRequired();
        }
    }
}
