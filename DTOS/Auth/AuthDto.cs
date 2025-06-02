namespace HospitalApp.DTOs;


public interface DtoWithAuth
{
    public Auth Auth { get; set; }
}
public class Auth
{
    public string Email { get; set; }
    public string Password { get; set; }
}

