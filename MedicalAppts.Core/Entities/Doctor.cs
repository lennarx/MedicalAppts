using MedicalAppts.Core.Enums;

namespace MedicalAppts.Core.Entities
{
    public class Doctor : BaseEntity
    {
        public string Name { get; set; }
        public MedicalSpecialty Specialty { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; }
    }
}
