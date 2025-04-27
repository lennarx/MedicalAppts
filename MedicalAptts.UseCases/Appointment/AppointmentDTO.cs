using MedicalAppts.Core.Enums;
using System.Text.Json.Serialization;

namespace MedicalAptts.UseCases.Appointment
{
    public class AppointmentDTO
    {
        public int AppointmentId { get; set; }
        public string Patient { get; set; }
        public string Doctor { get; set; }
        public DateTime AppointmentDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AppointmentStatus Status { get; set; }
        public string Notes { get; set; }
    }
}
