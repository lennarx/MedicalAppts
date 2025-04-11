namespace MedicalAppts.Core.Errors
{
    public class GenericErrors
    {
        public static readonly Error AppointmentNotFound = new Error(400, "Appointment not found for provided criteria");
    }
}
