using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppts.Infrastructure.Implementations
{
    public class AppointmentsRepository : MedicalApptRepository<Appointment>, IAppointmentsRepository
    {
        public AppointmentsRepository(MedicalApptsDbContext context) : base(context) { }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateAndDoctorIdAsync(DateTime? date, int doctorId)
        {
            return await _dbSet.Include(x => x.Doctor)
                .Include(x => x.Patient).
                Where(x => x.DoctorId == doctorId && (!date.HasValue || x.AppointmentDate.Date == date.Value.Date)).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateAndPatientIdAsync(DateTime? date, int patientId)
        {
            return await _dbSet.Include(x => x.Doctor)
                .Include(x => x.Patient)
                .Where(x => x.PatientId == patientId && (!date.HasValue || x.AppointmentDate.Date == date.Value.Date)).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            return await _dbSet.Include(x => x.Patient)
                .Include(x => x.Doctor)
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

        public override async Task<Appointment> GetByIdAsync(int id)
        {
            return await _dbSet.Include(x => x.Doctor)
                .Include(x => x.Patient)
                .FirstOrDefaultAsync(appt => appt.Id == id);
        }
    }
}
