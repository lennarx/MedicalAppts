using MedicalAppts.Core.Enums;

namespace MedicalAptts.UseCases.Appointment
{
    public class AppointmentDTO
    {
        public string Patient { get; set; }
        public string Doctor { get; set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
        public string Notes { get; set; }
    }
}
