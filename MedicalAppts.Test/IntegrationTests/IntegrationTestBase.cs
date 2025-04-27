using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;

namespace MedicalAppts.Test.IntegrationTests
{
    public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        protected readonly HttpClient _client;

        public IntegrationTestBase(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        protected async Task AuthorizeAsync()
        {
            var login = new { Email = "admin@medicalapp.com", Password = "Admin123!" };
            var response = await _client.PostAsJsonAsync("/api/login", login);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            var token = json.ToString();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
