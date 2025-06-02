using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessLogic.Models
{
    public class DoctorBusinessModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Specialty { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        public List<AppointmentBusinessModel> Appointments { get; set; }
        public List<ServiceBusinessModel> Services { get; set; } = new();
    }
}
