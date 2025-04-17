using MedicalAptts.UseCases.Users.Login;
using System.Reflection;

namespace MedicalAppts.Api.Configurations
{  
    public static class MediatrConfigurations
    {
        public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
        {
            var loginCommandAssembly = Assembly.GetAssembly(typeof(LoginCommandHandler));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(loginCommandAssembly!));
            return services;
        }
    }
}
