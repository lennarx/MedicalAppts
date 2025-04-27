using MedicalAptts.UseCases.Appointment.CancelAppointment;
using MedicalAptts.UseCases.Appointment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using MedicalAptts.UseCases.Appointment.GetAppointmentsPerDoctor;
using MedicalAppts.Core;
using Microsoft.AspNetCore.Authorization;
using MedicalAptts.UseCases.Doctor;
using MedicalAptts.UseCases.Doctor.CreateDoctor;
using MedicalAptts.UseCases.Doctor.GetAvailableSchedulePerDoctor;
using MedicalAppts.Core.Enums;
using MedicalAptts.UseCases.Doctor.GetDoctorBySpecialty;
using MedicalAptts.UseCases.Doctor.SetDoctorSchedule;
using MedicalAptts.UseCases.Doctor.GetDoctorSchedule;

namespace MedicalAppts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMediator _mediator;

        public DoctorsController(IMediator mediator, ILogger<DoctorsController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{specialty}")]
        [Produces("application/json")]
        [SwaggerOperation(
           Summary = "Retrieves doctors per speciality",
           Description = "This endpoint is used to retrieve doctors per speciality"
       )]
        [ProducesResponseType(typeof(IEnumerable<DoctorDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDoctorsPerSpeciality([FromRoute] string specialty)
        {
            if (!Enum.TryParse<MedicalSpecialty>(specialty, true, out var specialtyEnum))
            {
                return BadRequest($"Invalid specialty: {specialty}");
            }

            var command = new GetDoctorsBySpecialtyQuery(specialtyEnum);
            var result = (await _mediator.Send(command))
                .Match(resultValue => Result<IEnumerable<DoctorDTO>, Error>.Success(resultValue), error => error);

            if (result.Error != null)
            {
                _logger.LogError(result.Error.Message);
                return Problem(result.Error.Message, null, result.Error.HttpStatusCode);
            }
            else
            {
                _logger.LogInformation($"Doctors per speciality: {specialty} retrieved succesfully");
                return Ok(result?.Value);
            }
        }

        [HttpGet("{doctorId}/appointments")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Retrieves doctors appointments",
            Description = "This endpoint is used to retrieve appointments based on a doctorId. Appointment date is optional"
        )]
        [ProducesResponseType(typeof(IEnumerable<AppointmentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [Authorize(Roles = $"{nameof(UserRole.ADMIN)},{nameof(UserRole.DOCTOR)}")]
        public async Task<IActionResult> GetAppointmentsPerDoctor([FromRoute] int doctorId, [FromQuery] DateTime? appointmentDate)
        {
            var command = new GetAppointmentsPerDoctorQuery(doctorId, appointmentDate);
            var result = (await _mediator.Send(command))
                .Match(resultValue => Result<IEnumerable<AppointmentDTO>, Error>.Success(resultValue), error => error);

            _logger.LogInformation("Appointments per doctor retrieved successfully.");
            return Ok(result?.Value);
        }

        [HttpGet("{doctorId}/availableTimeFrame")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Retrieves doctors availability",
            Description = "This endpoint is used to retrieve doctors availability based on doctorId. AppointmentDate is optional"
        )]
        [ProducesResponseType(typeof(IEnumerable<DoctorsAvailableTimeFrameDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAvailableTimeFrameByDoctor([FromRoute] int doctorId, [FromQuery] DateTime? appointmentDate)
        {
            var command = new GetAvailableSchedulePerDoctorQuery(doctorId, appointmentDate);
            var result = (await _mediator.Send(command))
                .Match(resultValue => Result<IEnumerable<DoctorsAvailableTimeFrameDTO>, Error>.Success(resultValue), error => error);

            if (result.Error != null)
            {
                _logger.LogError(result.Error.Message);
                return Problem(result.Error.Message, null, result.Error.HttpStatusCode);
            }
            else
            {
                _logger.LogInformation("Availability per doctor retrieved successfully.");
                return Ok(result?.Value);
            }
        }

        [HttpGet("{doctorId}/schedule")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Retrieves doctors schedule",
            Description = "This endpoint is used to retrieve doctors availability based on doctorId. AppointmentDate is optional"
        )]
        [ProducesResponseType(typeof(IEnumerable<DoctorsScheduleDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDoctorScheduleByDoctor([FromRoute] int doctorId, [FromQuery] DateTime? appointmentDate)
        {
            var command = new GetDoctorsScheduleQuery(doctorId, appointmentDate);
            var result = (await _mediator.Send(command))
                .Match(resultValue => Result<IEnumerable<DoctorsScheduleDTO>, Error>.Success(resultValue), error => error);

            _logger.LogInformation("Schedule per doctor retrieved successfully.");
            return Ok(result?.Value);
        }       

        [HttpPost]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Creates a Doctor",
            Description = "This endpoint is used to create a new doctor"
        )]
        [ProducesResponseType(typeof(DoctorDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [Authorize(Roles = $"{nameof(UserRole.ADMIN)}")]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorCreationForm createDoctorForm)
        {
            var command = new CreateDoctorCommand(createDoctorForm);
            var result = (await _mediator.Send(command))
                .Match(resultValue => resultValue, error => error);

            if (result.Error != null)
            {
                _logger.LogError(result.Error.Message);
                return Problem(result.Error.Message, null, result.Error.HttpStatusCode);
            }
            else
            {
                _logger.LogInformation("Doctor created successfully.");
                return CreatedAtAction(nameof(CreateDoctor), new { id = result?.Value?.DoctorId }, result?.Value);
            }
        }

        [HttpPost("{doctorId}/schedule")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Creates a Doctor Schedule",
            Description = "This endpoint is used to create a doctor schedule"
        )]
        [ProducesResponseType(typeof(DoctorsScheduleDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [Authorize(Roles = $"{nameof(UserRole.ADMIN)}, {nameof(UserRole.DOCTOR)}")]
        public async Task<IActionResult> CreateDoctorSchedule([FromRoute] int doctorId, [FromBody] CreateDoctorScheduleForm createDoctorScheduleForm)
        {
            var command = new SetDoctorScheduleCommand(doctorId, createDoctorScheduleForm);
            var result = (await _mediator.Send(command))
                .Match(resultValue => resultValue, error => error);

            if (result.Error != null)
            {
                _logger.LogError(result.Error.Message);
                return Problem(result.Error.Message, null, result.Error.HttpStatusCode);
            }
            else
            {
                _logger.LogInformation("Doctor schedule created successfully.");
                return CreatedAtAction(nameof(CreateDoctorSchedule), new { id = result?.Value?.DoctorId }, result?.Value);
            }
        }
    }
}
