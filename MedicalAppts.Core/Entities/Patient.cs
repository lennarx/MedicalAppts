namespace MedicalAppts.Core.Entities
{
    public class Patient : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
