using MediatR;
using MedicalAppts.Api.Utils.Validators;
using MedicalAptts.UseCases.Appointment;
using MedicalAptts.UseCases.Patient;
using MedicalAptts.UseCases.Patient.CreatePatient;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MedicalAptts.UseCases.Appointment.GetAppointmentsPerPatient;
using MedicalAppts.Core;

namespace MedicalAppts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PatientsController> _logger;

        public PatientsController(IMediator mediator, ILogger<PatientsController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{patientId}/appointments")]
        [Produces("application/json")]
        [SwaggerOperation(
           Summary = "Retrieves patients appointments",
           Description = "This endpoint is used to retrieve appointments based on a patientId. Appointment date is optional"
       )]
        [ProducesResponseType(typeof(IEnumerable<AppointmentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAppointmentsPerDoctor([FromRoute] int patientId, [FromQuery] DateTime? appointmentDate)
        {
            var command = new GetAppointmentsPerPatient(patientId, appointmentDate);
            var result = (await _mediator.Send(command))
                .Match(resultValue => Result<IEnumerable<AppointmentDTO>, Error>.Success(resultValue), error => error);

            if (result.Error != null)
            {
                _logger.LogError(result.Error.Message);
                return Problem(result.Error.Message, null, result.Error.HttpStatusCode);
            }
            else
            {
                _logger.LogInformation("Appointments per patient retrieved successfully.");
                return Ok(result?.Value);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a patient",
            Description = "This endpoint is used to create a patient user in the system "
        )]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PatientDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePatient([FromBody] PatientCreationForm patientCreationForm)
        {
            var validator = new PatientCreationFormValidator();
            var validationResult = await validator.ValidateAsync(patientCreationForm);
            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation failed: {Errors}", validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            var command = new CreatePatientCommand(patientCreationForm);
            var result = (await _mediator.Send(command))
                .Match(resultValue => resultValue, error => error);

            if (result.Error != null)
            {
                _logger.LogError(result.Error.Message);
                return Problem(result.Error.Message, null, result.Error.HttpStatusCode);
            }
            else
            {
                _logger.LogInformation("Patient created successfully.");
                return CreatedAtAction(nameof(CreatePatient), new { id = result?.Value?.PatientId }, result?.Value);
            }
        }
    }
}
