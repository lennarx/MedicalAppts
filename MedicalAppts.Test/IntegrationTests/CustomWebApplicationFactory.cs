using MedicalAppts.Core.Contracts;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Infrastructure;
using MedicalAppts.Infrastructure.Implementations;
using MedicalAppts.Test.IntegrationTests;
using MedicalAppts.Test.IntegrationTests.ImplementationsForTest;
using MedicalAptts.UseCases.Users.Login;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    private readonly string _dbName = $"FakeMedicalApptTestDb_{Guid.NewGuid()}";
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            ConfigureInMemoryDatabase(services);
            RegisterApplicationServices(services);
            RegisterMediatR(services);
            InitializeDatabase(services);
        });
    }
    private void ConfigureInMemoryDatabase(IServiceCollection services)
    {
        var contextDescriptors = services
       .Where(d =>
           //d.ServiceType == typeof(DbContextOptions<MedicalApptsDbContext>) ||
           //d.ServiceType == typeof(MedicalApptsDbContext) ||
           d.ServiceType.Assembly.FullName == "Microsoft.EntityFrameworkCore, Version=9.0.4.0, Culture=neutral, PublicKeyToken=adb9793829ddae60")
       .ToList();

        foreach (var descriptor in contextDescriptors)
            services.Remove(descriptor);

        services.AddDbContext<MedicalApptsDbContext>(options =>
            options.UseInMemoryDatabase(_dbName));
    }

    private void RegisterApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDoctorsRepository, DoctorsRepository>();
        services.AddScoped<IAppointmentsRepository, AppointmentTestRepository>();
        services.AddScoped<IDoctorsScheduleRepository, DoctorsScheduleRepository>();
        services.AddScoped<IPatientsRepository, PatientsRepository>();
        services.AddScoped<ITokenService, JwtService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICacheService, InMemoryCacheService>();
    }

    private void RegisterMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(
                typeof(TProgram).Assembly,
                typeof(LoginCommand).Assembly));
    }

    private void InitializeDatabase(IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();

        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<MedicalApptsDbContext>();
        db.Database.EnsureCreated();

        TestDataSeeder.SeedAsync(db).GetAwaiter().GetResult();

    }
}