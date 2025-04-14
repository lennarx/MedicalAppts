using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Errors;
using MedicalAppts.Core.Events;
using MedicalAptts.UseCases.Helpers.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;

namespace MedicalAptts.UseCases.Patient.CreatePatient
{
    public class CreatePatientCommandHandler(IPatientsRepository patientsRepository, ILogger logger, IMediator mediator) : IRequestHandler<CreatePatientCommand, Result<PatientDTO, Error>>
    {
        private readonly IPatientsRepository _patientsRepository = patientsRepository;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        public async Task<Result<PatientDTO, Error>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var hasher = new PasswordHasher();
            var patient = new MedicalAppts.Core.Entities.Patient
            {
                DateOfBirth = request.PatientCreationForm.DateOfBirth,
                PhoneNumber = request.PatientCreationForm.PhoneNumber,
                Email = request.PatientCreationForm.Email,
                Name = request.PatientCreationForm.Name,
                UserStatus = MedicalAppts.Core.Enums.UserStatus.ACTIVE,
                PasswordHash = hasher.HashPassword(request.PatientCreationForm.Password),
                UserRole = MedicalAppts.Core.Enums.UserRole.PATIENT
            };

            try
            {
                await _patientsRepository.AddAsync(patient);
                await _mediator.Publish(new UserCreatedEvent(patient.Email));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
                return Result<PatientDTO, Error>.Failure(GenericErrors.PatientCreationError);
            }

            return Result<PatientDTO, Error>.Success(patient.MapToPatientDTO());
        }
    }
}
