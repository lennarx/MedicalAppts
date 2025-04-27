namespace MedicalAptts.UseCases.Appointment
{
    public abstract class UpdateAppointmentBaseCommand
    {
        public int AppointmentId { get; }
        public int PatientId { get; }
        public UpdateAppointmentBaseCommand(int appointmentId, int patientId)
        {
            AppointmentId = appointmentId;
            PatientId = patientId;
        }
    }
}
