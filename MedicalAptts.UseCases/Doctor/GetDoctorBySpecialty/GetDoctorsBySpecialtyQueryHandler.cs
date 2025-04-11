using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Errors;
using MedicalAptts.UseCases.Helpers.Extensions;

namespace MedicalAptts.UseCases.Doctor.GetDoctorBySpecialty
{
    public class GetDoctorsBySpecialtyQueryHandler(IDoctorsRepository doctorsRepository) : IRequestHandler<GetDoctorsBySpecialtyQuery, Result<IEnumerable<DoctorDTO>, Error>>
    {
        private readonly IDoctorsRepository _doctorsRepository = doctorsRepository;
        public async Task<Result<IEnumerable<DoctorDTO>, Error>> Handle(GetDoctorsBySpecialtyQuery request, CancellationToken cancellationToken)
        {
            var doctors = await _doctorsRepository.GetDoctorsBySpecialtyAsync(request.Specialty);

            if (!doctors.Any())
            {
                return Result<IEnumerable<DoctorDTO>, Error>.Failure(GenericErrors.NoSpecialtyDoctorsFound);
            }

            return Result<IEnumerable<DoctorDTO>, Error>.Success(doctors.MapToDoctorDTOs());
        }
    }
}
