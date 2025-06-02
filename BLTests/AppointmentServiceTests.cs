using BussinessLogic.Models;
using DomainData.Repositories;


using System;
using System.Linq.Expressions;
using AutoMapper;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.UoW;
using DomainData.services;
using HospitalApp.DTOs;
using Moq;
using Xunit;

namespace BLTests;


public class AppointmentServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<Appointment>> _appointmentRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AppoinmentService _service;

    public AppointmentServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _appointmentRepoMock = new Mock<IGenericRepository<Appointment>>();
        _mapperMock = new Mock<IMapper>();

        _unitOfWorkMock.Setup(u => u.AppointmentRepo).Returns(_appointmentRepoMock.Object);

        _service = new AppoinmentService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void AddAppointment_ShouldCreateAndMapAppointment()
    {
        // Arrange
        var appointment = new Appointment { Id = 1, Reason = "Checkup", AppointmentDate = DateTime.Now };
        var createdAppointment = new Appointment { Id = 1, Reason = "Checkup", AppointmentDate = DateTime.Now };
        var mappedResult = new AppointmentBusinessModel { Reason = "Checkup" };

        _appointmentRepoMock
            .Setup(r => r.Create(It.IsAny<Appointment>()))
            .Returns(createdAppointment);

        _mapperMock
            .Setup(m => m.Map<AppointmentBusinessModel>(createdAppointment))
            .Returns(mappedResult);

        // Act
        var result = _service.AddApoinment(appointment);

        // Assert
        _appointmentRepoMock.Verify(r => r.Create(appointment), Times.Once);
        _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        _mapperMock.Verify(m => m.Map<AppointmentBusinessModel>(createdAppointment), Times.Once);

        Assert.Equal(mappedResult, result);
    }

    [Fact]
    public void GetAllAppointments_ShouldReturnMappedArray()
    {
        // Arrange
        var orderBy = "Date";
        var appointments = new[]
        {
            new Appointment { Id = 1, Reason = "Checkup", AppointmentDate = DateTime.Today },
            new Appointment { Id = 2, Reason = "Consultation", AppointmentDate = DateTime.Today.AddDays(1) }
        };
        var mappedAppointments = new[]
        {
            new AppointmentBusinessModel {  Reason = "Checkup" },
            new AppointmentBusinessModel {  Reason = "Consultation" }
        };

        _appointmentRepoMock
            .Setup(r => r.GetAll(
                null,
                It.IsAny<Expression<Func<Appointment, object>>>(),
                null,
                "Patient", "Doctor"))
            .Returns(appointments);

        _mapperMock
            .Setup(m => m.Map<AppointmentBusinessModel[]>(appointments))
            .Returns(mappedAppointments);

        // Act
        var result = _service.GetAllAppoinments(orderBy);

        // Assert
        _appointmentRepoMock.Verify(r => r.GetAll(
            null,
            It.IsAny<Expression<Func<Appointment, object>>>(),
            null,
            "Patient", "Doctor"), Times.Once);

        _mapperMock.Verify(m => m.Map<AppointmentBusinessModel[]>(appointments), Times.Once);

        Assert.Equal(mappedAppointments, result);
    }
}
