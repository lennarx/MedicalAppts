using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppts.Infrastructure.Implementations
{
    public class DoctorsScheduleRepository : MedicalApptRepository<DoctorSchedule>, IDoctorsScheduleRepository
    {
        public DoctorsScheduleRepository(MedicalApptsDbContext context) : base(context) { }
        public async Task<IEnumerable<DoctorSchedule>> GetSchedulesByDoctorIdAsync(int doctorId)
        {
            return await _dbSet.Include(x => x.Doctor).Where(x => x.DoctorId == doctorId).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<DoctorSchedule>> GetSchedulesByDayAsync(DayOfWeek day)
        {
            return await _dbSet.Where(x => x.DayOfWeek == day).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<DoctorSchedule>> GetSchedulesByDateAndDoctorIdAsync(DayOfWeek day, int doctorId)
        {
            return await _dbSet.Include(x => x.Doctor).Where(x => x.DayOfWeek == day && x.DoctorId == doctorId).AsNoTracking().ToListAsync();
        }
    }
}
