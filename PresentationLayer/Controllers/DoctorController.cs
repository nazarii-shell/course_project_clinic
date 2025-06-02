using DataAccess.Models;
using DomainData;
using DomainData.services;
using HospitalApp.DTOs;
using HospitalApp.DTOs.Doctor;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Utils;

namespace HospitalApp.Controllers;

[ApiController]
[Route("api")]
public class DoctorController : ControllerBase
{
    private readonly UserService _userService;
    private readonly DoctorService _doctorService;
    private readonly AuthService _authService;


    public DoctorController(DoctorService doctorService, AuthService authService)
    {
        _doctorService = doctorService;
        _authService = authService;
    }

    [HttpGet("doctors")]
    public IActionResult GetAllDoctors([FromQuery] string? query, [FromQuery] string? orderBy, [FromQuery] DoctorFilter? filter)
    {
        return Ok(_doctorService.GetAllDoctors(query, orderBy, filter));
    }
    
    [HttpPatch("doctor/{id}")]
    public IActionResult UpdatePatient(int id, UpdateDoctorDto patient)
    {
        if (!_authService.CheckAuth(patient, "Manager"))
            return BadRequest("You are not authorized");

        
        var updated = _doctorService.UpdateDoctor(id, patient);
        if (updated == null) return BadRequest("Patient not found");
        return Ok(updated);
    }
}