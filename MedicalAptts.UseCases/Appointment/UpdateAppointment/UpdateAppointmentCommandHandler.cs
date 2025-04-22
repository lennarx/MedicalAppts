using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Errors;
using MedicalAptts.UseCases.Appointment.SetAppointment;
using Microsoft.Extensions.Logging;

namespace MedicalAptts.UseCases.Appointment.UpdateAppointment
{
    public class UpdateAppointmentCommandHandler(IAppointmentsRepository appointmentsRepository, IMediator mediator, ILogger<UpdateAppointmentCommandHandler> logger)
        : IRequestHandler<UpdateAppointmentCommand, Result<AppointmentDTO, Error>>
    {
        private readonly IAppointmentsRepository _appointmentsRepository = appointmentsRepository;
        private readonly ILogger<UpdateAppointmentCommandHandler> _logger = logger;
        private readonly IMediator _mediator = mediator;
        public async Task<Result<AppointmentDTO, Error>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointmentToUpdate = await _appointmentsRepository.GetByIdAsync(request.AppointmentId);

            if (appointmentToUpdate is null)
            {
                _logger.LogError($"Appointment with id {request.AppointmentId} not found");
                return Result<AppointmentDTO, Error>.Failure(GenericErrors.AppointmentNotFound);
            }
            if (appointmentToUpdate.PatientId != request.PatientId)
            {
                _logger.LogError($"Patient with id {request.PatientId} is not the owner of the appointment with id {request.AppointmentId}");
                return Result<AppointmentDTO, Error>.Failure(GenericErrors.AppointmentNotOwnedByPatient);
            }
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
