using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;
using MedicalAppts.Core.Enums;
using MedicalAptts.UseCases.Appointment.GetAppointmentsPerPatient;
using Moq;

namespace MedicalAppts.Test.Appointment
{
    public class GetAppointmentsPerPatientQueryHandlerTests
    {
        private readonly Mock<IAppointmentsRepository> _appointmentsRepositoryPatientMock;
        private readonly GetAppointmentPerPatientQueryHandler _handler;

        public GetAppointmentsPerPatientQueryHandlerTests()
        {
            _appointmentsRepositoryPatientMock = new Mock<IAppointmentsRepository>();
            _handler = new GetAppointmentPerPatientQueryHandler(_appointmentsRepositoryPatientMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsAppointmentsForPatientAndDate()
        {
            var date = new DateTime(2025, 4, 24);
            var patientId = 123;
            var query = new GetAppointmentsPerPatient(patientId, date);

            var appointments = new List<Core.Entities.Appointment>
            {
                new()
                {
                    Id = 1,
                    AppointmentDate = date,
                    Doctor = new Core.Entities.Doctor { Name = "Dr. Smith" },
                    Patient = new Core.Entities.Patient { Name = "John Doe" },
                    Status = AppointmentStatus.SCHEDULED
                }
            };

            _appointmentsRepositoryPatientMock
                .Setup(r => r.GetAppointmentsByDateAndPatientIdAsync(date, patientId))
                .ReturnsAsync(appointments);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Value is not null);
            var dtos = result.Value.ToList();
            Assert.Single(dtos);
            Assert.Equal(1, dtos[0].AppointmentId);
            Assert.Equal("John Doe", dtos[0].Patient);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoAppointmentsFound()
        {            
            var date = new DateTime(2025, 4, 24);
            var patientId = 123;
            var query = new GetAppointmentsPerPatient(patientId, date);

            _appointmentsRepositoryPatientMock
                .Setup(r => r.GetAppointmentsByDateAndPatientIdAsync(date, patientId))
                .ReturnsAsync(new List<Core.Entities.Appointment>());

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Value is not null);
            Assert.Empty(result.Value);
        }
    }
}