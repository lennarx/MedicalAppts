using FluentAssertions;
using MedicalAppts.Test.IntegrationTests;
using System.Net;
using System.Net.Http.Json;

public class LoginControllerIntegrationTests : IntegrationTestBase
{
    public LoginControllerIntegrationTests(CustomWebApplicationFactory<Program> factory) : base(factory) {}

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenFormIsInvalid()
    {
        var login = new { Email = "wrongUser", Password = "Admin123!" };
        var response = await _client.PostAsJsonAsync("/api/login", login);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenPasswordIsInvalid()
    {
        var login = new { Email = "admin@medicalapp.com", Password = "Admin123!!!!!" };
        var response = await _client.PostAsJsonAsync("/api/login", login);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
