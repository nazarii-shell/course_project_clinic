using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DomainData
{
    public class ClinicContext : DbContext
    {
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Service> Services { get; set; }

        public string DataBasePath { get; }

        public ClinicContext()
        {
            DataBasePath = "./clinic.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DataBasePath}");
        }
    }
}
