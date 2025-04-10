namespace MedicalAppts.Core.Entities
{
    public class DoctorSchedule : BaseEntity
    {
        public Doctor Doctor { get; set; }
        public int DoctorId { get; set; }
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
    }
}
