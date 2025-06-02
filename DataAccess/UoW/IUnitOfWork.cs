using DataAccess.Models;
using DomainData.Repositories;

namespace DataAccess.UoW
{
    public interface IUnitOfWork : IDisposable
    {
       public GenericRepository<User> UserRepo { get; }
        public GenericRepository<Appointment> AppointmentRepo { get; }
        public GenericRepository<Doctor> DoctorRepo { get; }
        public GenericRepository<Patient> PatientRepo { get; }
        public GenericRepository<Service> ServiceRepo { get; }

        public void Save();

    }
}
