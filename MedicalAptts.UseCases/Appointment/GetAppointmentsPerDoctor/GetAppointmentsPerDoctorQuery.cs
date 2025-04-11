using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Appointment.GetAppointmentsPerDoctor
{
    public class GetAppointmentsPerDoctorQuery : IRequest<Result<IEnumerable<AppointmentDTO>, Error>>
    {
        public int DoctorId { get; }
        public DateTime? AppointmentDate { get; }
        public GetAppointmentsPerDoctorQuery(int doctorId, DateTime? appointmentDate = null)
        {
            DoctorId = doctorId;
            AppointmentDate = appointmentDate;
        }
    }
}
