using DataAccess.Repositories;

namespace BLTests;

using System;
using AutoMapper;
using Moq;
using Xunit;
using DomainData.services;
using DataAccess.Models;
using BussinessLogic.Models;
using DataAccess.UoW;
using HospitalApp.DTOs;

public class PatientServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<Patient>> _patientRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly PatientService _service;

    public PatientServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _patientRepoMock = new Mock<IGenericRepository<Patient>>();
        _mapperMock = new Mock<IMapper>();

        _unitOfWorkMock.Setup(u => u.PatientRepo).Returns(_patientRepoMock.Object);

        _service = new PatientService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void AddPatient_ShouldCreateAndMapPatient()
    {
        // Arrange
        var patient = new Patient { Id = 1, FullName = "John Doe" };
        var createdPatient = new Patient { Id = 1, FullName = "John Doe" };
        var mappedPatient = new PatientBusinessModel { FullName = "John Doe" };

        _patientRepoMock
            .Setup(r => r.Create(It.IsAny<Patient>()))
            .Returns(createdPatient);

        _mapperMock
            .Setup(m => m.Map<PatientBusinessModel>(createdPatient))
            .Returns(mappedPatient);

        // Act
        var result = _service.AddPatient(patient);

        // Assert
        _patientRepoMock.Verify(r => r.Create(patient), Times.Once);
        _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        _mapperMock.Verify(m => m.Map<PatientBusinessModel>(createdPatient), Times.Once);

        Assert.Equal(mappedPatient, result);
    }

    [Fact]
    public void GetById_ShouldReturnPatientFromRepo()
    {
        // Arrange
        var patient = new Patient { Id = 5, FullName = "Jane Smith" };

        _patientRepoMock
            .Setup(r => r.GetById(5))
            .Returns(patient);

        // Act
        var result = _service.GetById(5);

        // Assert
        _patientRepoMock.Verify(r => r.GetById(5), Times.Once);
        Assert.Equal(patient, result);
    }

    [Fact]
    public void GetAllPatients_ShouldReturnMappedArray()
    {
        // Arrange
        var patients = new[]
        {
            new Patient { Id = 1, FullName = "John Doe" },
            new Patient { Id = 2, FullName = "Jane Smith" }
        };

        var mappedPatients = new[]
        {
            new PatientBusinessModel { FullName = "John Doe" },
            new PatientBusinessModel { FullName = "Jane Smith" }
        };

        _patientRepoMock
            .Setup(r => r.GetAll(null, null, null))
            .Returns(patients);

        _mapperMock
            .Setup(m => m.Map<PatientBusinessModel[]>(patients))
            .Returns(mappedPatients);

        // Act
        var result = _service.GetAllPatients();

        // Assert
        _patientRepoMock.Verify(r => r.GetAll(null, null, null), Times.Once);
        _mapperMock.Verify(m => m.Map<PatientBusinessModel[]>(patients), Times.Once);
        Assert.Equal(mappedPatients, result);
    }

    [Fact]
    public void UpdatePatient_ShouldReturnNull_IfPatientNotFound()
    {
        // Arrange
        _patientRepoMock
            .Setup(r => r.GetById(It.IsAny<int>()))
            .Returns((Patient)null);

        var dto = new UpdatePatientDto();

        // Act
        var result = _service.UpdatePatient(1, dto);

        // Assert
        Assert.Null(result);
        _patientRepoMock.Verify(r => r.GetById(1), Times.Once);
        _unitOfWorkMock.Verify(u => u.Save(), Times.Never);
        _mapperMock.Verify(m => m.Map(It.IsAny<UpdatePatientDto>(), It.IsAny<Patient>()), Times.Never);
    }

    [Fact]
    public void UpdatePatient_ShouldMapAndReturnUpdatedPatient()
    {
        // Arrange
        var existingPatient = new Patient { Id = 1, FullName = "Old Name" };
        var dto = new UpdatePatientDto { /* set properties as needed */ };
        var mappedPatientBusinessModel = new PatientBusinessModel { FullName = "Updated Name" };

        _patientRepoMock
            .Setup(r => r.GetById(1))
            .Returns(existingPatient);

        _mapperMock
            .Setup(m => m.Map(dto, existingPatient))
            .Verifiable();

        _unitOfWorkMock
            .Setup(u => u.Save())
            .Verifiable();

        _mapperMock
            .Setup(m => m.Map<PatientBusinessModel>(existingPatient))
            .Returns(mappedPatientBusinessModel);

        // Act
        var result = _service.UpdatePatient(1, dto);

        // Assert
        _patientRepoMock.Verify(r => r.GetById(1), Times.Once);
        _mapperMock.Verify(m => m.Map(dto, existingPatient), Times.Once);
        _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        _mapperMock.Verify(m => m.Map<PatientBusinessModel>(existingPatient), Times.Once);
        Assert.Equal(mappedPatientBusinessModel, result);
    }
}
