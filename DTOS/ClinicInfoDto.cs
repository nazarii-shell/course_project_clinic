namespace DTOS;

public class ClinicInfoDto
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string WorkingHours { get; set; }
    public List<string> Services { get; set; }
}