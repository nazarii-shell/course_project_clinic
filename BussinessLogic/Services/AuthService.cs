using DomainData.services;
using HospitalApp.DTOs;

namespace PresentationLayer.Utils;

public class AuthService
{
    private UserService _userService;
    
    public AuthService(UserService userService)
    {
        _userService = userService;
    }
    
    public bool CheckAuth(DtoWithAuth authDto, string role)
    {
        var potentialUser = _userService.FindUser((user => user.Email == authDto.Auth.Email));

        if (potentialUser == null || potentialUser.Password != authDto.Auth.Password || potentialUser.Role != role)
        {
            return false;
        }

        return true;
    }
}