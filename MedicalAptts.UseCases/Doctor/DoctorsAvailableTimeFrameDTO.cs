namespace MedicalAptts.UseCases.Doctor
{
    public class DoctorsAvailableTimeFrameDTO
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<double> AvailableTimeFramesPerDay { get; set; } = new List<double>();
    }
}
