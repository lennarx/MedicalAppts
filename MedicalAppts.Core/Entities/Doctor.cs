using MedicalAppts.Core.Enums;

namespace MedicalAppts.Core.Entities
{
    public class Doctor : User
    {
        public MedicalSpecialty Specialty { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; }
    }
}
