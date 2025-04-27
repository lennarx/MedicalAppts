using MediatR;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Contracts;
using Microsoft.Extensions.Logging;
using MedicalAppts.Core;
using MedicalAppts.Core.Errors;

namespace MedicalAptts.UseCases.Appointment
{
    public abstract class UpdateAppointmentBaseCommandHandler(IAppointmentsRepository appointmentsRepository, ILogger<UpdateAppointmentBaseCommandHandler> logger,
        IMediator mediator, ICacheService cacheService) 
    {
        protected readonly IAppointmentsRepository _appointmentsRepository = appointmentsRepository;
        protected readonly ILogger<UpdateAppointmentBaseCommandHandler> _logger = logger;
        protected readonly IMediator _mediator = mediator;
        protected readonly ICacheService _cacheService = cacheService;

        protected async Task<UpdateApptValidationResult> PerformInitialValidation(UpdateAppointmentBaseCommand request)
        {
            var appointmentToUpdate = await _appointmentsRepository.GetByIdAsync(request.AppointmentId);

            var validationResult = ValidateAppointmentToUpdate(appointmentToUpdate, request.PatientId, request.AppointmentId);
            if (validationResult != null)
            {
                return new UpdateApptValidationResult { ErrorResult = validationResult };
            }

            return new UpdateApptValidationResult { Appointment = appointmentToUpdate };

        }

        protected Result<AppointmentDTO, Error> ValidateAppointmentToUpdate(MedicalAppts.Core.Entities.Appointment apptToUpdate, int patientId, int apptId)
        {
            if (apptToUpdate is null)
            {
                _logger.LogError($"Appointment with id {apptId} not found");
                return Result<AppointmentDTO, Error>.Failure(GenericErrors.AppointmentNotFound);
            }

            if (apptToUpdate.PatientId != patientId)
            {
                _logger.LogError($"Patient with id {patientId} is not the owner of the appointment with id {apptId}");
                return Result<AppointmentDTO, Error>.Failure(GenericErrors.AppointmentNotOwnedByPatient);
            }

            return null;
        }

        protected async Task RemoveCacheIfExistsAsync(int doctorId, DateTime appointmentDate)
        {
            var cacheKey = $"DoctorAvailability:{doctorId}:{appointmentDate:yyyyMMdd}";
            await _cacheService.RemoveAsync(cacheKey);
        }
    }

    public class UpdateApptValidationResult
    {
        public MedicalAppts.Core.Entities.Appointment Appointment { get; set; }
        public Result<AppointmentDTO, Error> ErrorResult { get; set; }
    }
}
