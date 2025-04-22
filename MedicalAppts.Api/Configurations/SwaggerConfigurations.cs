using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace MedicalAppts.Api.Configurations
{
    public static class SwaggerConfigurations
    {
        public static IServiceCollection AddSwaggerConfigs(this IServiceCollection services)
        {
            var info = new OpenApiInfo()
            {
                Title = "Medical Appointments Api Documentation",
                Version = "v1",
                Description = "This API allows you to perform appointments managements. Also allows you to create users (doctors and patients)."
            };

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", info);

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                options.EnableAnnotations();
                options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        { securityScheme, Array.Empty<string>() }
                    });
            });
            return services;
        }
    }
}
