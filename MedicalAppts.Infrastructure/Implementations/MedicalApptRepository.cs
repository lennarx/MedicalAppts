using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppts.Infrastructure.Implementations
{
    public abstract class MedicalApptRepository<T>(MedicalApptsDbContext context) : IMedicalApptRepository<T> where T : BaseEntity
    {
        private readonly MedicalApptsDbContext _context = context;
        protected readonly DbSet<T> _dbSet;   
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IEnumerable<T> GetFiltered(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public void UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }

    }
}
