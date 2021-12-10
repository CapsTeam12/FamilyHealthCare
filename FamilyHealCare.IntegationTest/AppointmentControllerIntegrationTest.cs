using AutoMapper;
using Business;
using Data;
using Data.Entities;
using FamilyHealCare.IntegrationTests.Common;
using System;
using Xunit;
using Business.Services;
using AppointmentService.Controllers;
using FamilyHealCare.IntegationTest.TestData;
using Microsoft.AspNetCore.Mvc;
using Contract.DTOs;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FamilyHealCare.IntegationTest
{
    public class AppointmentControllerIntegrationTest : IClassFixture<SqliteInMemoryFixture>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly BaseRepository<Appointment> _appointmentRepository;
        private readonly BaseRepository<User> _userRepository;
        private readonly ClsAppointmentService _appointmentService;
        private readonly AppointmentController _appointmentController;

        private readonly IMapper _mapper;

        public AppointmentControllerIntegrationTest(SqliteInMemoryFixture fixture)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
            _mapper = config.CreateMapper();

            fixture.CreateDatabase();
            _dbContext = fixture.Context;

            _appointmentRepository = new BaseRepository<Appointment>(_dbContext);
            _userRepository = new BaseRepository<User>(_dbContext);

            _appointmentService = new ClsAppointmentService(_appointmentRepository, _mapper);
            _appointmentController = new AppointmentController(_appointmentService);

            UserArrangeData.InitUserDataAsync(_userRepository).Wait();
            AppointmentArrangeData.InitAppointmentDataAsync(_appointmentRepository).Wait();
        }

        [Fact]
        public async Task GetAppointmentSuccess()
        {
            //Arrange
            var appointmentDto = AppointmentArrangeData.GetAppointmentDto();
            _appointmentController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { }
            };

            ActionResult<AppointmentDetailsDto> result;

            //Action
            using (var scope = _dbContext.Database.BeginTransaction())
            {
                result = (ActionResult)await _appointmentController.GetAppoinmentsAsync(appointmentDto);
            }

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
        }
        [Fact]
        public async Task CreateAppointmentSuccess()
        {
            //Arrange
            var appointmentCreateDto = AppointmentArrangeData.GetAppointmentCreateDto();
            _appointmentController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { }
            };

            ActionResult<AppointmentDetailsDto> result;

            //Action
            using (var scope = _dbContext.Database.BeginTransaction())
            {
                result = (ActionResult)await _appointmentController.CreateAppoinmentsAsync(appointmentCreateDto);
            }

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}
