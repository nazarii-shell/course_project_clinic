using DataAccess.Repositories;

namespace BLTests;

using AutoMapper;
using BussinessLogic.Models;
using DataAccess.Models;
using DataAccess.UoW;
using DomainData.services;
using Moq;
using Xunit;
using System;

public class UserServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IGenericRepository<User>> _userRepoMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _userRepoMock = new Mock<IGenericRepository<User>>();

        _unitOfWorkMock.Setup(u => u.UserRepo).Returns(_userRepoMock.Object);
        _userService = new UserService(_unitOfWorkMock.Object, _mapperMock.Object);
    }
    
    [Fact]
    public void Exists_ShouldReturnTrue_WhenUserExists()
    {
        _userRepoMock.Setup(r => r.Exists(It.IsAny<Func<User, bool>>())).Returns(true);

        var result = _userService.Exists("test@example.com");

        Assert.True(result);
    }

    [Fact]
    public void Exists_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        _userRepoMock.Setup(r => r.Exists(It.IsAny<Func<User, bool>>())).Returns(false);

        var result = _userService.Exists("notfound@example.com");

        Assert.False(result);
    }
    
    [Fact]
    public void AddUser_ShouldCallCreateAndSave_AndReturnUser()
    {
        var user = new User { Id = 1, Email = "add@test.com" };

        _userRepoMock.Setup(r => r.Create(user)).Verifiable();
        _unitOfWorkMock.Setup(u => u.Save()).Verifiable();

        var result = _userService.AddUser(user);

        _userRepoMock.Verify(r => r.Create(user), Times.Once);
        _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        Assert.Equal(user, result);
    }
    
    [Fact]
    public void GetById_ShouldReturnCorrectUser()
    {
        var user = new User { Id = 42 };
        _userRepoMock.Setup(r => r.GetById(42)).Returns(user);

        var result = _userService.GetById(42);

        Assert.Equal(user, result);
    }
    
    [Fact]
    public void GetAllUsers_ShouldMapAndReturnBusinessModels()
    {
        var users = new []
        {
            new User { Id = 1, Email = "a@test.com" },
            new User { Id = 2, Email = "b@test.com" }
        };

        var mapped = new[]
        {
            new UserBusinessModel { Id = 1 },
            new UserBusinessModel { Id = 2 }
        };

        _userRepoMock.Setup(r => r.GetAll(null, null, null, null)).Returns(users);
        _mapperMock.Setup(m => m.Map<UserBusinessModel[]>(users)).Returns(mapped);

        var result = _userService.GetAllUsers();

        Assert.Equal(mapped, result);
    }
    
    [Fact]
    public void FindUser_ShouldReturnMatchingUser()
    {
        var user = new User { Id = 99, Email = "found@example.com" };
        _userRepoMock.Setup(r => r.Find(It.IsAny<Func<User, bool>>()))
            .Returns<Func<User, bool>>(predicate => predicate(user) ? user : null);

        var result = _userService.FindUser(u => u.Email == "found@example.com");

        Assert.NotNull(result);
        Assert.Equal("found@example.com", result.Email);
    }

    [Fact]
    public void FindUser_ShouldReturnNull_WhenNoMatch()
    {
        var user = new User { Id = 100, Email = "wrong@example.com" };
        _userRepoMock.Setup(r => r.Find(It.IsAny<Func<User, bool>>()))
            .Returns<Func<User, bool>>(predicate => predicate(user) ? user : null);

        var result = _userService.FindUser(u => u.Email == "no-match@example.com");

        Assert.Null(result);
    }
}

