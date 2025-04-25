using MedicalAppts.Core.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace MedicalAppts.Api.Authentication;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IConfiguration _configuration;
    private const string HEADER_API_KEY = "AUTH-API-KEY";

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IConfiguration configuration)
        : base(options, logger, encoder, clock)
    {
        _configuration = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(HEADER_API_KEY, out var apiKeyHeader))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var headerKey = apiKeyHeader.ToString();
        var expectedKey = _configuration["ApiKey"];
        string? role = null;

        if (headerKey == $"{expectedKey}-PATIENT") role = UserRole.PATIENT.ToString();
        else if (headerKey == $"{expectedKey}-DOCTOR") role = UserRole.DOCTOR.ToString();
        else if (headerKey == expectedKey) role = UserRole.ADMIN.ToString();

        if (role == null)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid API Key or missing role"));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "ApiKeyUser"),
            new Claim(ClaimTypes.Role, role)
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}