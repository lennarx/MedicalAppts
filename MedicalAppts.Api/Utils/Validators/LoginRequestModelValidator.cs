using FluentValidation;
using MedicalAptts.UseCases.Users;

namespace MedicalAppts.Api.Utils.Validators
{
    public class LoginRequestModelValidator : AbstractValidator<LoginForm>
    {
        public LoginRequestModelValidator()
        {
            RuleFor(model => model.UserId)
             .NotEmpty();

            RuleFor(model => model.Email)
             .NotEmpty()
                .WithMessage("Email is required.")
                .MaximumLength(30)
                .WithMessage("Email has a 30 characters limit.")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .WithMessage("Email format is invalid");

            RuleFor(model => model.Password)
                .NotEmpty()
                .WithMessage("Passwordis required.");

            RuleFor(model => model.UserRole)
                .NotEmpty()
                .WithMessage("User role is required.");
        }
    }
}
