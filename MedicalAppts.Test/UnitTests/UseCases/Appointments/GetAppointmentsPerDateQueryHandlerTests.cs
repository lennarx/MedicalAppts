using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;
using MedicalAppts.Core.Enums;
using MedicalAptts.UseCases.Appointment.GetAppointmentsPerDate;
using Moq;

namespace MedicalAppts.Test.UnitTests.UseCases.Appointments
{
    public class GetAppointmentsPerDateQueryHandlerTests
    {
        private readonly Mock<IAppointmentsRepository> _appointmentRepositoryMock;
        private readonly GetAppointmentsPerDateQueryHandler _handler;

        public GetAppointmentsPerDateQueryHandlerTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentsRepository>();
            _handler = new GetAppointmentsPerDateQueryHandler(_appointmentRepositoryMock.Object);
        }


        [Fact]
        public async Task Handle_ReturnsMappedAppointments_WhenAppointmentsExist()
        {
            var date = new DateTime(2025, 4, 24);
            var query = new GetAppointmentsPerDateQuery(date);

            var appointments = new List<Core.Entities.Appointment>
            {
                new()
                {
                    Id = 1,
                    AppointmentDate = date,
                    Doctor = new Core.Entities.Doctor { Name = "Dr. Smith" },
                    Patient = new Core.Entities.Patient { Name = "John Doe" },
                    Status = AppointmentStatus.SCHEDULED
                },
                new()
                {
                    Id = 2,
                    AppointmentDate = date,
                    Doctor = new Core.Entities.Doctor { Name = "Dr. Jones" },
                    Patient = new Core.Entities.Patient { Name = "Jane Doe" },
                    Status = AppointmentStatus.SCHEDULED
                }
            };

            _appointmentRepositoryMock
                .Setup(r => r.GetAppointmentsByDateAsync(date))
                .ReturnsAsync(appointments);

            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Value is not null);
            var dtos = result.Value.ToList();
            Assert.Equal(2, dtos.Count);
            Assert.Equal(1, dtos[0].AppointmentId);
            Assert.Equal(2, dtos[1].AppointmentId);
            Assert.Equal("John Doe", dtos[0].Patient);
            Assert.Equal("Jane Doe", dtos[1].Patient);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoAppointmentsExist()
        {
            var date = new DateTime(2025, 4, 24);
            var query = new GetAppointmentsPerDateQuery(date);

            _appointmentRepositoryMock
                .Setup(r => r.GetAppointmentsByDateAsync(date))
                .ReturnsAsync(new List<Core.Entities.Appointment>());

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Value is not null);
            Assert.Empty(result.Value);
        }
    }
}

