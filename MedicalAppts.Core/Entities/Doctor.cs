using MedicalAppts.Core.Enums;

namespace MedicalAppts.Core.Entities
{
    public class Doctor : BaseUser
    {
        public string Name { get; set; }
        public MedicalSpecialty Specialty { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; }
        public UserRole UserRole { get; set; } = UserRole.DOCTOR;
    }
}
