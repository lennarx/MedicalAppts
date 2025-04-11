using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;

namespace MedicalAptts.UseCases.Doctor.SetDoctorSchedule
{
    public class SetDoctorScheduleCommandHandler(IDoctorsScheduleRepository doctorsScheduleRepository) : IRequestHandler<SetDoctorScheduleCommand, Result<DoctorsScheduleDTO, Error>>
    {
        private readonly IDoctorsScheduleRepository _doctorsScheduleRepository = doctorsScheduleRepository;
        public async Task<Result<DoctorsScheduleDTO, Error>> Handle(SetDoctorScheduleCommand request, CancellationToken cancellationToken)
        {
            await _doctorsScheduleRepository.AddAsync(new DoctorSchedule
            {
                DoctorId = request.DoctorId,
                DayOfWeek = request.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            });

            return Result<DoctorsScheduleDTO, Error>.Success(new DoctorsScheduleDTO
            {
                DoctorId = request.DoctorId,
                DayOfWeek = request.DayOfWeek,
                StartTime = request.StartTime.Hours * 100 + request.StartTime.Minutes,
                EndTime = request.EndTime.Hours * 100 + request.EndTime.Minutes
            });
        }
    }
}
