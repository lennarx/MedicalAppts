using FluentValidation;
using MedicalAptts.UseCases.Doctor;

namespace MedicalAppts.Api.Utils.Validators
{
    public class DoctorCreationFormValidator : AbstractValidator<DoctorCreationForm>
    {
        public DoctorCreationFormValidator()
        {
            RuleFor(form => form.Name)
                .NotEmpty()
                    .WithMessage("Name is required.")
                    .MaximumLength(50)
                    .WithMessage("Name has a 30 characters limit.");

            RuleFor(form => form.Specialty)
                .NotNull()
                    .WithMessage("Specialty is required.")
                    .IsInEnum()
                    .WithMessage("Specialty must be a valid enum value.");

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
        }
    }
}
