using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Patient.CreatePatient
{
    public class CreatePatientCommand : IRequest<Result<PatientDTO, Error>>
    {
        public PatientCreationForm PatientCreationForm { get; }
        public CreatePatientCommand(PatientCreationForm patientCreationForm)
        {
            PatientCreationForm = patientCreationForm;
        }
    }
}
