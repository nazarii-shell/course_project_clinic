using System.Linq.Expressions;
using AutoMapper;
using BussinessLogic.Models;
using DataAccess.Models;
using DataAccess.UoW;
using HospitalApp.DTOs;

namespace DomainData.services;

public class AppoinmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public AppoinmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public AppointmentBusinessModel AddApoinment(Appointment appoinment)
    {
        var createdAppointment = _unitOfWork.AppointmentRepo.Create(appoinment);
        _unitOfWork.Save();
        
        return _mapper.Map<AppointmentBusinessModel>(createdAppointment) ;
    }
    
    public AppointmentBusinessModel[] GetAllAppoinments(string? orderBy)
    {
        Expression<Func<Appointment, object>>? orderByPredicate = (a => orderBy == "Date" ? a.AppointmentDate : a.Reason);

        
        var res=  _unitOfWork.AppointmentRepo.GetAll(null, orderByPredicate, null,"Patient", "Doctor");
        
        return _mapper.Map<AppointmentBusinessModel[]>(res);
    }
}