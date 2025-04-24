using MedicalAppts.Core.Enums;
using Moq;
using MediatR;
using Microsoft.Extensions.Logging;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Contracts;
using MedicalAppts.Core;
using MedicalAptts.UseCases.Appointment.SetAppointment;
using MedicalAptts.UseCases.Appointment.UpdateAppointment;
using MedicalAptts.UseCases.Appointment;
using Microsoft.EntityFrameworkCore.Storage;
using MedicalAppts.Core.Errors;

namespace MedicalAppts.Test.Appointment
{
    public class UpdateAppointmentCommandHandlerTests
    {
        private readonly Mock<IAppointmentsRepository> _appointmentsRepositoryMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly Mock<ICacheService> _cacheServiceMock = new();
        private readonly UpdateAppointmentCommandHandler _handler;

        public UpdateAppointmentCommandHandlerTests()
        {
            _handler = new UpdateAppointmentCommandHandler(
                _appointmentsRepositoryMock.Object,
                _mediatorMock.Object,
                Mock.Of<ILogger<UpdateAppointmentCommandHandler>>(),
                _cacheServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenAppointmentIsUpdated()
        {
            var appointmentId = 1;
            var patientId = 1;
            var doctorId = 2;
            var appointmentDate = new DateTime(2025, 5, 1);
            var newDate = new DateTime(2025, 5, 2);
            var request = new UpdateAppointmentCommand(appointmentId, patientId, newDate);

            var appointment = new Core.Entities.Appointment
            {
                Id = appointmentId,
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentDate = appointmentDate
            };

            _appointmentsRepositoryMock.Setup(r => r.GetByIdAsync(appointmentId))
                .ReturnsAsync(appointment);

            _appointmentsRepositoryMock.Setup(r => r.BeginTransactionAsync())
                .ReturnsAsync(Mock.Of<IDbContextTransaction>());

            _appointmentsRepositoryMock.Setup(r => r.DeleteAsync(appointmentId))
                .Returns(Task.CompletedTask);

            _mediatorMock.Setup(m => m.Send(It.IsAny<SetAppointmentCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<AppointmentDTO, Error>.Success(new AppointmentDTO { Status = AppointmentStatus.SCHEDULED }));

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.True(result.Value is not null);
            Assert.Equal(AppointmentStatus.SCHEDULED, result.Value.Status);
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenAppointmentNotFound()
        {
            var request = new UpdateAppointmentCommand(1, 1, DateTime.Now);
            _appointmentsRepositoryMock.Setup(r => r.GetByIdAsync(request.AppointmentId))
                .ReturnsAsync((Core.Entities.Appointment)null!);

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.False(result.Value is not null);
            Assert.Equal(GenericErrors.AppointmentNotFound, result.Error);
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenPatientIsNotOwner()
        {
            var request = new UpdateAppointmentCommand(1, 99, DateTime.Now);
            var appointment = new Core.Entities.Appointment { Id = 1, PatientId = 1 };

            _appointmentsRepositoryMock.Setup(r => r.GetByIdAsync(request.AppointmentId))
                .ReturnsAsync(appointment);

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.False(result.Value is not null);
            Assert.Equal(GenericErrors.AppointmentNotOwnedByPatient, result.Error);
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenSetAppointmentFails()
        {
            var request = new UpdateAppointmentCommand(1, 1, DateTime.Now);
            var appointment = new Core.Entities.Appointment { Id = 1, PatientId = 1, DoctorId = 2, AppointmentDate = DateTime.Now };

            _appointmentsRepositoryMock.Setup(r => r.GetByIdAsync(request.AppointmentId)).ReturnsAsync(appointment);
            _appointmentsRepositoryMock.Setup(r => r.BeginTransactionAsync())
                .ReturnsAsync(Mock.Of<IDbContextTransaction>());
            _appointmentsRepositoryMock.Setup(r => r.DeleteAsync(appointment.Id)).Returns(Task.CompletedTask);
            _mediatorMock.Setup(m => m.Send(It.IsAny<SetAppointmentCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<AppointmentDTO, Error>.Failure(GenericErrors.AppointmentRequestError));

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.False(result.Value is not null);
            Assert.Equal(GenericErrors.AppointmentRequestError, result.Error);
        }
    }
}
