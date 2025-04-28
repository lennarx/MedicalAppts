using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;
using MedicalAppts.Core.Errors;
using MedicalAptts.UseCases.Appointment.SetAppointment;
using Microsoft.EntityFrameworkCore.Storage;
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

            return await TryAppointmentAction(appointmentToUpdate, request.NewDate, transaction);
        }

        protected override async Task<Result<AppointmentDTO, Error>> PerformApptAction(MedicalAppts.Core.Entities.Appointment apptToUpdate, DateTime? newApptDate = null, IDbContextTransaction? tx = null)
        {
            await _appointmentsRepository.DeleteAsync(apptToUpdate.Id);

            var result = await _mediator.Send(new SetAppointmentCommand(apptToUpdate.PatientId, apptToUpdate.DoctorId, newApptDate!.Value));

            if (result.Error != null)
                await tx.RollbackAsync();
            else
                await tx.CommitAsync();
            
            await RemoveCacheIfExistsAsync(apptToUpdate.DoctorId, apptToUpdate.AppointmentDate);

            return result;
        }
    }
}
