using MedicalAppts.Core.Entities;
using MedicalAppts.Core.Enums;
using MedicalAppts.Core.Errors;
using Moq;
using MediatR;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAptts.UseCases.Appointment.SetAppointment;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using MedicalAppts.Core.Events;

namespace MedicalAppts.Test.Appointment
{
    public class SetAppointmentCommandHandlerTests
    {
        private readonly Mock<IAppointmentsRepository> _appointmentsRepositoryMock = new();
        private readonly Mock<IPatientsRepository> _patientsRepositoryMock = new();
        private readonly Mock<IDoctorsRepository> _doctorsRepositoryMock = new();
        private readonly Mock<IDoctorsScheduleRepository> _scheduleRepositoryMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly Mock<ILogger<SetAppointmentCommandHandler>> _loggerMock = new();

        private readonly SetAppointmentCommandHandler _handler;

        public SetAppointmentCommandHandlerTests()
        {
            _handler = new SetAppointmentCommandHandler(
                _appointmentsRepositoryMock.Object,
                _patientsRepositoryMock.Object,
                _doctorsRepositoryMock.Object,
                _scheduleRepositoryMock.Object,
                _loggerMock.Object,
                _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenAppointmentAlreadyExists()
        {
            var request = new SetAppointmentCommand(1, 1, DateTime.Today);
            _appointmentsRepositoryMock.Setup(r => r.GetAppointmentsByDateAndDoctorIdAsync(request.AppointmentDate, request.DoctorId))
                .ReturnsAsync(new List<Core.Entities.Appointment> { new Core.Entities.Appointment() });

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.False(result.Value is not null);
            Assert.Equal(GenericErrors.AppointmentAlreadyExists, result.Error);
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenAppointmentOutsideDoctorSchedule()
        {
            var request = new SetAppointmentCommand(1, 1, DateTime.Today.AddHours(10));
            _appointmentsRepositoryMock.Setup(r => r.GetAppointmentsByDateAndDoctorIdAsync(request.AppointmentDate, request.DoctorId))
                .ReturnsAsync(new List<Core.Entities.Appointment>());

            _scheduleRepositoryMock.Setup(r => r.GetFiltered(It.IsAny<Func<DoctorSchedule, bool>>(), false))
                .Returns(new List<DoctorSchedule>());

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.False(result.Value is not null);
            Assert.Equal(GenericErrors.AppointmentOutOfTimeFrame, result.Error);
        }

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenAppointmentIsScheduled()
        {
            var request = new SetAppointmentCommand(1, 1, new DateTime(2025, 4, 25, 10, 0, 0));

            var doctor = new Core.Entities.Doctor { Id = 1, Name = "Dr. Smith" };
            var patient = new Core.Entities.Patient { Id = 1, Name = "Jane", Email = "jane@example.com" };

            _appointmentsRepositoryMock.Setup(r => r.GetAppointmentsByDateAndDoctorIdAsync(request.AppointmentDate, request.DoctorId))
                .ReturnsAsync(new List<Core.Entities.Appointment>());

            _scheduleRepositoryMock.Setup(r => r.GetFiltered(It.IsAny<Func<DoctorSchedule, bool>>(), false))
                .Returns(new List<DoctorSchedule> {
                    new DoctorSchedule {
                        DoctorId = 1,
                        DayOfWeek = request.AppointmentDate.DayOfWeek,
                        StartTime = new TimeSpan(9, 0, 0),
                        EndTime = new TimeSpan(17, 0, 0)
                    }
                });

            _doctorsRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(doctor);
            _patientsRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(patient);

            _appointmentsRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Core.Entities.Appointment>())).Callback<Core.Entities.Appointment>(appt =>
                {
                    appt.Doctor = doctor;
                    appt.Patient = patient;
                }).Returns(Task.CompletedTask);

            _mediatorMock.Setup(m => m.Publish(It.IsAny<AppointmentCreatedEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.True(result.Value is not null);
            Assert.Equal(AppointmentStatus.SCHEDULED, result.Value.Status);
            Assert.Equal("Dr. Smith", result.Value.Doctor);
            Assert.Equal("Jane", result.Value.Patient);
        }
    }
}
