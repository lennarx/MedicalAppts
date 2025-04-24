using MedicalAppts.Core.Errors;
using MediatR;
using Moq;
using MedicalAppts.Core.Contracts;
using MedicalAppts.Core;
using MedicalAptts.UseCases.Appointment.GetAppointmentsPerDoctor;
using MedicalAptts.UseCases.Appointment;
using MedicalAptts.UseCases.Doctor.GetAvailableSchedulePerDoctor;
using MedicalAptts.UseCases.Doctor.GetDoctorSchedule;
using MedicalAptts.UseCases.Doctor;

namespace MedicalAppts.Test.Appointment
{
    public class GetAvailableSchedulePerDoctorQueryHandlerTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly Mock<ICacheService> _cacheServiceMock = new();
        private readonly GetAvailableSchedulePerDoctorQueryHandler _handler;

        public GetAvailableSchedulePerDoctorQueryHandlerTests()
        {
            _handler = new GetAvailableSchedulePerDoctorQueryHandler(
                _mediatorMock.Object,
                _cacheServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsCachedResult_WhenAvailable()
        {
            var date = new DateTime(2025, 4, 25);
            var query = new GetAvailableSchedulePerDoctorQuery(1, date);
            var cachedResult = new List<DoctorsAvailableTimeFrameDTO> {
                new DoctorsAvailableTimeFrameDTO { DoctorId = 1, DoctorName = "Dr. A", Date = date, AvailableTimeFramesPerDay = new List<int> { 9, 10 } }
            };

            _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<DoctorsAvailableTimeFrameDTO>>($"DoctorAvailability:1:{date:yyyyMMdd}"))
                .ReturnsAsync(cachedResult);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Value is not null);
            Assert.Equal(1, result.Value.Count());
        }

        [Fact]
        public async Task Handle_ReturnsSchedules_WhenNoCacheAvailable()
        {
            var date = new DateTime(2025, 4, 25);
            var query = new GetAvailableSchedulePerDoctorQuery(1, date);

            _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<DoctorsAvailableTimeFrameDTO>>($"DoctorAvailability:1:{date:yyyyMMdd}"))
                .ReturnsAsync((IEnumerable<DoctorsAvailableTimeFrameDTO>)null);

            var schedules = new List<DoctorsScheduleDTO> {
                new DoctorsScheduleDTO { DoctorId = 1, DoctorName = "Dr. A", DayOfWeek = DayOfWeek.Friday, StartTime = 900, EndTime = 1200 }
            };
            var appointments = new List<AppointmentDTO> {
                new AppointmentDTO { AppointmentDate = new DateTime(2025, 4, 25, 10, 0, 0) }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetDoctorsScheduleQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<IEnumerable<DoctorsScheduleDTO>, Error>.Success(schedules));
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentsPerDoctorQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<IEnumerable<AppointmentDTO>, Error>.Success(appointments));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Value is not null);
            Assert.Single(result.Value);
            Assert.Contains(9, result.Value.First().AvailableTimeFramesPerDay);
            Assert.DoesNotContain(10, result.Value.First().AvailableTimeFramesPerDay);
        }

        [Fact]
        public async Task Handle_ReturnsEmpty_WhenNoSchedulesAvailable()
        {
            var date = new DateTime(2025, 4, 25);
            var query = new GetAvailableSchedulePerDoctorQuery(1, date);

            _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<DoctorsAvailableTimeFrameDTO>>($"DoctorAvailability:1:{date:yyyyMMdd}"))
                .ReturnsAsync((IEnumerable<DoctorsAvailableTimeFrameDTO>)null);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetDoctorsScheduleQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<IEnumerable<DoctorsScheduleDTO>, Error>.Success(new List<DoctorsScheduleDTO>()));
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentsPerDoctorQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<IEnumerable<AppointmentDTO>, Error>.Success(new List<AppointmentDTO>()));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Value is null);
            Assert.Equal(GenericErrors.ScheduleNotSet, result.Error);
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenDoctorAppointmentsFails()
        {
            var date = new DateTime(2025, 4, 25);
            var query = new GetAvailableSchedulePerDoctorQuery(1, date);

            var schedules = new List<DoctorsScheduleDTO> {
                new DoctorsScheduleDTO { DoctorId = 1, DoctorName = "Dr. A", DayOfWeek = DayOfWeek.Friday, StartTime = 900, EndTime = 1200 }
            };

            _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<DoctorsAvailableTimeFrameDTO>>($"DoctorAvailability:1:{date:yyyyMMdd}"))
                .ReturnsAsync((IEnumerable<DoctorsAvailableTimeFrameDTO>)null);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetDoctorsScheduleQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<IEnumerable<DoctorsScheduleDTO>, Error>.Success(schedules));
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentsPerDoctorQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<IEnumerable<AppointmentDTO>, Error>.Success(new List<AppointmentDTO>()));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Value is null);
            Assert.Equal(GenericErrors.AppointmentNotFound, result.Error);
        }
    }
}