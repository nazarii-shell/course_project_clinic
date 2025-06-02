namespace HospitalApp.DTOs;

public class UpdateDoctorDto : DtoWithAuth
{
    public string? FullName { get; set; }
    public string? Specialty { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    
    public string? PhoneNumber { get; set; }

    public Auth Auth { get; set; }
}