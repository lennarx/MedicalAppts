using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAptts.UseCases.Helpers.Extensions;

namespace MedicalAptts.UseCases.Appointment.GetAppointmentsPerDoctor
{
    public class GetAppointmentsPerDoctorQueryHandler(IAppointmentsRepository appointmentsRepository) : IRequestHandler<GetAppointmentsPerDoctorQuery, Result<IEnumerable<AppointmentDTO>, Error>>
    {
        private readonly IAppointmentsRepository _appointmentsRepository = appointmentsRepository;
        public async Task<Result<IEnumerable<AppointmentDTO>, Error>> Handle(GetAppointmentsPerDoctorQuery request, CancellationToken cancellationToken)
        {
            var appointments = (await _appointmentsRepository.GetAppointmentsByDateAndDoctorIdAsync(request.AppointmentDate, request.DoctorId))
                .MapToAppointmentDTOs();

            return Result<IEnumerable<AppointmentDTO>, Error>.Success(appointments);
        }
    }
}
