namespace MedicalAppts.Core.Errors
{
    public class GenericErrors
    {
        public static readonly Error AppointmentNotFound = new Error(404, "Appointment not found for provided criteria");
        public static readonly Error NoSpecialtyDoctorsFound = new Error(404, "No doctors found for required specialty");
        public static readonly Error AppointmentRequestError = new Error(400, "Error while processing appointment request");
        public static readonly Error AppointmentAlreadyExists = new Error(400, "Appointment already exists");
        public static readonly Error AppointmentCancellationError = new Error(400, "An error occurred while attempting to cancel the appointment");
        public static readonly Error AppointmentUpdateError = new Error(400, "An error occurred while attempting to reschedule the appointment");
        public static readonly Error AppointmentNotOwnedByPatient = new Error(400, "The patient is not the owner of the appointment");
        public static readonly Error AppointmentOutOfTimeFrame = new Error(400, "Appointment is not within the doctor attention time frame");
        public static readonly Error DoctorCreationError = new Error(400, "An error occurred while attempting to create the doctor");
        public static readonly Error ScheduleNotSet = new Error(404, "Doctor schedules or appointments not found");
        public static readonly Error PatientCreationError = new Error(400, "An error occurred while attempting to create the patient");
    }
}
