using FluentValidation;
using MedicalAptts.UseCases.Patient;

namespace MedicalAppts.Api.Utils.Validators
{
    public class PatientCreatiionFormValidator : AbstractValidator<PatientCreationForm>
    {
        public PatientCreatiionFormValidator()
        {
            RuleFor(form => form.Email)
             .NotEmpty()
                .WithMessage("Email is required.")
                .MaximumLength(30)
                .WithMessage("Email has a 30 characters limit.")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .WithMessage("Email format is invalid");

            RuleFor(form => form.Password)
             .NotEmpty()
                .WithMessage("Password is required.");

            RuleFor(form => form.DateOfBirth)
             .NotEmpty()
                .WithMessage("Date of birth is required.");

            RuleFor(form => form.PhoneNumber)
             .NotEmpty()
                .WithMessage("Phone Number is required.");
        }
    }
}
