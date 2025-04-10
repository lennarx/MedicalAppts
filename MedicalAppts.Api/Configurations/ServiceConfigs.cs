using MedicalAppts.Infrastructure.Configurations;

namespace MedicalAppts.Api.Configurations
{
    public static class ServiceConfigs
    {
        public static IServiceCollection AddServiceConfigs(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddAuthentication();
            services.AddInfrastructureServices(builder.Configuration);
            return services;
        }
    }
}
