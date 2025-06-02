using DomainData.services;
using HospitalApp.DTOs;
using Moq;
using DataAccess.UoW; // your namespaces
using AutoMapper;
using DataAccess.Models;
using DataAccess.Repositories;
using PresentationLayer.Utils;

public class AuthServiceTests
{
    private readonly AuthService _authService;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IGenericRepository<User>> _userRepoMock;

    public AuthServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _userRepoMock = new Mock<IGenericRepository<User>>();

        // Setup mocks as needed for UserService's FindUser

        // For example, mock UserRepo.Find method:
        var userRepoMock = new Mock<IGenericRepository<DataAccess.Models.User>>();
        _unitOfWorkMock.Setup(u => u.UserRepo).Returns(userRepoMock.Object);

        // We'll setup Find in the test method below.

        var userService = new UserService(_unitOfWorkMock.Object, _mapperMock.Object);
        _authService = new AuthService(userService);
    }

    [Fact]
    public void CheckAuth_ReturnsTrue_WhenUserExistsAndPasswordAndRoleMatch()
    {
        // Arrange
        var authDto = new RegisterUserWithAuth
        {
            Auth = new Auth { Email = "test@example.com", Password = "password123" }
        };
        var role = "Admin";

        var expectedUser = new User
        {
            Email = "test@example.com",
            Password = "password123",
            Role = "Admin"
        };

        // Create mock of user repository
        var userRepoMock = new Mock<IGenericRepository<User>>();

        // Setup unitOfWorkMock to return the mock repository object
        _unitOfWorkMock.Setup(u => u.UserRepo).Returns(userRepoMock.Object);

        // Now you can setup Find on the userRepoMock:
        userRepoMock.Setup(repo =>
                repo.Find(It.IsAny<Func<User, bool>>()))
            .Returns<Func<User, bool>>(predicate =>
                predicate(expectedUser) ? expectedUser : null);

        // Act
        var result = _authService.CheckAuth(authDto, role);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void CheckAuth_UserNotFound_ReturnsFalse()
    {
        // Arrange
        var authDto = new RegisterUserWithAuth
        {
            Auth = new Auth { Email = "notfound@example.com", Password = "pass" }
        };

        // Setup Find to always return null (user not found)
        _userRepoMock.Setup(r => r.Find(It.IsAny<Func<User, bool>>()))
            .Returns((User)null);

        // Act
        var result = _authService.CheckAuth(authDto, "Admin");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CheckAuth_WrongPassword_ReturnsFalse()
    {
        // Arrange
        var user = new User
        {
            Email = "test@example.com",
            Password = "correctpassword",
            Role = "User"
        };

        var authDto = new RegisterUserWithAuth
        {
            Auth = new Auth { Email = user.Email, Password = "wrongpassword" }
        };

        _userRepoMock.Setup(r => r.Find(It.IsAny<Func<User, bool>>()))
            .Returns<Func<User, bool>>(predicate => predicate(user) ? user : null);

        // Act
        var result = _authService.CheckAuth(authDto, "User");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CheckAuth_WrongRole_ReturnsFalse()
    {
        // Arrange
        var user = new User
        {
            Email = "test@example.com",
            Password = "password",
            Role = "User"
        };

        var authDto = new RegisterUserWithAuth
        {
            Auth = new Auth { Email = user.Email, Password = user.Password }
        };

        _userRepoMock.Setup(r => r.Find(It.IsAny<Func<User, bool>>()))
            .Returns<Func<User, bool>>(predicate => predicate(user) ? user : null);

        // Act
        var result = _authService.CheckAuth(authDto, "Admin");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Auth_ReturnsFalse()
    {
        // Arrange
        DtoWithAuth authDto = null;

        // Act
        var result = _authService.CheckAuth(authDto, "Admin");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CheckAuth_NullAuthProperty_ReturnsFalse()
    {
        // Arrange
        var authDto = new RegisterUserWithAuth { Auth = null };

        // Act
        var result = _authService.CheckAuth(authDto, "Admin");

        // Assert
        Assert.False(result);
    }
}