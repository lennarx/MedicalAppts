using MedicalAppts.Core.Enums;

namespace MedicalAppts.Core.Entities
{
    public class Patient : BaseUser
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public UserRole UserRole { get; set; } = UserRole.PATIENT;
    }
}
