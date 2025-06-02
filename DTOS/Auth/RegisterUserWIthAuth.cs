namespace HospitalApp.DTOs;

public class RegisterUserWithAuth: DtoWithAuth
{
    public string Email { get; set; }

    public string Password { get; set; }

    public Auth Auth { get; set; }
}