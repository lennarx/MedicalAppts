using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAptts.UseCases.Helpers.Extensions;

namespace MedicalAptts.UseCases.Appointment.GetAppointmentsPerDate
{
    public class GetAppointmentsPerDateQueryHandler : IRequestHandler<GetAppointmentsPerDateQuery, Result<IEnumerable<AppointmentDTO>, Error>>
    {
        private readonly IAppointmentsRepository _appointmentsRepository;
        public GetAppointmentsPerDateQueryHandler(IAppointmentsRepository appointmentsRepository)
        {
            _appointmentsRepository = appointmentsRepository;
        }
        public async Task<Result<IEnumerable<AppointmentDTO>, Error>> Handle(GetAppointmentsPerDateQuery request, CancellationToken cancellationToken)
        {
            var appointments = await _appointmentsRepository.GetAppointmentsByDateAsync(request.AppointmentDate);
            return Result<IEnumerable<AppointmentDTO>, Error>.Success(appointments.MapToAppointmentDTOs());
        }
    }
}
