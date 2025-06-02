using DataAccess.Models;
using DataAccess.Repositories;
using DomainData.Repositories;

namespace DataAccess.UoW
{
    public interface IUnitOfWork : IDisposable
    {
       public IGenericRepository<User> UserRepo { get; }
        public IGenericRepository<Appointment> AppointmentRepo { get; }
        public IGenericRepository<Doctor> DoctorRepo { get; }
        public IGenericRepository<Patient> PatientRepo { get; }
        public IGenericRepository<Service> ServiceRepo { get; }

        public void Save();

    }
}
