using AutoMapper;
using BussinessLogic.Models;
using DataAccess.Models;
using DataAccess.UoW;
using HospitalApp.DTOs;

namespace DomainData.services;

public class PatientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public PatientService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public PatientBusinessModel AddPatient(Patient patient)
    {
        var createdPatient = _unitOfWork.PatientRepo.Create(patient);
        _unitOfWork.Save();
        
        return _mapper.Map<PatientBusinessModel>(createdPatient) ;
    }

    public Patient GetById(int id)
    {
        return _unitOfWork.PatientRepo.GetById(id);
    }

    
    public PatientBusinessModel[] GetAllPatients()
    {
        var res=  _unitOfWork.PatientRepo.GetAll();
        return _mapper.Map<PatientBusinessModel[]>(res);
    }

    public PatientBusinessModel? UpdatePatient(int id, UpdatePatientDto patientDto)
    {
        var entity = _unitOfWork.PatientRepo.GetById(id);
        if (entity == null) return null;
        
        // Map updated fields from DTO to entity
        _mapper.Map(patientDto, entity);
        
        _unitOfWork.Save();
        
        return _mapper.Map<PatientBusinessModel>(entity);
    }
}