using MedicalAppts.Core.Entities;

namespace MedicalAppts.Core.Contracts.Repositories
{
    public interface IMedicalApptRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> GetFiltered(Func<T, bool> predicate);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void UpdateAsync(T entity);
        Task DeleteAsync(int id);        
    }
}
