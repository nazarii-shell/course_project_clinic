using BussinessLogic.Models;
using DataAccess.Models;
using HospitalApp.DTOs;

namespace BussinessLogic.Profiles;
using AutoMapper;


public class ServiceProfile: Profile
{
    public ServiceProfile ()
    {
        CreateMap<ServiceBusinessModel, Service>();
        CreateMap<Service, ServiceBusinessModel>();
        CreateMap<ServiceDto, Service>();
    }
}