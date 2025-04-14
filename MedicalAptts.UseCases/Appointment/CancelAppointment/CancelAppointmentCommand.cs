using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Appointment.CancelAppointment
{
    public class CancelAppointmentCommand : IRequest<Result<AppointmentDTO, Error>>
    {
        public int AppointmentId { get; }
        public CancelAppointmentCommand(int appointmentId)
        {
            AppointmentId = appointmentId;
        }
    }
}
