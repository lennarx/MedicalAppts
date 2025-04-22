using MedicalAppts.Core.Entities;
using MedicalAptts.UseCases.Appointment;
using MedicalAptts.UseCases.Doctor;
using MedicalAptts.UseCases.Patient;

namespace MedicalAptts.UseCases.Helpers.Extensions
{
    public static class Mappers
    {
        public static IEnumerable<DoctorsScheduleDTO> MapToDoctorsScheduleDTOs(this IEnumerable<DoctorSchedule> schedules)
        {
            return schedules.Select(x => new DoctorsScheduleDTO
            {
                DoctorId = x.DoctorId,
                DayOfWeek = x.DayOfWeek,
                DoctorName = x.Doctor.Name,
                StartTime = x.StartTime.Hours * 100 + x.StartTime.Minutes,
                EndTime = x.EndTime.Hours * 100 + x.EndTime.Minutes
            });
        }

        public static DoctorsScheduleDTO MapToDoctorScheduleDTO(this DoctorSchedule schedule)
        {
            return new DoctorsScheduleDTO
            {
                DoctorId = schedule.DoctorId,
                DayOfWeek = schedule.DayOfWeek,
                DoctorName = schedule.Doctor.Name,
                StartTime = schedule.StartTime.Hours * 100 + schedule.StartTime.Minutes,
                EndTime = schedule.EndTime.Hours * 100 + schedule.EndTime.Minutes
            };
        }

        public static IEnumerable<DoctorDTO> MapToDoctorDTOs(this IEnumerable<MedicalAppts.Core.Entities.Doctor> doctors)
        {
            return doctors.Select(x => new DoctorDTO
            {
                DoctorId = x.Id,
                Name = x.Name,
                Specialty = x.Specialty
            });
        }

        public static DoctorDTO MapToDoctorDTO(this MedicalAppts.Core.Entities.Doctor doctor)
        {
            return new DoctorDTO
            {
                DoctorId = doctor.Id,
                Name = doctor.Name,
                Specialty = doctor.Specialty
            };
        }

        public static AppointmentDTO MapToAppointmentDTO(this MedicalAppts.Core.Entities.Appointment appointment)
        {
            return new AppointmentDTO
            {
                AppointmentId = appointment.Id,
                Patient = appointment.Patient.Name,
                Doctor = appointment.Doctor.Name,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status
            };
        }

        public static IEnumerable<AppointmentDTO> MapToAppointmentDTOs(this IEnumerable<MedicalAppts.Core.Entities.Appointment> appointments)
        {
            return appointments.Select(x => new AppointmentDTO
            {
                AppointmentId = x.Id,
                Patient = x.Patient.Name,
                Doctor = x.Doctor.Name,
                AppointmentDate = x.AppointmentDate,
                Status = x.Status
            });
        }

        public static IEnumerable<PatientDTO> MapToPatientDTOs(this IEnumerable<MedicalAppts.Core.Entities.Patient> patients)
        {
            return patients.Select(x => new PatientDTO
            {
                PatientId = x.Id,
                Name = x.Name,
                DateOfbirth = x.DateOfBirth.ToString("dd-mm-YYYY"),
                Email = x.Email,
                PhoneNumber = x.PhoneNumber
            });
        }

        public static PatientDTO MapToPatientDTO(this MedicalAppts.Core.Entities.Patient patient)
        {
            return new PatientDTO
            {
                PatientId = patient.Id,
                Name = patient.Name,
                DateOfbirth = patient.DateOfBirth.ToString("dd-mm-YYYY"),
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber
            };
        }
    }
}
