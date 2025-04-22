using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Appointment.GetAppointmentsPerDate
{
    public class GetAppointmentsPerDateQuery : IRequest<Result<IEnumerable<AppointmentDTO>, Error>>
    {
        public DateTime AppointmentDate { get; }
        public GetAppointmentsPerDateQuery(DateTime appointmentDate)
        {
            AppointmentDate = appointmentDate;
        }
    }
}
