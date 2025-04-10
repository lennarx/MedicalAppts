using MedicalAppts.Core.Contracts;
using StackExchange.Redis;

namespace MedicalAppts.Infrastructure.Implementations
{
    public class ReddisCacheService(IConnectionMultiplexer connection) : ICacheService
    {
        private readonly IDatabase _db = connection.GetDatabase();
        public async Task<string?> GetAsync(string key)
        {
            return await _db.StringGetAsync(key);
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
