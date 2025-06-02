using System.Linq.Expressions;
using AutoMapper;
using BussinessLogic.Models;
using DataAccess.Models;
using DataAccess.UoW;
using HospitalApp.DTOs;
using HospitalApp.DTOs.Doctor;

namespace DomainData.services;

public class DoctorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public DoctorBusinessModel AddDoctor(Doctor patient)
    {
        var createdDoctor = _unitOfWork.DoctorRepo.Create(patient);
        _unitOfWork.Save();

        return _mapper.Map<DoctorBusinessModel>(createdDoctor);
    }

    public Doctor GetById(int id)
    {
        return _unitOfWork.DoctorRepo.GetById(id);
    }

    public bool Exists(string email)
    {
        return _unitOfWork.DoctorRepo.Exists((doctor => doctor.Email == email));
    }


    public DoctorBusinessModel[] GetAllDoctors(string? query, string? orderBy, DoctorFilter? filter)
    {
        Expression<Func<Doctor, bool>>? searchPredicate =
            query != null ? (d) => d.FullName.Contains(query) || d.Specialty.Contains(query) : null;
        
        Expression<Func<Doctor, object>>? orderByPredicate = (doctor => orderBy == "Name" ? doctor.FullName : doctor.Specialty);
        
        Expression<Func<Doctor, bool>>? filterPredicate = d =>
                (filter.Specialty == null || d.Specialty.Contains(filter.Specialty)) &&
                (filter.ServiceName == null || d.Services.Any(s => s.Name.Contains(filter.ServiceName)));
        
        
        var response = _unitOfWork.DoctorRepo.GetAll(
            searchPredicate,
            orderByPredicate,
            filterPredicate,
            "Services"
        );

        return _mapper.Map<DoctorBusinessModel[]>(response);
    }

    public DoctorBusinessModel? UpdateDoctor(int id, UpdateDoctorDto doctorDto)
    {
        var entity = _unitOfWork.DoctorRepo.GetById(id);
        if (entity == null) return null;

        // Map updated fields from DTO to entity
        _mapper.Map(doctorDto, entity);

        _unitOfWork.Save();

        return _mapper.Map<DoctorBusinessModel>(entity);
    }
}