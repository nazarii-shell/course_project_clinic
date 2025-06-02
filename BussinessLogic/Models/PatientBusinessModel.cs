

namespace BussinessLogic.Models;

public class PatientBusinessModel
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string ContactPhone { get; set; }

    public List<AppointmentBusinessModel> Appointments { get; set; }
}