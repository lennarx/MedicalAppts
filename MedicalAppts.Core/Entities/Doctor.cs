using MedicalAppts.Core.Enums;

namespace MedicalAppts.Core.Entities
{
    public class Doctor : BaseEntity
    {
        public string Name { get; set; }
        public MedicalSpecialty Specialty { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<DoctorSchedule> DoctorSchedules { get; set; }
    }
}
