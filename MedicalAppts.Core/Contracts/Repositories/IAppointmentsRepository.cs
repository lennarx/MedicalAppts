using MedicalAppts.Core.Entities;

namespace MedicalAppts.Core.Contracts.Repositories
{
    public interface IAppointmentsRepository : IMedicalApptRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date);
        Task<IEnumerable<Appointment>> GetAppointmentsByDateAndDoctorIdAsync(DateTime date, int doctorId);
        Task<Appointment> GetAppointmentsByDateAndPatientIdAsync(DateTime appointmentDate, int patientId);
    }
}
