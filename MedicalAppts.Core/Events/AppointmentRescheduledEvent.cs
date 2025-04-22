using MediatR;

namespace MedicalAppts.Core.Events
{
    public class AppointmentRescheduledEvent : AppointmentBaseEvent, INotification
    {
        public AppointmentRescheduledEvent(string doctor, string patient, string patientEmail, string dateTime) : base(doctor, patient, patientEmail, dateTime) { }
    }
}
