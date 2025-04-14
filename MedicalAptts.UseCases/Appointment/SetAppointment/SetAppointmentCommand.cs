using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Appointment.SetAppointment
{
    public class SetAppointmentCommand : IRequest<Result<AppointmentDTO, Error>>
    {
        public int PatientId { get; }
        public int DoctorId { get; }
        public DateTime AppointmentDate { get; }
        public SetAppointmentCommand(int patientId, int doctorId, DateTime appointmentDate)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            AppointmentDate = appointmentDate;
        }
    }
}
