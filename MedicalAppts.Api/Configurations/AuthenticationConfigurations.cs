using MedicalAppts.Api.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MedicalAppts.Api.Configurations
{
    public static class AuthenticationConfigurations
    {
        public static IServiceCollection AddAuthenticationConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication("SmartAuth")
                .AddPolicyScheme("SmartAuth", "JWT o API Key", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        var hasApiKey = context.Request.Headers.ContainsKey("AUTH-API-KEY");
                        var hasBearer = context.Request.Headers.ContainsKey("Authorization") &&
                                        context.Request.Headers["Authorization"].ToString().StartsWith("Bearer ");
                        return hasApiKey ? "ApiKey" :
                               hasBearer ? JwtBearerDefaults.AuthenticationScheme :
                               "ApiKey";
                    };
                });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:TokenSecretKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddAuthentication("SmartAuth").AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null);
            return services;
        }
    }
}
