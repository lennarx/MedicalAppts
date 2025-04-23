using MedicalAppts.Core.Contracts;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Infrastructure.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

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

            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(config["Reddis:Host"]));

            services.AddScoped<ITokenService, JwtService>();
            services.AddScoped<ICacheService, ReddisCacheService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDoctorsRepository ,DoctorsRepository>();
            services.AddScoped<IPatientsRepository, PatientsRepository>();
            services.AddScoped<IAppointmentsRepository, AppointmentsRepository>();
            services.AddScoped<IDoctorsScheduleRepository, DoctorsScheduleRepository>();

            return services;
        }

    }
}
