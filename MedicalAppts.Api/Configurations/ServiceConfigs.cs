using MedicalAppts.Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppts.Api.Configurations
{
    public static class ServiceConfigs
    {
        public static IServiceCollection AddServiceConfigs(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddAuthentication();
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddAuthenticationConfigs(builder.Configuration);
            services.AddInfrastructureServices(builder.Configuration);
            services.AddMediatrConfigs();
            services.AddSwaggerConfigs();
            return services;
        }
    }
}
