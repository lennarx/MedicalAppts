using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;
using MedicalAppts.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppts.Infrastructure.Implementations
{
    public class DoctorsRepository : MedicalApptRepository<Doctor>, IDoctorsRepository
    {
        public DoctorsRepository(MedicalApptsDbContext context) : base(context) { }
        public async Task<IEnumerable<Doctor>> GetDoctorsBySpecialtyAsync(MedicalSpecialty specialty)
        {
            return await _dbSet.Where(x => x.Specialty == specialty).AsNoTracking().ToListAsync();
        }

        public async Task<BaseUser?> GetUserByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateUserAsync(BaseUser user)
        {
            await base.UpdateAsync((Doctor)user);
        }
    }
}
