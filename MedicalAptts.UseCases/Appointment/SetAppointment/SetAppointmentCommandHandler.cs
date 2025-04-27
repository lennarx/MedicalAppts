using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Errors;
using MedicalAppts.Core.Events;
using MedicalAptts.UseCases.Helpers.Extensions;
using Microsoft.Extensions.Logging;

namespace MedicalAptts.UseCases.Appointment.SetAppointment
{
    public class SetAppointmentCommandHandler(IAppointmentsRepository appointmentsRepository, IPatientsRepository patientsRepository, IDoctorsRepository doctorsRepository, IDoctorsScheduleRepository doctorsScheduleRepository,
        ILogger<SetAppointmentCommandHandler> logger, IMediator mediator) : IRequestHandler<SetAppointmentCommand, Result<AppointmentDTO, Error>>
    {
        private readonly IAppointmentsRepository _appointmentsRepository = appointmentsRepository;
        private readonly IPatientsRepository _patientRepository = patientsRepository;
        private readonly IDoctorsRepository _doctorsRepository = doctorsRepository;
        private readonly IDoctorsScheduleRepository _doctorsScheduleRepository = doctorsScheduleRepository;
        private readonly ILogger<SetAppointmentCommandHandler> _logger = logger;
        private readonly IMediator _mediator = mediator;
        public async Task<Result<AppointmentDTO, Error>> Handle(SetAppointmentCommand request, CancellationToken cancellationToken)
        {
            if (await AppointmentAlreadyExists(request.AppointmentDate, request.DoctorId))
            {
                return Result<AppointmentDTO, Error>.Failure(GenericErrors.AppointmentAlreadyExists);
            }

            if (!AppointmentIsInDoctorTimeFrame(request.AppointmentDate, request.DoctorId))
            {
                return Result<AppointmentDTO, Error>.Failure(GenericErrors.AppointmentOutOfTimeFrame);
            }

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
                var doctor = await _doctorsRepository.GetByIdAsync(request.DoctorId);
                var patient = await _patientRepository.GetByIdAsync(request.PatientId);

                await _mediator.Publish(new AppointmentCreatedEvent(doctor.Name, patient.Name, patient.Email, request.AppointmentDate.ToString("dd-mm-YYYY")));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.StackTrace);
                return Result<AppointmentDTO, Error>.Failure(GenericErrors.AppointmentRequestError);
            }

            _logger.LogInformation($"Appointment scheduled for patient {request.PatientId} with doctor {request.DoctorId} on {request.AppointmentDate}");
            return Result<AppointmentDTO, Error>.Success(apptRequest.MapToAppointmentDTO());
        }

        private bool AppointmentIsInDoctorTimeFrame(DateTime appointmentDate, int doctorId)
        {
            var doctorSchedule = _doctorsScheduleRepository.GetFiltered(x => x.DoctorId == doctorId && x.DayOfWeek == appointmentDate.DayOfWeek).FirstOrDefault();
            if (doctorSchedule == null)
            {
                return false;
            }
            var startTime = new TimeSpan(doctorSchedule.StartTime.Hours, doctorSchedule.StartTime.Minutes, 0);
            var endTime = new TimeSpan(doctorSchedule.EndTime.Hours, doctorSchedule.EndTime.Minutes, 0);
            var appointmentTime = new TimeSpan(appointmentDate.Hour, appointmentDate.Minute, 0);
            return appointmentTime >= startTime && appointmentTime <= endTime;
        }

        private async Task<bool> AppointmentAlreadyExists(DateTime apptDate, int doctorId)
        {
            var appt = (await _appointmentsRepository.GetAppointmentsByDateAndDoctorIdAsync(apptDate, doctorId)).FirstOrDefault();
            if (appt != null)
            {
                return appt.AppointmentDate.Hour == apptDate.Hour;
            }
            return false;
        }
    }
}
