using DataAccess.Models;
using DomainData.services;
using HospitalApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Utils;

namespace HospitalApp.Controllers;

[ApiController]
[Route("api")]
public class PatientController : ControllerBase
{
    private readonly UserService _userService;
    private readonly PatientService _patientService;
    private readonly AuthService _authService;

    public PatientController(PatientService patienService, AuthService authService)
    {
        _patientService = patienService;
        _authService = authService;
    }

    [HttpGet("patients")]
    public IActionResult GetAllPatients()
    {
        
        return Ok(_patientService.GetAllPatients());
    }

    [HttpPatch("patient/{id}")]
    public IActionResult UpdatePatient(int id, UpdatePatientDto patient)
    {
        if (!_authService.CheckAuth(patient, "Manager"))
            return BadRequest("You are not authorized");

        
        var updated = _patientService.UpdatePatient(id, patient);
        if (updated == null) return BadRequest("Patient not found");
        return Ok(updated);
    }
}