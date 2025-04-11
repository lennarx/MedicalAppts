using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Doctor.GetDoctorSchedule
{
    public class GetDoctorsScheduleQuery : IRequest<Result<IEnumerable<DoctorsScheduleDTO>, Error>>
    {
        public int DoctorId { get; }
        public DateTime? AppointmentDate { get; }
        public GetDoctorsScheduleQuery(int doctorId, DateTime? appointmentDate = null)
        {
            DoctorId = doctorId;
            AppointmentDate = appointmentDate;
        }
    }
}
