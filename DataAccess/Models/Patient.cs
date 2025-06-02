using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models;


[Table("Patients")]
public class Patient
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string ContactPhone { get; set; }

    public int? UserId { get; set; } // якщо зареєстрований користувач
    public User User { get; set; }

    public List<Appointment> Appointments { get; set; }
}