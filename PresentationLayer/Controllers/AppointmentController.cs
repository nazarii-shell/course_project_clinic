using DataAccess.Models;
using DomainData.services;
using HospitalApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Utils;

namespace HospitalApp.Controllers;

[ApiController]
[Route("api")]
public class Appoinment : ControllerBase
{
    private readonly DoctorService _doctorService;
    private readonly PatientService _patientService;
    private readonly AppoinmentService _appointmentService;
    private readonly AuthService _authService;


    public Appoinment(AppoinmentService patienService, PatientService patientService, DoctorService doctorService, AuthService authService)
    {
        _appointmentService = patienService;
        _patientService = patientService;
        _doctorService = doctorService;
        _authService = authService;
    }

    [HttpPost("appointment")]
    public IActionResult Create([FromBody] AppointmentDto appointmentDto)
    {
        if (!_authService.CheckAuth(appointmentDto, "Registered"))
            return BadRequest("You are not authorized");
        
        var doctor = _doctorService.GetById(appointmentDto.DoctorId);
        
        if (doctor == null)
            return BadRequest("Doctor not found");
        
        var patient = _patientService.GetById(appointmentDto.PatientId);
        if (patient == null)
            return BadRequest("Patient not found");

        var newAppointment = new Appointment
        {
            Doctor = doctor,
            Patient = patient,
            AppointmentDate = appointmentDto.Date,
            Reason = appointmentDto.Reason,
        };
        
        var createdAppoinment  = _appointmentService.AddApoinment(newAppointment);
        
        return Ok(createdAppoinment);
    }  
    
    [HttpGet("appointments")]
    public IActionResult GetAppointments([FromQuery] string? orderBy)
    {
        return Ok(_appointmentService.GetAllAppoinments(orderBy));
    }
}