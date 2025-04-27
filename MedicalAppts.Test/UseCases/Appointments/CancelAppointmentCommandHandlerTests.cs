using FluentAssertions;
using MediatR;
using MedicalAppts.Core.Contracts;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;
using MedicalAppts.Core.Errors;
using MedicalAppts.Core.Events;
using MedicalAptts.UseCases.Appointment;
using MedicalAptts.UseCases.Appointment.CancelAppointment;
using Microsoft.Extensions.Logging;
using Moq;

public class CancelAppointmentCommandHandlerTests
{
    private readonly Mock<IAppointmentsRepository> _appointmentsRepoMock = new();
    private readonly Mock<ILogger<UpdateAppointmentBaseCommandHandler>> _loggerMock = new();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<ICacheService> _cacheServiceMock = new();

    private readonly CancelAppointmentCommandHandler _handler;

    public CancelAppointmentCommandHandlerTests()
    {
        _handler = new CancelAppointmentCommandHandler(
            _appointmentsRepoMock.Object,
            _loggerMock.Object,
            _mediatorMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenAppointmentNotFound()
    {
        _appointmentsRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Appointment?)null);

        var command = new CancelAppointmentCommand(1, 1);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Error.Should().Be(GenericErrors.AppointmentNotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenPatientIdDoesNotMatch()
    {
        var appointment = new Appointment { Id = 1, PatientId = 99 };
        _appointmentsRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(appointment);

        var command = new CancelAppointmentCommand(1, 1);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Error.Should().Be(GenericErrors.AppointmentNotOwnedByPatient);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenUpdateFails()
    {
        var appointment = new Appointment
        {
            Id = 1,
            PatientId = 1,
            DoctorId = 2,
            AppointmentDate = DateTime.Today,
            Patient = new Patient { Name = "John", Email = "john@test.com" },
            Doctor = new Doctor { Email = "doc@test.com" }
        };
        _appointmentsRepoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(appointment);
        _appointmentsRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Appointment>())).ThrowsAsync(new Exception("fail"));

        var result = await _handler.Handle(new CancelAppointmentCommand(1, 1), CancellationToken.None);

        result.Error.Should().Be(GenericErrors.AppointmentCancellationError);
    }

    [Fact]
    public async Task Handle_Should_CancelAppointment_And_ReturnSuccess()
    {
        var appointment = new Appointment
        {
            Id = 1,
            PatientId = 1,
            DoctorId = 2,
            AppointmentDate = DateTime.Today,
            Patient = new Patient { Name = "John", Email = "john@test.com" },
            Doctor = new Doctor { Email = "doc@test.com" }
        };

        _appointmentsRepoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(appointment);
        _appointmentsRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Appointment>())).Returns(Task.CompletedTask);
        _cacheServiceMock.Setup(x => x.RemoveAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
        _mediatorMock.Setup(x => x.Publish(It.IsAny<AppointmentCanceledEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await _handler.Handle(new CancelAppointmentCommand(1, 1), CancellationToken.None);

        result.Value?.Should().NotBeNull();
        result.Value?.AppointmentId.Should().Be(appointment.Id);
        _cacheServiceMock.Verify(x => x.RemoveAsync(It.Is<string>(k => k.Contains("DoctorAvailability"))), Times.Once);
        _mediatorMock.Verify(x => x.Publish(It.IsAny<AppointmentCanceledEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
