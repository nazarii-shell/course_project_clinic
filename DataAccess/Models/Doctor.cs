using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    [Table("Doctors")]
    public class Doctor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Specialty { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Appointment> Appointments { get; set; }
        public List<Service> Services { get; set; } 
    }
}
