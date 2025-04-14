using MedicalAppts.Core.Enums;

namespace MedicalAptts.UseCases.Doctor
{
    public class CreateDoctorForm
    {
        public string Email { get; set; }
        public MedicalSpecialty Specialty { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
