namespace HospitalApp.DTOs;

public class ServiceDto 
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<RegisterDoctorDto> Doctors { get; set; }
}