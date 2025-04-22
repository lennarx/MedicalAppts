using MediatR;
using MedicalAptts.UseCases.Users;
using MedicalAptts.UseCases.Users.Login;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MedicalAppts.Api.Utils.Validators;

namespace MedicalAppts.Api.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a patient",
            Description = "This endpoint is used to get the authorization token for using the other endpoints, using the " +
            "User Email and Password to authenticate. If the password is inputted 3 times wrong the user will be rendered inactive. ",
            OperationId = "login"
        )]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginForm loginForm)
        {
            var validator = new LoginRequestModelValidator();
            var validationResult = await validator.ValidateAsync(loginForm);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var loginCommand = new LoginCommand(loginForm.Email, loginForm.Password);
            var loginResult = (await _mediator.Send(loginCommand, CancellationToken.None))
                            .Match(resultValue => resultValue, error => error);

            if (loginResult.Error != null)
                return Problem(loginResult.Error.Message, null, loginResult.Error.HttpStatusCode);
            else
                return Ok(loginResult.Value);
        }
    }
}
