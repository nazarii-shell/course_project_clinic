using DomainData.services;
using DTOS;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api")]
public class InfoController : ControllerBase
{
    private ServiceService _serviceService;
    
    public InfoController(ServiceService serviceService)
    {
        _serviceService = serviceService;
    }
    
    [HttpGet("info")]
    public IActionResult GetClinicInfo()
    {
        var info = new ClinicInfoDto
        {
            Name = "Клініка 'МедЛайф'",
            Address = "вул. Здоров'я, 10",
            Phone = "+380123456789",
            Email = "info@medlife.com",
            WorkingHours = "Пн-Пт: 08:00–18:00",
            Services = _serviceService.GetAll().Select(s => s.Name).ToList()
        };

        return Ok(info);
    }
}