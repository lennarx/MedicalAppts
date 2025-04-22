using MedicalAppts.Core.Entities;
using MedicalAppts.Core.Enums;
using MedicalAppts.Infrastructure;
using Microsoft.AspNet.Identity;

namespace MedicalAppts.Api.Utils.Seeders
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(MedicalApptsDbContext context)
        {
            if (!context.Patients.Any())
            {
                var hasher = new PasswordHasher();
                var admin = new Patient
                {
                    Name = "Admin",
                    PhoneNumber = "1234567890",
                    PasswordHash = hasher.HashPassword("Admin123!"),
                    UserRole = UserRole.ADMIN,
                    Email = "admin@medicalapp.com"
                };

                context.Patients.Add(admin);
                await context.SaveChangesAsync();
            }
        }
    }
}
