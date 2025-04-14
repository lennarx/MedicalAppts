using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Doctor.CreateDoctor
{
    public class CreateDoctorCommand : IRequest<Result<DoctorDTO, Error>>
    {
        public CreateDoctorForm DoctorForm { get; }
        public CreateDoctorCommand(CreateDoctorForm doctorForm)
        {
            DoctorForm = doctorForm;
        }
    }
}
