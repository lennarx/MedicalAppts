using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Appointment.UpdateAppointment
{
    public class UpdateAppointmentCommand : IRequest<Result<AppointmentDTO, Error>>
    {
        public int AppointmentId { get; }
        public int PatientId { get; }
        public DateTime? NewDate { get; }
        public UpdateAppointmentCommand(int appointmentId, int patientId, DateTime? newDate)
        {
            AppointmentId = appointmentId;
            PatientId = patientId;
            NewDate = newDate;
        }
    }
}
