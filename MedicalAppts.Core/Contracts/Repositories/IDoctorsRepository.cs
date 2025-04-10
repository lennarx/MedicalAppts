using MedicalAppts.Core.Entities;
using MedicalAppts.Core.Enums;

namespace MedicalAppts.Core.Contracts.Repositories
{
    public interface IDoctorsRepository : IMedicalApptRepository<Doctor>
    {
        Task<IEnumerable<Doctor>> GetDoctorsBySpecialtyAsync(MedicalSpecialty specialty);
        //Task<IEnumerable<Doctor>> GetAvailableDoctorsAsyncBySpecialty(DateTime appointmentDate, TimeSpan startTime, TimeSpan endTime, MedicalSpecialty specialty);
        //Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime appointmentDate, TimeSpan startTime, TimeSpan endTime);
    }
}
