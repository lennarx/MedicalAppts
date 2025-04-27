using FluentAssertions;
using MedicalAptts.UseCases.Appointment;
using System.Net;
using System.Net.Http.Json;

namespace MedicalAppts.Test.IntegrationTests.Controllers
{
    public class AppointmentControllerIntegrationTest : IntegrationTestBase
    {
        public AppointmentControllerIntegrationTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetApptsByDate_ShouldReturnOk_WhenValidDataIsProvided()
        {
            await AuthorizeAsync();
            var date = IntegrationTestHelper.NextMondayAt().ToString("yyyy-MM-dd");

            var response = await _client.GetAsync($"/api/appointments/{date}");

            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<MedicalAptts.UseCases.Appointment.AppointmentDTO>>();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task CreateAppointment_ShouldReturnOk_WhenValidDataIsProvided()
        {
            await AuthorizeAsync();
            var appt = new AppointmentCreationForm
            {
                AppointmentDate = IntegrationTestHelper.NextMondayAt(11, 0),
                DoctorId = 1,
                PatientId = 2
            };

            var response = await _client.PostAsJsonAsync("/api/appointments", appt);

            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreateAppointment_ShouldReturnBadRequest_WhenApptAlreadyExists()
        {
            await AuthorizeAsync();
            var appt = new AppointmentCreationForm
            {
                AppointmentDate = IntegrationTestHelper.NextMondayAt(),
                DoctorId = 1,
                PatientId = 2
            };

            var response = await _client.PostAsJsonAsync("/api/appointments", appt);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateAppointment_ShouldReturnBadRequest_WhenApptAIsOutsideDoctorTimeFrame()
        {
            await AuthorizeAsync();
            var appt = new AppointmentCreationForm
            {
                AppointmentDate = IntegrationTestHelper.NextMondayAt().AddDays(1),
                DoctorId = 1,
                PatientId = 2
            };

            var response = await _client.PostAsJsonAsync("/api/appointments", appt);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
