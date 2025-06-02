using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using DomainData;
using DomainData.Repositories;

namespace DataAccess.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposedValue;

        private readonly ClinicContext clinicContext;
        private GenericRepository<User> _userRepo;
        private GenericRepository<Appointment> _appointmentRepo;
        private GenericRepository<Doctor> _doctorRepo;
        private GenericRepository<Patient> _patientRepo;
        private GenericRepository<Service> _serviceRepo;

        public GenericRepository<User> UserRepo => _userRepo ??= new GenericRepository<User>(clinicContext);
        public GenericRepository<Appointment> AppointmentRepo => _appointmentRepo ??= new GenericRepository<Appointment>(clinicContext);
        public GenericRepository<Doctor> DoctorRepo => _doctorRepo ??= new GenericRepository<Doctor>(clinicContext);
        public GenericRepository<Patient> PatientRepo => _patientRepo ??= new GenericRepository<Patient>(clinicContext);
        public GenericRepository<Service> ServiceRepo => _serviceRepo ??= new GenericRepository<Service>(clinicContext);

        public UnitOfWork(ClinicContext context)
        {
            this.clinicContext = context;
            this.clinicContext.Database.EnsureCreated();
        }

        public void Save()
        {
            clinicContext.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    clinicContext.Dispose();   
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UnitOfWork()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
