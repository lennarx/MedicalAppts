using MedicalAppts.Core.Entities;
using MedicalAppts.Core.Errors;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Events;
using MedicalAptts.UseCases.Doctor.CreateDoctor;
using MedicalAptts.UseCases.Doctor;
using MedicalAppts.Core.Enums;

namespace MedicalAppts.Test.UnitTests.UseCases.Doctors
{
    public class CreateDoctorCommandHandlerTests
    {
        private readonly Mock<IDoctorsRepository> _doctorsRepositoryMock = new();
        private readonly Mock<ILogger<CreateDoctorCommandHandler>> _loggerMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly CreateDoctorCommandHandler _handler;

        public CreateDoctorCommandHandlerTests()
        {
            _handler = new CreateDoctorCommandHandler(
                _doctorsRepositoryMock.Object,
                _loggerMock.Object,
                _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenDoctorIsCreated()
        {
            var command = new CreateDoctorCommand(
                new DoctorCreationForm
                {
                    Name = "Dr. Smith",
                    Specialty = MedicalSpecialty.CARDIOLOGIST,
                    Email = "drsmith@example.com",
                    Password = "securePassword"
                });

            _doctorsRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Core.Entities.Doctor>())).Returns(Task.CompletedTask);
            _mediatorMock.Setup(m => m.Publish(It.IsAny<UserCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Value is not null);
            Assert.Equal("Dr. Smith", result.Value.Name);
            Assert.Equal(MedicalSpecialty.CARDIOLOGIST, result.Value.Specialty);
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenExceptionIsThrown()
        {
            var command = new CreateDoctorCommand(
                new DoctorCreationForm
                {
                    Name = "Dr. Smith",
                    Specialty = MedicalSpecialty.CARDIOLOGIST,
                    Email = "drsmith@example.com",
                    Password = "securePassword"
                });

            _doctorsRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Core.Entities.Doctor>())).ThrowsAsync(new Exception("DB error"));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Value is not null);
            Assert.Equal(GenericErrors.DoctorCreationError, result.Error);
        }
    }
}
