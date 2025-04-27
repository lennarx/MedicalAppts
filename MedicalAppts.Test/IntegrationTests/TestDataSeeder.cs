using MedicalAppts.Core.Entities;
using MedicalAppts.Core.Enums;
using MedicalAppts.Infrastructure;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppts.Test.IntegrationTests
{
    public static class TestDataSeeder
    {
        public static async Task SeedAsync(MedicalApptsDbContext context)
        {
            if (context.Doctors.Any() || context.Patients.Any() || context.Appointments.Any())
                return;

            var hasher = new PasswordHasher();
            var doctor = new Core.Entities.Doctor
            {
                Name = "Dr. House",
                Email = "house@clinic.com",
                PasswordHash = hasher.HashPassword("hashed"), // en tests no importa
                UserRole = UserRole.DOCTOR,
                UserStatus = UserStatus.ACTIVE,
                Specialty = MedicalSpecialty.CARDIOLOGIST,
                DoctorSchedules = new List<DoctorSchedule>()
            };
            context.Doctors.Add(doctor);

            var schedule = new DoctorSchedule
            {
                DayOfWeek = DayOfWeek.Monday,
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(13, 0, 0),
                Doctor = doctor
            };
            context.doctorSchedules.Add(schedule);

            var patient = new Core.Entities.Patient
            {
                Name = "John Doe",
                Email = "john@doe.com",
                PasswordHash = hasher.HashPassword("hashed"),
                UserRole = UserRole.PATIENT,
                UserStatus = UserStatus.ACTIVE,
                PhoneNumber = "123456789",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            context.Patients.Add(patient);

            var admin = new User
            {
                Name = "Franco Admin",
                PasswordHash = hasher.HashPassword("Admin123!"),
                UserRole = UserRole.ADMIN,
                Email = "admin@medicalapp.com"
            };
            context.Users.Add(admin);
            await context.SaveChangesAsync();

            var appointment = new Core.Entities.Appointment
            {
                AppointmentDate = IntegrationTestHelper.NextMondayAt(),
                Doctor = doctor,
                Patient = patient,
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Status = AppointmentStatus.SCHEDULED,
                ReasonForVisit = "Routine check-up",
                Notes = "Test appointment"
            };

            context.Appointments.Add(appointment);            
            await context.SaveChangesAsync();
        }
    }
}
