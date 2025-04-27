using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Appointment.CancelAppointment
{
    public class CancelAppointmentCommand : UpdateAppointmentBaseCommand, IRequest<Result<AppointmentDTO, Error>>
    {
        public CancelAppointmentCommand(int appointmentId, int patientId) : base(appointmentId, patientId) { }
    }
}
