using MedicalAppts.Core.Enums;

namespace MedicalAppts.Core.Entities
{
    public class Patient : User
    {
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
