namespace MedicalAptts.UseCases.Doctor
{
    public class CreateDoctorScheduleForm
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
