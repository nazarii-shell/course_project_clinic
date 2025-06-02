namespace HospitalApp.DTOs;

public class RegisterPatient
{
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    
    public string ContactPhone { get; set; }
    public string Email { get; set; }
    
    public required string Password { get; set; }
}