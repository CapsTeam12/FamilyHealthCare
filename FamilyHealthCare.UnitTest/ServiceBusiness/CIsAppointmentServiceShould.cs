using AutoMapper;
using Business;
using Business.IServices;
using Business.Services;
using Data.Entities;
using FamilyHealCare.UnitTests.TestData;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FamilyHealthCare.UnitTest.ServiceBusiness
{
    public class CIsAppointmentServiceShould
    {
        private readonly ClsAppointmentService _appointmentService;
        private readonly Mock<IBaseRepository<Appointment>> _appointmentRepository;
        private readonly IMapper _mapper;

        public CIsAppointmentServiceShould()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
            _mapper = config.CreateMapper();
            _appointmentRepository = new Mock<IBaseRepository<Appointment>>();
            _appointmentService = new ClsAppointmentService(
                _appointmentRepository.Object,
                _mapper);
        }

        [Fact]
        public async Task ValidAppointmentDetailsDtoShouldBeFoundSuccess()
        {
            //Arrange
            var appointments = new List<Appointment>();
            _appointmentRepository
                .Setup(x => x.Entities)
                .Returns(appointments.AsQueryable().BuildMock().Object);

            //Action
            IActionResult result = await _appointmentService.GetAppointmentsAsync("Examination");

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task ValidAppointmentCreateDtoShouldBeSuccess()
        {
            //Arrange
            var appointmentCreateDto = AppointmentTestData.ValidGetAppointmentCreateDto();

            var appointments = new List<Appointment>();
            _appointmentRepository
                .Setup(x => x.Entities)
                .Returns(appointments.AsQueryable().BuildMock().Object);

            //Action
            IActionResult result = await _appointmentService.CreateAppointmentAsync(appointmentCreateDto);

            //Assert
            result.Should().NotBeNull();
            _appointmentRepository.Verify(mock => mock.Create(It.IsAny<Appointment>()), Times.Once);
        }
    }
}
