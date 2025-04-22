using MediatR;

namespace MedicalAppts.Core.Events
{
    public class AppointmentCreatedEvent : AppointmentBaseEvent, INotification
    {
        public AppointmentCreatedEvent(string doctor, string patient, string patientEmail, string dateTime) : base(doctor, patient, patientEmail, dateTime) { }
    }
}
