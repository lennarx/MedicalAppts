using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Errors;
using MedicalAppts.Core.Events;
using MedicalAptts.UseCases.Helpers.Extensions;
using Microsoft.Extensions.Logging;

namespace MedicalAptts.UseCases.Appointment.CancelAppointment
{
    public class CancelAppointmentCommandHandler(IAppointmentsRepository appointmentsRepository, IDoctorsRepository doctorsRespository, IPatientsRepository patientsRepository,
        ILogger<CancelAppointmentCommand> logger, IMediator mediator) : IRequestHandler<CancelAppointmentCommand, Result<AppointmentDTO, Error>>
    {
        private readonly IAppointmentsRepository _appointmentsRepository = appointmentsRepository;
        private readonly ILogger<CancelAppointmentCommand> _logger = logger;
        private readonly IDoctorsRepository _doctorsRepository = doctorsRespository;
        private readonly IPatientsRepository _patientsRepository = patientsRepository;
        private readonly IMediator _mediator = mediator;
        public async Task<Result<AppointmentDTO, Error>> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointmentToCancel = await _appointmentsRepository.GetByIdAsync(request.AppointmentId);

            if (appointmentToCancel is null)
            {
                _logger.LogError($"Appointment with id {request.AppointmentId} not found");
                return Result<AppointmentDTO, Error>.Failure(GenericErrors.AppointmentNotFound);
            }

            appointmentToCancel.Status = MedicalAppts.Core.Enums.AppointmentStatus.CANCELLED;

            try
            {
                await _appointmentsRepository.UpdateAsync(appointmentToCancel);

                await _mediator.Publish(new AppointmentCanceledEvent(appointmentToCancel.Doctor.Email, appointmentToCancel.Patient.Name, appointmentToCancel.Patient.Email, appointmentToCancel.AppointmentDate.ToString("dd-mm-YYYY")));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Appointment with id {request.AppointmentId} could not be cancelled");
                return Result<AppointmentDTO, Error>.Failure(GenericErrors.AppointmentCancellationError);
            }

            return Result<AppointmentDTO, Error>.Success(appointmentToCancel.MapToAppointmentDTO());
        }
    }
}
