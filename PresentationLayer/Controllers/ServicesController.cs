using DomainData.services;
using HospitalApp.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api")]
public class ServiceController : ControllerBase
{
    private readonly ServiceService _serviceService;


    public ServiceController(ServiceService serviceService)
    {
        _serviceService = serviceService;
    }


    [HttpPost("service")]
    public IActionResult AddService([FromBody] ServiceDto serviceDto)
    {
        return Ok(_serviceService.AddService(serviceDto));
    }

    [HttpGet("services")]
    public IActionResult GetAllServices([FromQuery] string? query, [FromQuery] string? orderBy)
    {
        return Ok(_serviceService.GetAllServices(query, orderBy));
    } 
}