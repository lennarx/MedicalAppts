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

                var apiKeySecurityScheme = new OpenApiSecurityScheme
                {
                    Description = "API Key needed to access the endpoints. Example: 'AUTH-API-KEY: {your-key}'",
                    In = ParameterLocation.Header,
                    Name = "AUTH-API-KEY",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "ApiKeyScheme",
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
                };

                var jwtSecurityScheme = new OpenApiSecurityScheme
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
                options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                options.AddSecurityDefinition(apiKeySecurityScheme.Reference.Id, apiKeySecurityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                            },
                            new List<string>()
                        },
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
                            },
                            new List<string>()
                        }
                    });
            });
            return services;
        }
    }
}
