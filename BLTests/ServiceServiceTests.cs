using DataAccess.Repositories;

namespace BLTests;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Moq;
using Xunit;
using DomainData.services;
using DataAccess.Models;
using BussinessLogic.Models;
using DataAccess.UoW;
using HospitalApp.DTOs;

public class ServiceServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<Service>> _serviceRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ServiceService _serviceService;

    public ServiceServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _serviceRepoMock = new Mock<IGenericRepository<Service>>();
        _mapperMock = new Mock<IMapper>();

        _unitOfWorkMock.Setup(u => u.ServiceRepo).Returns(_serviceRepoMock.Object);

        _serviceService = new ServiceService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void GetAll_ShouldReturnMappedServices()
    {
        // Arrange
        var services = new[]
        {
            new Service { Id = 1, Name = "Service1", Description = "Desc1", Price = 100 },
            new Service { Id = 2, Name = "Service2", Description = "Desc2", Price = 200 }
        };

        var mapped = new[]
        {
            new ServiceBusinessModel { Id = 1, Name = "Service1", Description = "Desc1", Price = 100 },
            new ServiceBusinessModel { Id = 2, Name = "Service2", Description = "Desc2", Price = 200 }
        };

        _serviceRepoMock.Setup(r => r.GetAll(null, null, null)).Returns(services);
        _mapperMock.Setup(m => m.Map<ServiceBusinessModel[]>(services)).Returns(mapped);

        // Act
        var result = _serviceService.GetAll();

        // Assert
        _serviceRepoMock.Verify(r => r.GetAll(null, null, null), Times.Once);
        _mapperMock.Verify(m => m.Map<ServiceBusinessModel[]>(services), Times.Once);
        Assert.Equal(mapped, result);
    }

    [Fact]
    public void AddService_ShouldCreateAndMapService()
    {
        // Arrange
        var serviceDto = new ServiceDto { Name = "New Service", Description = "New Desc", Price = 150 };
        var serviceEntity = new Service { Name = "New Service", Description = "New Desc", Price = 150 };
        var createdEntity = new Service { Id = 10, Name = "New Service", Description = "New Desc", Price = 150 };
        var mappedResult = new ServiceBusinessModel
            { Id = 10, Name = "New Service", Description = "New Desc", Price = 150 };

        _mapperMock.Setup(m => m.Map<Service>(serviceDto)).Returns(serviceEntity);
        _serviceRepoMock.Setup(r => r.Create(serviceEntity)).Returns(createdEntity);
        _mapperMock.Setup(m => m.Map<ServiceBusinessModel>(createdEntity)).Returns(mappedResult);

        // Act
        var result = _serviceService.AddService(serviceDto);

        // Assert
        _mapperMock.Verify(m => m.Map<Service>(serviceDto), Times.Once);
        _serviceRepoMock.Verify(r => r.Create(serviceEntity), Times.Once);
        _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        _mapperMock.Verify(m => m.Map<ServiceBusinessModel>(createdEntity), Times.Once);
        Assert.Equal(mappedResult, result);
    }
}