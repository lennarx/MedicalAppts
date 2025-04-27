using FluentValidation;
using MedicalAptts.UseCases.Doctor;

namespace MedicalAppts.Api.Utils.Validators
{
    public class CreateDoctorScheduleFormValidator : AbstractValidator<CreateDoctorScheduleForm>
    {
        public CreateDoctorScheduleFormValidator()
        {
            RuleFor(form => form.StartTime)
                .NotNull()
                .NotEqual(form => form.EndTime)
                .LessThan(form => form.EndTime)
                    .WithMessage("Start time must be less than end time.");

            RuleFor(form => form.EndTime)
                .NotNull()
                .NotEqual(form => form.StartTime)
                .GreaterThan(form => form.StartTime)
                    .WithMessage("End time must be greater than start time.");
            RuleFor(form => form.DayOfWeek)
                .NotNull()
                .IsInEnum()
                    .WithMessage("Day of week must be a valid enum value.");
        }
    }
}
