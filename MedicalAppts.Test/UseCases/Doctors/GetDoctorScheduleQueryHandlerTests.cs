using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;
using MedicalAptts.UseCases.Doctor.GetDoctorSchedule;
using Moq;

namespace MedicalAppts.Test.Doctor
{
    public class GetDoctorsScheduleQueryHandlerTests
    {
        private readonly Mock<IDoctorsScheduleRepository> _scheduleRepoMock = new();
        private readonly GetDoctorsScheduleQueryHandler _handler;

        public GetDoctorsScheduleQueryHandlerTests()
        {
            _handler = new GetDoctorsScheduleQueryHandler(_scheduleRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsAllSchedules_WhenAppointmentDateIsNull()
        {
            var query = new GetDoctorsScheduleQuery(1, null);

            var schedules = new List<DoctorSchedule>
            {
                new() { DoctorId = 1, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(17, 0, 0) , Doctor = new Core.Entities.Doctor { Id = 1, Name = "John Smith" }}
            };

            _scheduleRepoMock
                .Setup(r => r.GetSchedulesByDoctorIdAsync(1))
                .ReturnsAsync(schedules);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Value is not null);
            Assert.Single(result.Value);
            Assert.Equal(DayOfWeek.Monday, result.Value.First().DayOfWeek);
        }

        [Fact]
        public async Task Handle_ReturnsFilteredSchedules_WhenAppointmentDateIsProvided()
        {
            var date = new DateTime(2025, 4, 28);
            var query = new GetDoctorsScheduleQuery(1, date);

            var schedules = new List<DoctorSchedule>
            {
                new() { DoctorId = 1, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(15, 0, 0), Doctor = new Core.Entities.Doctor { Id = 1, Name = "John Smith"}},
                new() { DoctorId = 1, DayOfWeek = DayOfWeek.Tuesday, StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(12, 0, 0), Doctor = new Core.Entities.Doctor { Id = 1, Name = "John Smith"}}
            };

            _scheduleRepoMock
                .Setup(r => r.GetSchedulesByDateAndDoctorIdAsync(date.DayOfWeek, 1))
                .ReturnsAsync(schedules.Where(s => s.DayOfWeek == DayOfWeek.Monday).ToList());

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Value is not null);
            Assert.Single(result.Value);
            Assert.Equal(DayOfWeek.Monday, result.Value.First().DayOfWeek);
        }
    }
}