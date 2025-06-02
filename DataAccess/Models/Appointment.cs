using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models;


[Table("Appointments")]
public class Appointment
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public int PatientId { get; set; }
    public Patient Patient { get; set; }

    public DateTime AppointmentDate { get; set; }
    public string Reason { get; set; }
}