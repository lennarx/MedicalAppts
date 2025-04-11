using MedicalAppts.Core.Entities;

namespace MedicalAppts.Core.Contracts.Repositories
{
    public interface IMedicalApptRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> GetFiltered(Func<T, bool> predicate, bool trackEntities = false);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
