namespace HospitalApp.DTOs;

public class UpdatePatientDto : DtoWithAuth
{
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    
    public string? ContactPhone { get; set; }
    public string? Email { get; set; }
    
    public string? Password { get; set; }
    public Auth Auth { get; set; }
}