using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Appointment.UpdateAppointment
{
    public class UpdateAppointmentCommand : UpdateAppointmentBaseCommand, IRequest<Result<AppointmentDTO, Error>>
    {
        public DateTime? NewDate { get; }
        public UpdateAppointmentCommand(int appointmentId, int patientId, DateTime? newDate) : base(appointmentId, patientId)   
        {
            NewDate = newDate;
        }
    }
}
