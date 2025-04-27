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
    public class CancelAppointmentCommandHandler : UpdateAppointmentBaseCommandHandler, IRequestHandler<CancelAppointmentCommand, Result<AppointmentDTO, Error>>
    {
        public CancelAppointmentCommandHandler(IAppointmentsRepository appointmentsRepository, ILogger<UpdateAppointmentBaseCommandHandler> logger,
            IMediator mediator, ICacheService cacheService) : base(appointmentsRepository, logger, mediator, cacheService) {}
        public async Task<Result<AppointmentDTO, Error>> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await PerformInitialValidation(request);

            if(validationResult.ErrorResult != null)
            {
                return validationResult.ErrorResult;
            }            
            var appointmentToCancel = validationResult.Appointment;

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
    }
}
