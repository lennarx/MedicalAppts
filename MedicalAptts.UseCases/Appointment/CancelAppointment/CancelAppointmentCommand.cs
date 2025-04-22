using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Appointment.CancelAppointment
{
    public class CancelAppointmentCommand : IRequest<Result<AppointmentDTO, Error>>
    {
        public int AppointmentId { get; }
        public int PatientId { get; }
        public CancelAppointmentCommand(int appointmentId, int patientId)
        {
            AppointmentId = appointmentId;
            PatientId = patientId;
        }
    }
}
