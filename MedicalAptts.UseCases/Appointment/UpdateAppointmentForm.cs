using System.Text.Json.Serialization;

namespace MedicalAptts.UseCases.Appointment
{
    public class UpdateAppointmentForm
    {
        public DateTime? NewDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AppointmentActionsEnum Action { get; set; }
    }
}
