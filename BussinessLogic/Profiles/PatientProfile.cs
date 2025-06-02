using BussinessLogic.Models;
using DataAccess.Models;
using HospitalApp.DTOs;

namespace BussinessLogic.Profiles;
using AutoMapper;


public class PatientProfile: Profile
{
    public PatientProfile ()
    {
        CreateMap<PatientBusinessModel, Patient>();
        CreateMap<Patient, PatientBusinessModel>();
        CreateMap<UpdatePatientDto, Patient>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}