using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Errors;
using MedicalAppts.Core.Events;
using MedicalAptts.UseCases.Helpers.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;

namespace MedicalAptts.UseCases.Doctor.CreateDoctor
{
    public class CreateDoctorCommandHandler(IDoctorsRepository doctorsRepository, ILogger<CreateDoctorCommandHandler> logger, IMediator mediator) : IRequestHandler<CreateDoctorCommand, Result<DoctorDTO, Error>>
    {
        private readonly IDoctorsRepository _doctorRepository = doctorsRepository;
        private readonly ILogger<CreateDoctorCommandHandler> _logger = logger;
        private readonly IMediator _mediator = mediator;
        public async Task<Result<DoctorDTO, Error>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            var hasher = new PasswordHasher();
            var doctor = new MedicalAppts.Core.Entities.Doctor
            {
                Name = request.DoctorForm.Name,
                Specialty = request.DoctorForm.Specialty,
                Email = request.DoctorForm.Email,
                PasswordHash = hasher.HashPassword(request.DoctorForm.Password),
                UserRole = MedicalAppts.Core.Enums.UserRole.DOCTOR,
                UserStatus = MedicalAppts.Core.Enums.UserStatus.ACTIVE
            };

            try
            {
                await _doctorRepository.AddAsync(doctor);
                await _mediator.Publish(new UserCreatedEvent(doctor.Email));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
                return Result<DoctorDTO, Error>.Failure(GenericErrors.DoctorCreationError);    
            }

            return Result<DoctorDTO, Error>.Success(doctor.MapToDoctorDTO());
        }
    }
}
