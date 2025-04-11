using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAptts.UseCases.Helpers.Extensions;

namespace MedicalAptts.UseCases.Doctor.GetDoctorSchedule
{
    public class GetDoctorsScheduleQueryHandler(IDoctorsScheduleRepository doctorsScheduleRepository) : IRequestHandler<GetDoctorsScheduleQuery, Result<IEnumerable<DoctorsScheduleDTO>, Error>>
    {
        private readonly IDoctorsScheduleRepository _doctorsScheduleRepository = doctorsScheduleRepository;
        public async Task<Result<IEnumerable<DoctorsScheduleDTO>, Error>> Handle(GetDoctorsScheduleQuery request, CancellationToken cancellationToken)
        {
            return Result<IEnumerable<DoctorsScheduleDTO>, Error>.Success(request.AppointmentDate == null ? (await _doctorsScheduleRepository.GetSchedulesByDoctorIdAsync(request.DoctorId)).MapToDoctorsScheduleDTOs()
                : (await _doctorsScheduleRepository.GetSchedulesByDateAndDoctorIdAsync(request.AppointmentDate.Value.DayOfWeek, request.DoctorId)).MapToDoctorsScheduleDTOs());
        }       
    }
}
