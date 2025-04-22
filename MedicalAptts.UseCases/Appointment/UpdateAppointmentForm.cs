namespace MedicalAptts.UseCases.Appointment
{
    public class UpdateAppointmentForm
    {
        public DateTime? NewDate { get; set; }
        public AppointmentActionsEnum Action { get; set; }
        public UpdateAppointmentForm(DateTime? newDate, AppointmentActionsEnum action)
        {
            NewDate = newDate;
            Action = action;
        }
    }
}
