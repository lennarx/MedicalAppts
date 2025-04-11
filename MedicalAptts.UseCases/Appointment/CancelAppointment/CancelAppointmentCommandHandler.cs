using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Enums;
using MedicalAppts.Core.Errors;

namespace MedicalAptts.UseCases.Appointment.CancelAppointment
{
    public class CancelAppointmentCommandHandler(IAppointmentsRepository appointmentsRepository) : IRequestHandler<CancelAppointmentCommand, Result<AppointmentDTO, Error>>
    {
        private readonly IAppointmentsRepository _appointmentsRepository = appointmentsRepository;
        public async Task<Result<AppointmentDTO, Error>> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentsRepository.GetAppointmentsByDateAndPatientIdAsync(request.AppointmentDate, request.PatientId);

            if (appointment is null)
            {
                return GenericErrors.AppointmentNotFound;
            }

            appointment.Status = AppointmentStatus.CANCELLED;
            appointment.Notes = request.Reason;
            await _appointmentsRepository.UpdateAsync(appointment);

            return new AppointmentDTO
            {
                Patient = appointment.Patient.Name,
                Doctor = appointment.Doctor.Name,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                Notes = appointment.Notes
            };
        }
    }
}
