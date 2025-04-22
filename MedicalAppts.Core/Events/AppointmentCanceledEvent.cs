using MediatR;

namespace MedicalAppts.Core.Events
{
    public class AppointmentCanceledEvent : AppointmentBaseEvent, INotification
    {
        public AppointmentCanceledEvent(string doctor, string patient, string patientEmail, string dateTime) : base(doctor, patient, patientEmail, dateTime) {}
    }
}
