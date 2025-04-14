using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAptts.UseCases.Helpers.Extensions;

namespace MedicalAptts.UseCases.Appointment.GetAppointmentsPerPatient
{
    public class GetAppointmentPerPatientQueryHandler(IAppointmentsRepository appointmentsRepository) : IRequestHandler<GetAppointmentsPerPatient, Result<IEnumerable<AppointmentDTO>, Error>>
    {
        private readonly IAppointmentsRepository _appointmentRepository = appointmentsRepository;
        public async Task<Result<IEnumerable<AppointmentDTO>, Error>> Handle(GetAppointmentsPerPatient request, CancellationToken cancellationToken)
        {
            var appointments = await _appointmentRepository.GetAppointmentsByDateAndPatientIdAsync(request.AppointmentDate, request.PatientId);
            return Result<IEnumerable<AppointmentDTO>, Error>.Success(appointments.MapToAppointmentDTOs());
        }
    }
}
