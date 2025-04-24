using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;
using MedicalAppts.Core.Enums;
using MedicalAptts.UseCases.Appointment.GetAppointmentsPerDoctor;
using Moq;

namespace MedicalAppts.Test.Appointment
{
    public class GetAppointmentsPerDoctorQueryHandlerTests
    {
        private readonly Mock<IAppointmentsRepository> _appointmentsRepositoryDoctorMock;
        private readonly GetAppointmentsPerDoctorQueryHandler _handler;

        public GetAppointmentsPerDoctorQueryHandlerTests()
        {
            _appointmentsRepositoryDoctorMock = new Mock<IAppointmentsRepository>();
            _handler = new GetAppointmentsPerDoctorQueryHandler(_appointmentsRepositoryDoctorMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsAppointmentsForDoctorAndDate()
        {
            var date = new DateTime(2025, 4, 24);
            var doctorId = 456;
            var query = new GetAppointmentsPerDoctorQuery(doctorId, date);

            var appointments = new List<Core.Entities.Appointment>
            {
                new()
                {
                    Id = 1,
                    AppointmentDate = date,
                    Doctor = new Core.Entities.Doctor { Name = "Dr. House" },
                    Patient = new Core.Entities.Patient { Name = "Jane Doe" },
                    Status = AppointmentStatus.SCHEDULED
                }
            };

            _appointmentsRepositoryDoctorMock
                .Setup(r => r.GetAppointmentsByDateAndDoctorIdAsync(date, doctorId))
                .ReturnsAsync(appointments);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Value is not null);
            var dtos = result.Value.ToList();
            Assert.Single(dtos);
            Assert.Equal(1, dtos[0].AppointmentId);
            Assert.Equal("Jane Doe", dtos[0].Patient);
            Assert.Equal("Dr. House", dtos[0].Doctor);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoAppointmentsFound()
        {
            var date = new DateTime(2025, 4, 24);
            var doctorId = 456;
            var query = new GetAppointmentsPerDoctorQuery(doctorId, date);

            _appointmentsRepositoryDoctorMock
                .Setup(r => r.GetAppointmentsByDateAndDoctorIdAsync(date, doctorId))
                .ReturnsAsync(new List<Core.Entities.Appointment>());

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Value is not null);
            Assert.Empty(result.Value);
        }
    }
}
