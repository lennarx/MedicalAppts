using MedicalAppts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppts.Infrastructure
{
    public class MedicalApptsDbContext : DbContext
    {
        public MedicalApptsDbContext(DbContextOptions<MedicalApptsDbContext> options)
            : base(options)
        {
        }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorSchedule> doctorSchedules { get; set; }        
    }
}
