using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Doctor.SetDoctorSchedule
{
    public class SetDoctorScheduleCommand : IRequest<Result<DoctorsScheduleDTO, Error>>
    {
        public int DoctorId { get; }
        public DayOfWeek DayOfWeek { get; }
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }
        public SetDoctorScheduleCommand(int doctorId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            DoctorId = doctorId;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
