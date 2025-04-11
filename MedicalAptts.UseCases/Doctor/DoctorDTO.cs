using MedicalAppts.Core.Enums;

namespace MedicalAptts.UseCases.Doctor
{
    public class DoctorDTO
    {
        public string Name { get; set; }
        public MedicalSpecialty Specialty { get; set; }
    }
}
