using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MedicalAppts.Infrastructure.Implementations
{
    public abstract class MedicalApptRepository<T> : IMedicalApptRepository<T> where T : BaseEntity
    {
        private readonly MedicalApptsDbContext _context;
        protected readonly DbSet<T> _dbSet;   

        public MedicalApptRepository(MedicalApptsDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _dbSet.Remove(entity);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async virtual Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IEnumerable<T> GetFiltered(Func<T, bool> predicate, bool trackEntities = false)
        {
            return trackEntities == true ? _dbSet.Where(predicate).ToList() : _dbSet.AsNoTracking().Where(predicate).ToList();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _context.Database.CurrentTransaction != null
                ? _context.Database.CurrentTransaction
                : await _context.Database.BeginTransactionAsync();
        }
    }
}
