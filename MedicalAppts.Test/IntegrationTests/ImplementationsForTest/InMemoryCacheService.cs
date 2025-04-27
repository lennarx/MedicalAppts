using MedicalAppts.Core.Contracts;
using System.Text.Json;

namespace MedicalAppts.Test.IntegrationTests.ImplementationsForTest
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly Dictionary<string, string> _store = new();

        public async Task<T> GetAsync<T>(string key)
        {
            await Task.FromResult(_store.TryGetValue(key, out var value));
            if (value == null)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(value);
        }

        public Task SetAsync(string key, string value, TimeSpan? ttl = null)
        {
            _store[key] = value;
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _store.Remove(key);
            return Task.CompletedTask;
        }
    }
}
