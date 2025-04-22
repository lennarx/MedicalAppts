using MedicalAppts.Core.Enums;

namespace MedicalAptts.UseCases.Doctor
{
    public class DoctorDTO
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public MedicalSpecialty Specialty { get; set; }
    }
}
