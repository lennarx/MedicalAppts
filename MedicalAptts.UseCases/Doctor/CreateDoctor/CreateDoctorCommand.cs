using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Doctor.CreateDoctor
{
    public class CreateDoctorCommand : IRequest<Result<DoctorDTO, Error>>
    {
        public DoctorCreationForm DoctorForm { get; }
        public CreateDoctorCommand(DoctorCreationForm doctorForm)
        {
            DoctorForm = doctorForm;
        }
    }
}
