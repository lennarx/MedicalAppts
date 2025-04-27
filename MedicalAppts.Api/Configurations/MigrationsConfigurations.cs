using MedicalAppts.Api.Utils.Seeders;
using MedicalAppts.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppts.Api.Configurations
{
    public static class MigrationsConfigurations
    {
        public static async Task ApplyMigrationsIfNeeded(this IServiceProvider serviceProvider, IWebHostEnvironment env)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MedicalApptsDbContext>();
                var providerName = dbContext.Database.ProviderName;

                if (!string.IsNullOrEmpty(providerName) && providerName != "Microsoft.EntityFrameworkCore.InMemory")
                {
                    if (dbContext.Database.GetPendingMigrations().Any())
                    {
                        dbContext.Database.Migrate();
                    }
                }

                if (!env.IsDevelopment())
                {
                    await DbSeeder.SeedAsync(dbContext);
                }
            }
        }
    }
}
