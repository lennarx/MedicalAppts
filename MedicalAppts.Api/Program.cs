using MedicalAppts.Api.Configurations;
using Microsoft.OpenApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();
        builder.Services.AddServiceConfigs(builder);
        var app = builder.Build();

        app.UseSwagger(options =>
        {
            options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Medical API v1");
            c.RoutePrefix = "swagger";
        });

        await app.Services.ApplyMigrationsIfNeeded(app.Environment);

        app.MapControllers();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.Run();
    }    
}


