using MedicalAppts.Api.Configurations;
using MedicalAppts.Api.Utils.Seeders;
using MedicalAppts.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MedicalApptsDbContext>();
    if (dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }
    if (app.Environment.IsDevelopment())
    { 
        await DbSeeder.SeedAsync(dbContext);
    }
}


app.MapControllers();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.Run();
