using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppts.Infrastructure.Implementations
{
    public class AppointmentsRepository : MedicalApptRepository<Appointment>, IAppointmentsRepository
    {
        public AppointmentsRepository(MedicalApptsDbContext context) : base(context) { }
        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateAndDoctorIdAsync(DateTime date, int doctorId)
        {
            return await _dbSet.Where(x => x.AppointmentDate.Date == date.Date && x.DoctorId == doctorId).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            return await _dbSet
                .Where(x => x.AppointmentDate.Date == date.Date)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            return await _dbSet.Where(x => x.DoctorId == doctorId).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            return await _dbSet.Where(x => x.PatientId == patientId).AsNoTracking().ToListAsync();
        }
    }
}
