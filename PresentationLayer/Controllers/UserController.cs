using DataAccess.Models;
using DomainData;
using DomainData.services;
using HospitalApp.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApp.Controllers;

[ApiController]
[Route("api")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("users")]
    public IActionResult GetAllUsers()
    {
        return Ok(_userService.GetAllUsers());
    }
}