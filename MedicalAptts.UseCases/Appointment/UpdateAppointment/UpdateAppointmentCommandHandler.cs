using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Errors;
using MedicalAptts.UseCases.Appointment.SetAppointment;
using Microsoft.Extensions.Logging;

namespace MedicalAptts.UseCases.Appointment.UpdateAppointment
{
    public class UpdateAppointmentCommandHandler
        : UpdateAppointmentBaseCommandHandler, IRequestHandler<UpdateAppointmentCommand, Result<AppointmentDTO, Error>>
    {
        public UpdateAppointmentCommandHandler(IAppointmentsRepository appointmentsRepository, ILogger<UpdateAppointmentBaseCommandHandler> logger,
            IMediator mediator, ICacheService cacheService) : base(appointmentsRepository, logger, mediator, cacheService) { }
        public async Task<Result<AppointmentDTO, Error>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await PerformInitialValidation(request);

            if (validationResult.ErrorResult != null)
            {
                return validationResult.ErrorResult;
            }
            var appointmentToUpdate = validationResult.Appointment;
            var doctorId = appointmentToUpdate.DoctorId;

            using var transaction = await _appointmentsRepository.BeginTransactionAsync();


            try
            {
                await _appointmentsRepository.DeleteAsync(appointmentToUpdate.Id);

                var result =  await _mediator.Send(new SetAppointmentCommand(request.PatientId, doctorId, request.NewDate!.Value));

                if (result.Error != null)
                    await transaction.RollbackAsync();
                else
                    await transaction.CommitAsync();
                    await RemoveCacheIfExistsAsync(doctorId, appointmentToUpdate.AppointmentDate);

                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error updating appointment");
                return Result<AppointmentDTO, Error>.Failure(GenericErrors.AppointmentUpdateError);
            }

        }
    }
}
