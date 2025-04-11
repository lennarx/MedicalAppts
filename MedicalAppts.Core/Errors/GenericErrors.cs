namespace MedicalAppts.Core.Errors
{
    public class GenericErrors
    {
        public static readonly Error AppointmentNotFound = new Error(400, "Appointment not found for provided criteria");
        public static readonly Error NoSpecialtyDoctorsFound = new Error(404, "No doctors found for required specialty");
    }
}
