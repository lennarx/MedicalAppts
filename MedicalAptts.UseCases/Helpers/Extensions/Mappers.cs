using MedicalAppts.Core.Entities;
using MedicalAptts.UseCases.Doctor;

namespace MedicalAptts.UseCases.Helpers.Extensions
{
    public static class Mappers
    {
        public static IEnumerable<DoctorsScheduleDTO> MapToDoctorsScheduleDTOs(this IEnumerable<DoctorSchedule> schedules)
        {
            return schedules.Select(x => new DoctorsScheduleDTO
            {
                DayOfWeek = x.DayOfWeek,
                DoctorName = x.Doctor.Name,
                StartTime = x.StartTime.Hours * 100 + x.StartTime.Minutes,
                EndTime = x.EndTime.Hours * 100 + x.EndTime.Minutes
            });
        }

        public static DoctorsScheduleDTO MapToDoctorScheduleDTO(this DoctorSchedule schedule)
        {
            return new DoctorsScheduleDTO
            {
                DayOfWeek = schedule.DayOfWeek,
                DoctorName = schedule.Doctor.Name,
                StartTime = schedule.StartTime.Hours * 100 + schedule.StartTime.Minutes,
                EndTime = schedule.EndTime.Hours * 100 + schedule.EndTime.Minutes
            };
        }
    }
}
