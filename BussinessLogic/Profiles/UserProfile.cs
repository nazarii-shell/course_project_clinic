using BussinessLogic.Models;
using DataAccess.Models;

namespace BussinessLogic.Profiles;
using AutoMapper;


public class UserProfile: Profile
{
    public UserProfile ()
    {
        CreateMap<User, UserBusinessModel>();
        CreateMap<UserBusinessModel, User>();
    }
}