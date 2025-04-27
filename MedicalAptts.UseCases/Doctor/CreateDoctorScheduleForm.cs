using System.Text.Json.Serialization;

namespace MedicalAptts.UseCases.Doctor
{
    public class CreateDoctorScheduleForm
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DayOfWeek DayOfWeek { get; set; }
    }
}
