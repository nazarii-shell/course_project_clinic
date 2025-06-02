using BussinessLogic.Models;
using DataAccess.Models;
using HospitalApp.DTOs;

namespace BussinessLogic.Profiles;
using AutoMapper;


public class DoctorProfile: Profile
{
    public DoctorProfile ()
    {
        CreateMap<Doctor, DoctorBusinessModel>();
        CreateMap<DoctorBusinessModel, Doctor>();
        CreateMap<RegisterDoctorDto, Doctor>();
        CreateMap<UpdateDoctorDto, Doctor>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}