using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Enums;
using MedicalAptts.UseCases.Appointment;
using MedicalAptts.UseCases.Appointment.CancelAppointment;
using MedicalAptts.UseCases.Appointment.GetAppointmentsPerDate;
using MedicalAptts.UseCases.Appointment.GetAppointmentsPerPatient;
using MedicalAptts.UseCases.Appointment.SetAppointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace MedicalAppts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(IMediator mediator, ILogger<AppointmentsController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{appointmentsDate}")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Retrieves appointments",
            Description = "This endpoint is used to retrieve all the appointments based on a specific date"
        )]
        [ProducesResponseType(typeof(IEnumerable<AppointmentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [Authorize(Roles = $"{nameof(UserRole.ADMIN)}")]
        public async Task<IActionResult> GetAppointmentsByDate([FromRoute] DateTime appointmentsDate)
        {
            var query = new GetAppointmentsPerDateQuery(appointmentsDate);
            var result = (await _mediator.Send(query))
                .Match(resultValue => Result<IEnumerable<AppointmentDTO>, Error>.Success(resultValue), error => error);

            if (result.Error != null)
            {
                _logger.LogError(result.Error.Message);
                return Problem(result.Error.Message, null, result.Error.HttpStatusCode);
            }
            else
            {
                _logger.LogInformation("Appointments per date retrieved successfully.");
                return Ok(result?.Value);
            }
        }

        [HttpPost]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Creates an appointment",
            Description = "This endpoint is used to create a new appointment for a given date, doctor and patient id"
        )]
        [ProducesResponseType(typeof(AppointmentDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = $"{nameof(UserRole.ADMIN)},{nameof(UserRole.PATIENT)}")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentCreationForm appointmentCreationForm)
        {            
            var command = new SetAppointmentCommand(appointmentCreationForm.PatientId, appointmentCreationForm.DoctorId, appointmentCreationForm.AppointmentDate);
            var result = (await _mediator.Send(command))
                .Match(resultValue => resultValue, error => error);

            if (result.Error != null)
            {
                _logger.LogError(result.Error.Message);
                return Problem(result.Error.Message, null, result.Error.HttpStatusCode);
            }
            else
            {
                _logger.LogInformation("Appointment created successfully.");
                return CreatedAtAction(nameof(CreateAppointment), new { id = result?.Value?.AppointmentId }, result?.Value);
            }
        }
    }
}
