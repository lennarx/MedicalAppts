using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Appointment.CancelAppointment
{
    public class CancelAppointmentCommand : IRequest<Result<AppointmentDTO, Error>>
    {
        public DateTime AppointmentDate { get; }
        public int PatientId { get; }
        public string Reason { get; }
        public CancelAppointmentCommand(DateTime appointmentDate, int patientId, string reason)
        {
            AppointmentDate = appointmentDate;
            PatientId = patientId;
            Reason = reason;
        }
    }
}
