using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Errors;
using MedicalAppts.Core.Events;
using MedicalAptts.UseCases.Helpers.Extensions;
using Microsoft.Extensions.Logging;

namespace MedicalAptts.UseCases.Appointment.SetAppointment
{
    public class SetAppointmentCommandHandler(IAppointmentsRepository appointmentsRepository, IPatientsRepository patientsRepository, IDoctorsRepository doctorsRepository, ILogger logger, IMediator mediator) : IRequestHandler<SetAppointmentCommand, Result<AppointmentDTO, Error>>
    {
        private readonly IAppointmentsRepository _appointmentsRepository = appointmentsRepository;
        private readonly IPatientsRepository _patientRepository = patientsRepository;
        private readonly IDoctorsRepository _doctorsRepository = doctorsRepository;
        private readonly ILogger _logger = logger;
        private readonly IMediator _mediator = mediator;
        public async Task<Result<AppointmentDTO, Error>> Handle(SetAppointmentCommand request, CancellationToken cancellationToken)
        {
            var apptRequest = new MedicalAppts.Core.Entities.Appointment
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                AppointmentDate = request.AppointmentDate,
                Status = MedicalAppts.Core.Enums.AppointmentStatus.SCHEDULED
            };

            try
            {
                await _appointmentsRepository.AddAsync(apptRequest);
                var doctorTask = _doctorsRepository.GetByIdAsync(request.DoctorId);
                var patientTask = _patientRepository.GetByIdAsync(request.PatientId);

                await Task.WhenAll(doctorTask, patientTask);

                await _mediator.Publish(new AppointmentCreatedEvent(doctorTask.Result.Email, patientTask.Result.Name, patientTask.Result.Email, request.AppointmentDate.ToString("dd-mm-YYYY")));               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.StackTrace);
                return Result<AppointmentDTO, Error>.Failure(GenericErrors.AppointmentRequestError);
            }

            _logger.LogInformation($"Appointment scheduled for patient {request.PatientId} with doctor {request.DoctorId} on {request.AppointmentDate}");
            return Result<AppointmentDTO, Error>.Success(apptRequest.MapToAppointmentDTO());
        }
    }
}
