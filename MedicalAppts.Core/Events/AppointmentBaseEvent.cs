using MediatR;

namespace MedicalAppts.Core.Events
{
    public abstract class AppointmentBaseEvent
    {
        public string Doctor { get; }
        public string Patient { get; }
        public string PatientEmail { get; }
        public string DateTime { get; }

        public string MedicalSpeciality { get; set; }
        public AppointmentBaseEvent(string doctor, string patient, string patientEmail, string dateTime)
        {
            Doctor = doctor;
            Patient = patient;
            PatientEmail = patientEmail;
            DateTime = dateTime;
        }
    }
}
