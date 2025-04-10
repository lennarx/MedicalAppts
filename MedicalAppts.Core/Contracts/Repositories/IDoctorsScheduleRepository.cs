using MedicalAppts.Core.Entities;

namespace MedicalAppts.Core.Contracts.Repositories
{
    public interface IDoctorsScheduleRepository : IMedicalApptRepository<DoctorSchedule>
    {
        Task<IEnumerable<DoctorSchedule>> GetSchedulesByDoctorIdAsync(int doctorId);
        Task<IEnumerable<DoctorSchedule>> GetSchedulesByDayAsync(DayOfWeek day);
        Task<IEnumerable<DoctorSchedule>> GetSchedulesByDateAndDoctorIdAsync(DayOfWeek day, int doctorId);
    }
}
