using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAptts.UseCases.Doctor;
using MedicalAppts.Core.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using MedicalAppts.Test.IntegrationTests;
using Newtonsoft.Json;

namespace MedicalAppts.Api.IntegrationTests;

public class DoctorsControllerIntegrationTests : IntegrationTestBase
{
    public DoctorsControllerIntegrationTests(CustomWebApplicationFactory<Program> factory) : base (factory) {}

    [Fact]
    public async Task GetAppointmentsPerDoctor_ShouldReturnOk_WhenValidDoctorIdProvided()
    {
        var doctorId = 1;
        await AuthorizeAsync();
        var response = await _client.GetAsync($"/api/Doctors/{doctorId}/appointments");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }   

    [Fact]
    public async Task GetAvailableTimeFrameByDoctor_ShouldReturnOk_WhenValidDoctorIdProvided()
    {
        var doctorId = 1;
        await AuthorizeAsync();
        var response = await _client.GetAsync($"/api/Doctors/{doctorId}/availableTimeFrame");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAvailableSchedulePerDoctor_ShouldReturnNotFound_WhenInvalidDoctorIdProvided()
    {
        var doctorId = 2;
        await AuthorizeAsync();
        var response = await _client.GetAsync($"/api/Doctors/{doctorId}/availableTimeFrame");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetDoctorScheduleByDoctor_ShouldReturnOk_WhenValidDoctorIdProvided()
    {
        var doctorId = 1;
        await AuthorizeAsync();
        var response = await _client.GetAsync($"/api/Doctors/{doctorId}/schedule");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }


    [Fact]
    public async Task GetDoctorsPerSpeciality_ShouldReturnOk_WhenValidSpecialtyProvided()
    {
        var specialty = MedicalSpecialty.CARDIOLOGIST;
        await AuthorizeAsync();
        var response = await _client.GetAsync($"/api/Doctors/{specialty}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }


    [Fact]
    public async Task GetDoctorsPerSpeciality_ShouldReturnBadRequest_WhenInvalidSpecialtyIsProvided()
    {
        var specialty = "InvalidSpecialty";
        await AuthorizeAsync();
        var response = await _client.GetAsync($"/api/Doctors/{specialty}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetDoctorsPerSpeciality_ShouldReturnNotFound_WhenASpecialityWithNoDoctorsIsProvided()
    {
        var specialty = IntegrationTestHelper.GetEnumMemberValue(MedicalSpecialty.NEUROLOGIST);
        await AuthorizeAsync();
        var response = await _client.GetAsync($"/api/Doctors/{specialty}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateDoctor_ShouldReturnCreated_WhenDataIsValid()
    {
        var createDoctorForm = new CreateDoctorForm
        {
            Name = "Dr. House",
            Email = "dr.house@example.com",
            Specialty = MedicalSpecialty.CLINICAL,
            Password = "House123!"
        };
        await AuthorizeAsync();
        var response = await _client.PostAsJsonAsync("/api/Doctors", createDoctorForm);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateDoctorSchedule_ShouldReturnCreated_WhenDataIsValid()
    {
        var doctorId = 1;
        var schedule = new CreateDoctorScheduleForm
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeSpan(9, 0, 0),
            EndTime = new TimeSpan(17, 0, 0)
        };
        await AuthorizeAsync();
        var response = await _client.PostAsJsonAsync($"/api/Doctors/{doctorId}/schedule", schedule);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
