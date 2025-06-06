using AutoMapper;
using BussinessLogic.Models;
using DataAccess.Models;
using DataAccess.UoW;
using HospitalApp.DTOs;

namespace DomainData.services;

public class UserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public bool Exists(string email)
    {
        if (_unitOfWork.UserRepo.Exists(u => u.Email == email))
            return true;
        
        return false;
    }

    public User AddUser(User user)
    {
        _unitOfWork.UserRepo.Create(user);
        _unitOfWork.Save();
        
        return user;
    }

    public User GetById(int id)
    {
        return _unitOfWork.UserRepo.GetById(id);
    }
    
    public UserBusinessModel[] GetAllUsers()
    {
        var res=  _unitOfWork.UserRepo.GetAll(null, null, null, null);
        
        return _mapper.Map<UserBusinessModel[]>(res);
    }

    public User? FindUser(Func<User, bool> predicate)
    {
        return _unitOfWork.UserRepo.Find(predicate);
    }
}