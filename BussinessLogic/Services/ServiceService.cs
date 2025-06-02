using System.Linq.Expressions;
using AutoMapper;
using BussinessLogic.Models;
using DataAccess.Models;
using DataAccess.UoW;
using HospitalApp.DTOs;

namespace DomainData.services;

public class ServiceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ServiceService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public ServiceBusinessModel[] GetAll()
    {
        var res = _unitOfWork.ServiceRepo.GetAll();
        return _mapper.Map<ServiceBusinessModel[]>(res);
    }

    public ServiceBusinessModel AddService(ServiceDto serviceDto)
    {
        var service = _mapper.Map<Service>(serviceDto);
        var newService = _unitOfWork.ServiceRepo.Create(service);
        _unitOfWork.Save();
        return _mapper.Map<ServiceBusinessModel>(newService);
    }

    public ServiceBusinessModel[] GetAllServices(string query, string orderBy)
    {
        Expression<Func<Service, bool>> search =
            query != null ? (x) => x.Name.Contains(query) || x.Description.Contains(query) : null;
        Expression<Func<Service, object>> orderByPredicate =
            ((service) => orderBy == "Price" ? service.Price : service.Name);

        var all = _unitOfWork.ServiceRepo.GetAll(search, orderByPredicate);
        return _mapper.Map<ServiceBusinessModel[]>(all);
    }

  
}