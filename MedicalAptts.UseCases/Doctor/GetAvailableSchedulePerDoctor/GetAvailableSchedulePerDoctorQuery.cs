using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Doctor.GetAvailableSchedulePerDoctor
{
    public class GetAvailableSchedulePerDoctorQuery : IRequest<Result<IEnumerable<DoctorsAvailableTimeFrameDTO>, Error>>
    {
        public int DoctorId { get; }
        public DateTime? AppointmentDate { get; }
        public GetAvailableSchedulePerDoctorQuery(int doctorId, DateTime? appointmentDate = null)
        {
            DoctorId = doctorId;
            AppointmentDate = appointmentDate;
        }
    }
}
