using BussinessLogic.Models;
using DataAccess.Models;

namespace BussinessLogic.Profiles;
using AutoMapper;


public class AppointmentProfile: Profile
{
    public AppointmentProfile ()
    {
        CreateMap<Appointment, AppointmentBusinessModel>();
        CreateMap<AppointmentBusinessModel, Appointment>();
    }
}