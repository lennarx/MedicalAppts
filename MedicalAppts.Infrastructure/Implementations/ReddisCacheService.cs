using MedicalAppts.Core.Contracts;
using StackExchange.Redis;
using System.Text.Json;

namespace MedicalAppts.Infrastructure.Implementations
{
    public class ReddisCacheService(IConnectionMultiplexer connection) : ICacheService
    {
        private readonly IDatabase _db = connection.GetDatabase();
        public async Task<T> GetAsync<T>(string key)
        {
            var cacheResult = await _db.StringGetAsync(key);
            return cacheResult.HasValue ? JsonSerializer.Deserialize<T>(cacheResult!) : default;
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task SetAsync(string key, string value, TimeSpan? expiration = null)
        {
            await _db.StringSetAsync(key, value, expiration);
        }
    }
}
