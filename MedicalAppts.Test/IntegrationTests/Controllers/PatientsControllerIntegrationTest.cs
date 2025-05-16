using FluentAssertions;
using MedicalAptts.UseCases.Appointment;
using MedicalAptts.UseCases.Patient;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace MedicalAppts.Test.IntegrationTests.Controllers
{
    [Collection("Sequential")]
    public class PatientsControllerIntegrationTest : IntegrationTestBase
    {
        public PatientsControllerIntegrationTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetAppointmentsPerPatient_ShouldReturnOk_WhenPatientIdAndApptDateAreValid()
        {
            var patientId = 2;
            var appointmentDate = IntegrationTestHelper.NextMondayAt();
            await AuthorizeAsync();
            var appointmentDateString = appointmentDate.ToString("o");

            var response = await _client.GetAsync($"/api/patients/{patientId}/appointments?appointmentDate={appointmentDateString}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadAsStringAsync();
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetAppointmentsPerPatient_ShouldReturnOk_WhenPatientIdIsValidAndNoApptDateProvided()
        {
            var patientId = 2;
            await AuthorizeAsync();

            var response = await _client.GetAsync($"/api/patients/{patientId}/appointments");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadAsStringAsync();
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CreatePatient_ShouldReturnOk_WhenPatientFormIsValid()
        {
            var patientCreationForm = new PatientCreationForm
            {
                Email = "someEmail@some.com",
                Name = "Some Name",
                Password = "AStrongPassword123!",
                PhoneNumber = "1234567890",
                DateOfBirth = DateTime.Now.AddYears(-20),
            };

            await AuthorizeAsync();

            var response = await _client.PostAsJsonAsync("/api/patients", patientCreationForm);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var result = await response.Content.ReadAsStringAsync();
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CreatePatient_ShouldReturnBadRequest_WhenInvalidEmailFormatAndInvalidPasswordAreProvided()
        {
            var patientCreationForm = new PatientCreationForm
            {
                Email = "someEmailWithWrongFormat",
                Name = "Some Name",
                Password = "AStrongPassword123!",
                PhoneNumber = "1234567890",
                DateOfBirth = DateTime.Now.AddYears(-20),
            };

            await AuthorizeAsync();

            var response = await _client.PostAsJsonAsync("/api/patients", patientCreationForm);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            patientCreationForm.Email = "someEmail@some.com";
            patientCreationForm.Password = string.Empty;

            response = await _client.PostAsJsonAsync("/api/patients", patientCreationForm);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateAppointment_ShouldReturnNoContent_WhenUpdateDataIsValid()
        {

            var updateApptForm = new UpdateAppointmentForm
            {
                Action = AppointmentActionsEnum.RESCHEDULE,
                NewDate = IntegrationTestHelper.NextMondayAt(9, 0)
            };
            await AuthorizeAsync();

            var patientIdInTestDb = 2;
            var appointmentIdInTestDb = 1;

            var response = await _client.PatchAsJsonAsync($"/api/patients/{patientIdInTestDb}/appointments/{appointmentIdInTestDb}", updateApptForm);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateAppointment_ShouldReturnNotFound_WhenAppointmentToUpdateDoesntExist()
        {

            var updateApptForm = new UpdateAppointmentForm
            {
                Action = AppointmentActionsEnum.RESCHEDULE,
                NewDate = IntegrationTestHelper.NextMondayAt(9, 0)
            };
            await AuthorizeAsync();

            var patientIdInTestDb = 2;
            var appointmentIdInTestDb = 5;

            var response = await _client.PatchAsJsonAsync($"/api/patients/{patientIdInTestDb}/appointments/{appointmentIdInTestDb}", updateApptForm);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateAppointment_ShouldReturnBadRequest_WhenPatientIsntAppointmentOwner()
        {

            var updateApptForm = new UpdateAppointmentForm
            {
                Action = AppointmentActionsEnum.RESCHEDULE,
                NewDate = IntegrationTestHelper.NextMondayAt(9, 0)
            };
            await AuthorizeAsync();

            var notApptOwnerPatientId = 3;
            var appointmentIdInTestDb = 1;

            var response = await _client.PatchAsJsonAsync($"/api/patients/{notApptOwnerPatientId}/appointments/{appointmentIdInTestDb}", updateApptForm);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateAppointment_ShouldReturnNoContent_WhenDeleteDataIsValid()
        {

            var updateApptForm = new UpdateAppointmentForm
            {
                Action = AppointmentActionsEnum.CANCEL,
                NewDate = IntegrationTestHelper.NextMondayAt()
            };
            await AuthorizeAsync();

            var patientIdInTestDb = 2;
            var appointmentIdInTestDb = 1;

            var response = await _client.PatchAsJsonAsync($"/api/patients/{patientIdInTestDb}/appointments/{appointmentIdInTestDb}", updateApptForm);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateAppointment_ShouldReturnNotFound_WhenAppointmentToDeleteDoesntExist()
        {

            var updateApptForm = new UpdateAppointmentForm
            {
                Action = AppointmentActionsEnum.CANCEL,
                NewDate = IntegrationTestHelper.NextMondayAt(9, 0)
            };
            await AuthorizeAsync();

            var patientIdInTestDb = 2;
            var appointmentIdInTestDb = 5;

            var response = await _client.PatchAsJsonAsync($"/api/patients/{patientIdInTestDb}/appointments/{appointmentIdInTestDb}", updateApptForm);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateAppointment_ShouldReturnBadRequest_WhenPatientIsntAppointmentToDeleteOwner()
        {

            var updateApptForm = new UpdateAppointmentForm
            {
                Action = AppointmentActionsEnum.CANCEL,
                NewDate = IntegrationTestHelper.NextMondayAt(9, 0)
            };
            await AuthorizeAsync();

            var notApptOwnerPatientId = 3;
            var appointmentIdInTestDb = 1;

            var response = await _client.PatchAsJsonAsync($"/api/patients/{notApptOwnerPatientId}/appointments/{appointmentIdInTestDb}", updateApptForm);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
