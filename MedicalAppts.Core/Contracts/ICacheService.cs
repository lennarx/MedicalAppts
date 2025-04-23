namespace MedicalAppts.Core.Contracts
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync(string key, string value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
    }
}
