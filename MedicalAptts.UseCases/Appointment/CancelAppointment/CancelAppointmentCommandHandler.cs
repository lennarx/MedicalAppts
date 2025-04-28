using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Errors;
using MedicalAppts.Core.Events;
using MedicalAptts.UseCases.Doctor;
using MedicalAptts.UseCases.Helpers.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace MedicalAptts.UseCases.Appointment.CancelAppointment
{
    public class CancelAppointmentCommandHandler : UpdateAppointmentBaseCommandHandler, IRequestHandler<CancelAppointmentCommand, Result<AppointmentDTO, Error>>
    {
        public CancelAppointmentCommandHandler(IAppointmentsRepository appointmentsRepository, ILogger<UpdateAppointmentBaseCommandHandler> logger,
            IMediator mediator, ICacheService cacheService) : base(appointmentsRepository, logger, mediator, cacheService) { }
        public async Task<Result<AppointmentDTO, Error>> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await PerformInitialValidation(request);

            if (validationResult.ErrorResult != null)
            {
                return validationResult.ErrorResult;
            }
            var appointmentToCancel = validationResult.Appointment;

            appointmentToCancel.Status = MedicalAppts.Core.Enums.AppointmentStatus.CANCELLED;

            await RemoveCacheIfExistsAsync(appointmentToCancel.DoctorId, appointmentToCancel.AppointmentDate);

            return await TryAppointmentAction(appointmentToCancel);
        }

        protected override async Task<Result<AppointmentDTO, Error>> PerformApptAction(MedicalAppts.Core.Entities.Appointment apptToUpdate, DateTime? appointmentDate = null, IDbContextTransaction? tx = null)
        {
            await _appointmentsRepository.UpdateAsync(apptToUpdate);

            await _mediator.Publish(new AppointmentCanceledEvent(apptToUpdate.Doctor.Email, apptToUpdate.Patient.Name, apptToUpdate.Patient.Email, apptToUpdate.AppointmentDate.ToString("dd-mm-YYYY")));

            return Result<AppointmentDTO, Error>.Success(apptToUpdate.MapToAppointmentDTO());
        }
    }
}
