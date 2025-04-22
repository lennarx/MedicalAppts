using MedicalAppts.Core.Enums;

namespace MedicalAppts.Core.Entities
{
    public class Appointment : BaseEntity
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public virtual Patient Patient { get; set; }
        public int PatientId { get; set; }
        public virtual Doctor Doctor { get; set; }
        public int DoctorId { get; set; }
        public string? ReasonForVisit { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}
