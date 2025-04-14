using MediatR;

namespace MedicalAppts.Core.Events
{
    public class AppointmentCanceledEvent : INotification
    {
        public string Doctor { get; }
        public string Patient { get; }
        public string PatientEmail { get; }
        public string DateTime { get; }

        public string MedicalSpeciality { get; set; }
        public AppointmentCanceledEvent(string doctor, string patient, string patientEmail, string dateTime)
        {
            Doctor = doctor;
            Patient = patient;
            PatientEmail = patientEmail;
            DateTime = dateTime;
        }
    }
}
