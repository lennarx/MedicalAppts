using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAptts.UseCases.Appointment;
using MedicalAptts.UseCases.Appointment.GetAppointmentsPerDoctor;
using MedicalAptts.UseCases.Doctor.GetDoctorSchedule;

namespace MedicalAptts.UseCases.Doctor.GetAvailableSchedulePerDoctor
{
    public class GetAvailableSchedulePerDoctorQueryHandler(IMediator mediator) : IRequestHandler<GetAvailableSchedulePerDoctorQuery, Result<IEnumerable<DoctorsAvailableTimeFrameDTO>, Error>>
    {
        private readonly IMediator _mediator = mediator;

        public async Task<Result<IEnumerable<DoctorsAvailableTimeFrameDTO>, Error>> Handle(GetAvailableSchedulePerDoctorQuery request, CancellationToken cancellationToken)
        {
            var doctorScheduleResultTask = _mediator.Send(new GetDoctorsScheduleQuery(request.DoctorId, request.AppointmentDate), cancellationToken);
            var doctorsAppointmentResultTask = _mediator.Send(new GetAppointmentsPerDoctorQuery(request.DoctorId, request.AppointmentDate), cancellationToken);

            await Task.WhenAll(doctorScheduleResultTask, doctorsAppointmentResultTask);

            var doctorSchedules = doctorScheduleResultTask.Result.Value;
            var doctorAppointments = doctorsAppointmentResultTask.Result.Value;

            var freeSchedules = doctorSchedules.SelectMany(schedule => FilterIfApptIsAlreadyBooked(schedule, doctorAppointments));

            return Result<IEnumerable<DoctorsAvailableTimeFrameDTO>, Error>.Success(freeSchedules);
        }

        private IEnumerable<DoctorsAvailableTimeFrameDTO> FilterIfApptIsAlreadyBooked(DoctorsScheduleDTO schedule, IEnumerable<AppointmentDTO>? doctorAppointments)
        {
            return doctorAppointments
                .Where(appt => appt.AppointmentDate.Date.DayOfWeek == schedule.DayOfWeek).GroupBy(x => x.AppointmentDate)
                .Select(y => new DoctorsAvailableTimeFrameDTO
                {
                    Date = y.Key,
                    DoctorName = schedule.DoctorName,
                    DoctorId = schedule.DoctorId,
                    AvailableTimeFramesPerDay = FilterBookedTimeFrames(y.Select(x => x.AppointmentDate.Hour), schedule.StartTime, schedule.EndTime)
                });
        }

        private IEnumerable<double> FilterBookedTimeFrames(IEnumerable<int> apptHours, double startTime, double endTime)
        {
            var availableTimeFrames = new List<double>();
            for (double hour = startTime; hour < endTime; hour += 0.5)
            {
                if (!apptHours.Contains((int)hour))
                {
                    availableTimeFrames.Add(hour);
                }
            }
            return availableTimeFrames;
        }
    }
}
