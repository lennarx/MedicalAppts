using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;
using MedicalAptts.UseCases.Doctor.SetDoctorSchedule;
using Moq;

namespace MedicalAppts.Test.UnitTests.UseCases.Doctors
{
    public class SetDoctorScheduleCommandHandlerTests
    {
        private readonly Mock<IDoctorsScheduleRepository> _scheduleRepoMock = new();
        private readonly SetDoctorScheduleCommandHandler _handler;

        public SetDoctorScheduleCommandHandlerTests()
        {
            _handler = new SetDoctorScheduleCommandHandler(_scheduleRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsSuccess_WithCorrectDTO()
        {
            var doctorId = 1;
            var command = new SetDoctorScheduleCommand(doctorId, new MedicalAptts.UseCases.Doctor.CreateDoctorScheduleForm
            {
                DayOfWeek = DayOfWeek.Monday,
                StartTime = new TimeSpan(9, 30, 0),
                EndTime = new TimeSpan(17, 0, 0)
            });

            _scheduleRepoMock
                .Setup(r => r.AddAsync(It.IsAny<DoctorSchedule>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Value is not null);
            Assert.Equal(command.DoctorId, result.Value.DoctorId);
            Assert.Equal(command.DayOfWeek, result.Value.DayOfWeek);
            Assert.Equal(930, result.Value.StartTime);
            Assert.Equal(1700, result.Value.EndTime);
        }

        [Fact]
        public async Task Handle_ThrowsException_ReturnsFailure()
        {
            var doctorId = 2;
            var command = new SetDoctorScheduleCommand(doctorId, new MedicalAptts.UseCases.Doctor.CreateDoctorScheduleForm
            {
                DayOfWeek = DayOfWeek.Tuesday,
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(12, 0, 0)
            });

            _scheduleRepoMock
                .Setup(r => r.AddAsync(It.IsAny<DoctorSchedule>()))
                .ThrowsAsync(new Exception("Database error"));

            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
