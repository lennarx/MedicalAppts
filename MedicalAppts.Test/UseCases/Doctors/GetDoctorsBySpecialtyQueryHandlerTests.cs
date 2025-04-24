using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Enums;
using MedicalAppts.Core.Errors;
using MedicalAptts.UseCases.Doctor.GetDoctorBySpecialty;
using Moq;

namespace MedicalAppts.Test.Doctor
{
    public class GetDoctorsBySpecialtyQueryHandlerTests
    {
        private readonly Mock<IDoctorsRepository> _doctorsRepositoryMock = new();
        private readonly GetDoctorsBySpecialtyQueryHandler _handler;

        public GetDoctorsBySpecialtyQueryHandlerTests()
        {
            _handler = new GetDoctorsBySpecialtyQueryHandler(_doctorsRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsMappedDoctors_WhenDoctorsExist()
        {
            var specialty = MedicalSpecialty.CARDIOLOGIST;
            var query = new GetDoctorsBySpecialtyQuery(specialty);

            var doctors = new List<MedicalAppts.Core.Entities.Doctor>
            {
                new() { Id = 1, Name = "Dr. Heart", Specialty = MedicalSpecialty.CARDIOLOGIST, Email = "doc1@clinic.com", UserRole = UserRole.DOCTOR, UserStatus = UserStatus.ACTIVE }
            };

            _doctorsRepositoryMock
                .Setup(r => r.GetDoctorsBySpecialtyAsync(specialty))
                .ReturnsAsync(doctors);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Value is not null);
            Assert.Single(result.Value);
            Assert.Equal("Dr. Heart", result.Value.First().Name);
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenNoDoctorsExist()
        {
            var specialty = MedicalSpecialty.CARDIOLOGIST;
            var query = new GetDoctorsBySpecialtyQuery(specialty);

            _doctorsRepositoryMock
                .Setup(r => r.GetDoctorsBySpecialtyAsync(specialty))
                .ReturnsAsync(new List<MedicalAppts.Core.Entities.Doctor>());

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.Value is not null);
            Assert.Equal(GenericErrors.NoSpecialtyDoctorsFound, result.Error);
        }
    }
}
