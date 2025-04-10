namespace MedicalAppts.Core.Contracts
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string key);
        Task SetAsync(string key, string value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
    }
}
