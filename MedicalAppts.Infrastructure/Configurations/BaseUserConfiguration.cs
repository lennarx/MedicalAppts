using MedicalAppts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAppts.Infrastructure.Configurations
{
    public class BaseUserConfiguration : IEntityTypeConfiguration<BaseUser>
    {
        public void Configure(EntityTypeBuilder<BaseUser> builder)
        {
            builder.Property(b => b.Email)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(b => b.PasswordHash)
                .IsRequired();

            builder.Property(b => b.UserStatus)
                .IsRequired();
        }
    }
}
