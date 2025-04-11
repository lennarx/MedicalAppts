using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Enums;

namespace MedicalAptts.UseCases.Doctor.GetDoctorBySpecialty
{
    public class GetDoctorsBySpecialtyQuery : IRequest<Result<IEnumerable<DoctorDTO>, Error>>
    {
        public MedicalSpecialty Specialty { get; }
        public GetDoctorsBySpecialtyQuery(MedicalSpecialty specialty)
        {
            Specialty = specialty;
        }
    }
}
