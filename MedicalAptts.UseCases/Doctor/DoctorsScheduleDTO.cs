namespace MedicalAptts.UseCases.Doctor
{
    public class DoctorsScheduleDTO
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
