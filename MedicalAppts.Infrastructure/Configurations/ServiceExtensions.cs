using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAppts.Infrastructure.Configurations
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<MedicalApptsDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                options.EnableDetailedErrors();
            });
            //services.AddScoped<IJwtService, JwtService>();
            //services.AddScoped<ICacheService, CacheService>();
            //services.AddScoped(typeof(IAtmRepository<>), typeof(AtmRepository<>));

            return services;
        }
    }
}
