namespace MedicalAptts.UseCases.Appointment
{
    public class AppointmentCreationForm
    {
        public int DoctorId { get; set;  }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
