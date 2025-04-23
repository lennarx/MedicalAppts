using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Errors;
using MedicalAppts.Core.Events;
using MedicalAptts.UseCases.Doctor;
using MedicalAptts.UseCases.Helpers.Extensions;
using Microsoft.Extensions.Logging;

namespace MedicalAptts.UseCases.Appointment.CancelAppointment
{
    public class CancelAppointmentCommandHandler(IAppointmentsRepository appointmentsRepository, ILogger<CancelAppointmentCommand> logger,
        IMediator mediator, ICacheService cacheService) : IRequestHandler<CancelAppointmentCommand, Result<AppointmentDTO, Error>>
    {
        private readonly IAppointmentsRepository _appointmentsRepository = appointmentsRepository;
        private readonly ILogger<CancelAppointmentCommand> _logger = logger;
        private readonly IMediator _mediator = mediator;
        private readonly ICacheService _cacheService = cacheService;
        public async Task<Result<AppointmentDTO, Error>> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointmentToCancel = await _appointmentsRepository.GetByIdAsync(request.AppointmentId);

            if (appointmentToCancel is null)
            {
                _logger.LogError($"Appointment with id {request.AppointmentId} not found");
                return Result<AppointmentDTO, Error>.Failure(GenericErrors.AppointmentNotFound);
            }

            if (appointmentToCancel.PatientId != request.PatientId)
            {
                _logger.LogError($"Patient with id {request.PatientId} is not the owner of the appointment with id {request.AppointmentId}");
                return Result<AppointmentDTO, Error>.Failure(GenericErrors.AppointmentNotOwnedByPatient);
            }

            appointmentToCancel.Status = MedicalAppts.Core.Enums.AppointmentStatus.CANCELLED;

            await RemoveCacheIfExistsAsync(appointmentToCancel.DoctorId, appointmentToCancel.AppointmentDate);

            try
            {
                await _appointmentsRepository.UpdateAsync(appointmentToCancel);

                await _mediator.Publish(new AppointmentCanceledEvent(appointmentToCancel.Doctor.Email, appointmentToCancel.Patient.Name, appointmentToCancel.Patient.Email, appointmentToCancel.AppointmentDate.ToString("dd-mm-YYYY")));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Appointment with id {request.AppointmentId} could not be cancelled");
                _logger.LogError($"{ex}");
                return Result<AppointmentDTO, Error>.Failure(GenericErrors.AppointmentCancellationError);
            }

            return Result<AppointmentDTO, Error>.Success(appointmentToCancel.MapToAppointmentDTO());
        }

        private async Task RemoveCacheIfExistsAsync(int doctorId, DateTime appointmentDate)
        {
            var cacheKey = $"DoctorAvailability:{doctorId}:{appointmentDate:yyyyMMdd}";
            await _cacheService.RemoveAsync(cacheKey);
        }
    }
}
