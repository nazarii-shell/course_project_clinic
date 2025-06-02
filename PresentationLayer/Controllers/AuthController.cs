using System.Security.Authentication;
using DataAccess.Models;
using DomainData.services;
using HospitalApp.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Utils;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly PatientService _patientService;
    private readonly DoctorService _doctorService;
    private readonly AuthService _authService;

    public AuthController(UserService userService, PatientService patientService, DoctorService doctorService, AuthService authService)
    {
        _userService = userService;
        _patientService = patientService;
        _doctorService = doctorService;
        _authService = authService;
    }

    [HttpPost("register-patient")]
    public IActionResult RegisterPatient([FromBody] RegisterPatient request)
    {
        if (_userService.Exists(request.Email))
            return BadRequest("Email already exists");

        var newUser = new User
        {
            Password = request.Password,
            Email = request.Email,
            Role = "Registered"
        };

        var newPatient = new Patient
        {
            DateOfBirth = request.DateOfBirth,
            ContactPhone = request.ContactPhone,
            FullName = request.FullName,
            User = newUser,
        };

        var addedPatient = _patientService.AddPatient(newPatient);
        return Ok(addedPatient);
    } 
   
    [HttpPost("register-doctor")]
    public IActionResult RegisterDoctor([FromBody] RegisterDoctorDto registerDoctorDto)
    {
        if (_doctorService.Exists(registerDoctorDto.Email))
            return BadRequest("Username already exists");

        var newDoctor = new Doctor
        {
            Email = registerDoctorDto.Email,
            FullName = registerDoctorDto.FullName,
            Specialty = registerDoctorDto.Specialty,
            PhoneNumber = registerDoctorDto.PhoneNumber,
            Password = registerDoctorDto.Password,
        };

        var addedDoctor = _doctorService.AddDoctor(newDoctor);
        return Ok(addedDoctor);
    }
 
    [HttpPost("register-admin")]
    public IActionResult RegisterAdmin([FromBody] RegisterUserDto registerUserDto)
    {
        if (_userService.Exists(registerUserDto.Email))
            return BadRequest("Email already exists");

        var newUser = new User
        {
            Email = registerUserDto.Email,
            Password = registerUserDto.Password,
            Role = "Admin"
        };

        var addedUser = _userService.AddUser(newUser);
        return Ok(addedUser);
    }   
    
    [HttpPost("register-manager")]
    public IActionResult RegisterManager([FromBody] RegisterUserWithAuth registerUserDto)
    {
        if (!_authService.CheckAuth(registerUserDto, "Admin"))
            return BadRequest("You are not authorized");
        
        
        if (_userService.Exists(registerUserDto.Email))
            return BadRequest("Email already exists");

        var newUser = new User
        {
            Email = registerUserDto.Email,
            Password = registerUserDto.Password,
            Role = "Manager"
        };

        var addedUser = _userService.AddUser(newUser);
        return Ok(addedUser);
    }
}