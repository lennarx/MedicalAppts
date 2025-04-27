using MedicalAppts.Core.Enums;
using System.Text.Json.Serialization;

namespace MedicalAptts.UseCases.Doctor
{
    public class DoctorCreationForm
    {
        public string Email { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MedicalSpecialty Specialty { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
