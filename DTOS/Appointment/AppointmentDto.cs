namespace HospitalApp.DTOs;

public class AppointmentDto: DtoWithAuth
{
    public DateTime Date { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    
    public string Reason { get; set; }
    public Auth Auth { get; set; }
}