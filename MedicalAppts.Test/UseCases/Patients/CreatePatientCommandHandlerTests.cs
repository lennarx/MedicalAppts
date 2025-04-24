using MedicalAppts.Core.Errors;
using MediatR;
using Moq;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Events;
using MedicalAptts.UseCases.Patient.CreatePatient;
using MedicalAptts.UseCases.Patient;
using Microsoft.Extensions.Logging;

namespace MedicalAppts.Test.Patient
{
    public class CreatePatientCommandHandlerTests
    {
        private readonly Mock<IPatientsRepository> _patientsRepositoryMock = new();
        private readonly Mock<ILogger<CreatePatientCommandHandler>> _loggerMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly CreatePatientCommandHandler _handler;

        public CreatePatientCommandHandlerTests()
        {
            _handler = new CreatePatientCommandHandler(
                _patientsRepositoryMock.Object,
                _loggerMock.Object,
                _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenPatientCreated()
        {
            var request = new CreatePatientCommand(new PatientCreationForm
            {
                Name = "Jane Doe",
                Email = "jane@example.com",
                PhoneNumber = "123456789",
                Password = "securepwd",
                DateOfBirth = new DateTime(1990, 1, 1)
            });

            _patientsRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Core.Entities.Patient>())).Returns(Task.CompletedTask);
            _mediatorMock.Setup(m => m.Publish(It.IsAny<UserCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.True(result.Value is not null);
            Assert.Equal("Jane Doe", result.Value.Name);
            Assert.Equal("jane@example.com", result.Value.Email);
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenExceptionIsThrown()
        {
            var request = new CreatePatientCommand(new PatientCreationForm
            {
                Name = "Jane Doe",
                Email = "jane@example.com",
                PhoneNumber = "123456789",
                Password = "securepwd",
                DateOfBirth = new DateTime(1990, 1, 1)
            });

            _patientsRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Core.Entities.Patient>())).ThrowsAsync(new Exception("DB Error"));

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.False(result.Value is not null);
            Assert.Equal(GenericErrors.PatientCreationError, result.Error);
        }
    }
}
