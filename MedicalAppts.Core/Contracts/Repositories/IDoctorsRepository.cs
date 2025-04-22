using MedicalAppts.Core.Entities;
using MedicalAppts.Core.Enums;

namespace MedicalAppts.Core.Contracts.Repositories
{
    public interface IDoctorsRepository : IMedicalApptRepository<Doctor>
    {
        Task<IEnumerable<Doctor>> GetDoctorsBySpecialtyAsync(MedicalSpecialty specialty);
    }
}
