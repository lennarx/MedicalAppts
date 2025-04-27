using FluentValidation;
using MedicalAptts.UseCases.Appointment;

namespace MedicalAppts.Api.Utils.Validators
{
    public class UpdateAppointmentFormValidator : AbstractValidator<UpdateAppointmentForm>
    {
        public UpdateAppointmentFormValidator() 
        {
            RuleFor(form => form.Action)
                .NotNull()
                    .WithMessage("Action is required.")
                    .IsInEnum()
                    .WithMessage("Action must be a valid enum value.");
        }
    }
}
