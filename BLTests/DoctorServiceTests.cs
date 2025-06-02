using System.Linq.Expressions;
using AutoMapper;
using DataAccess.Models;
using DataAccess.UoW;
using DomainData.services;
using HospitalApp.DTOs.Doctor;
using BussinessLogic.Models;
using DataAccess.Repositories;
using HospitalApp.DTOs;
using Moq;
namespace BLTests;

public class DoctorServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<Doctor>> _doctorRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DoctorService _service;

    public DoctorServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _doctorRepoMock = new Mock<IGenericRepository<Doctor>>();
        _mapperMock = new Mock<IMapper>();

        _unitOfWorkMock.Setup(u => u.DoctorRepo).Returns(_doctorRepoMock.Object);

        _service = new DoctorService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void AddDoctor_ShouldCreateAndMapDoctor()
    {
        // Arrange
        var doctor = new Doctor { Id = 1, FullName = "Dr. Smith" };
        var createdDoctor = new Doctor { Id = 1, FullName = "Dr. Smith" };
        var mappedDoctor = new DoctorBusinessModel { FullName = "Dr. Smith" };

        _doctorRepoMock.Setup(r => r.Create(It.IsAny<Doctor>())).Returns(createdDoctor);
        _mapperMock.Setup(m => m.Map<DoctorBusinessModel>(createdDoctor)).Returns(mappedDoctor);

        // Act
        var result = _service.AddDoctor(doctor);

        // Assert
        _doctorRepoMock.Verify(r => r.Create(doctor), Times.Once);
        _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        _mapperMock.Verify(m => m.Map<DoctorBusinessModel>(createdDoctor), Times.Once);

        Assert.Equal(mappedDoctor, result);
    }

    [Fact]
    public void GetById_ShouldReturnDoctor()
    {
        // Arrange
        var doctor = new Doctor { Id = 2, FullName = "Dr. Jane" };
        _doctorRepoMock.Setup(r => r.GetById(2)).Returns(doctor);

        // Act
        var result = _service.GetById(2);

        // Assert
        _doctorRepoMock.Verify(r => r.GetById(2), Times.Once);
        Assert.Equal(doctor, result);
    }

    [Fact]
    public void Exists_ShouldReturnTrueIfExists()
    {
        // Arrange
        string email = "doc@example.com";
        _doctorRepoMock.Setup(r => r.Exists(It.IsAny<Func<Doctor, bool>>()))
            .Returns(true);
        

        // Act
        var result = _service.Exists(email);

        // Assert
        _doctorRepoMock.Verify(r => r.Exists(It.IsAny<Func<Doctor, bool>>()), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public void Exists_ShouldReturnFalseIfNotExists()
    {
        // Arrange
        string email = "doc@example.com";
        _doctorRepoMock.Setup(r => r.Exists(It.IsAny<Func<Doctor, bool>>()))
            .Returns(false);

        // Act
        var result = _service.Exists(email);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetAllDoctors_ShouldReturnMappedDoctors()
    {
        // Arrange
        string query = "Smith";
        string orderBy = "Name";
        var filter = new DoctorFilter { Specialty = "Cardiology", ServiceName = "Surgery" };

        var doctors = new []
        {
            new Doctor
            {
                Id = 1, FullName = "Dr. Smith", Specialty = "Cardiology",
                Services = new List<Service> { new Service { Name = "Surgery" } }
            },
            new Doctor
            {
                Id = 2, FullName = "Dr. Jones", Specialty = "Cardiology",
                Services = new List<Service> { new Service { Name = "Surgery" } }
            }
        };

        var mappedDoctors = new[]
        {
            new DoctorBusinessModel { FullName = "Dr. Smith" },
            new DoctorBusinessModel { FullName = "Dr. Jones" }
        };

        _doctorRepoMock.Setup(r => r.GetAll(
            It.IsAny<Expression<Func<Doctor, bool>>>(),
            It.IsAny<Expression<Func<Doctor, object>>>(),
            It.IsAny<Expression<Func<Doctor, bool>>>(),
            "Services"
        )).Returns(doctors);

        _mapperMock.Setup(m => m.Map<DoctorBusinessModel[]>(doctors)).Returns(mappedDoctors);

        // Act
        var result = _service.GetAllDoctors(query, orderBy, filter);

        // Assert
        _doctorRepoMock.Verify(r => r.GetAll(
            It.IsAny<Expression<Func<Doctor, bool>>>(),
            It.IsAny<Expression<Func<Doctor, object>>>(),
            It.IsAny<Expression<Func<Doctor, bool>>>(),
            "Services"
        ), Times.Once);

        _mapperMock.Verify(m => m.Map<DoctorBusinessModel[]>(doctors), Times.Once);

        Assert.Equal(mappedDoctors, result);
    }

    [Fact]
    public void UpdateDoctor_ShouldReturnUpdatedDoctor_WhenEntityExists()
    {
        // Arrange
        int id = 5;
        var existingDoctor = new Doctor { Id = id, FullName = "Dr. Old" };
        var doctorDto = new UpdateDoctorDto { FullName = "Dr. New" };
        var mappedDoctor = new DoctorBusinessModel { FullName = "Dr. New" };

        _doctorRepoMock.Setup(r => r.GetById(id)).Returns(existingDoctor);
        _mapperMock.Setup(m => m.Map(doctorDto, existingDoctor)).Verifiable();
        _mapperMock.Setup(m => m.Map<DoctorBusinessModel>(existingDoctor)).Returns(mappedDoctor);

        // Act
        var result = _service.UpdateDoctor(id, doctorDto);

        // Assert
        _doctorRepoMock.Verify(r => r.GetById(id), Times.Once);
        _mapperMock.Verify(m => m.Map(doctorDto, existingDoctor), Times.Once);
        _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        _mapperMock.Verify(m => m.Map<DoctorBusinessModel>(existingDoctor), Times.Once);

        Assert.Equal(mappedDoctor, result);
    }

    [Fact]
    public void UpdateDoctor_ShouldReturnNull_WhenEntityNotFound()
    {
        // Arrange
        int id = 5;
        var doctorDto = new UpdateDoctorDto { FullName = "Dr. New" };

        _doctorRepoMock.Setup(r => r.GetById(id)).Returns((Doctor)null);

        // Act
        var result = _service.UpdateDoctor(id, doctorDto);

        // Assert
        _doctorRepoMock.Verify(r => r.GetById(id), Times.Once);
        Assert.Null(result);
    }
}