using Business.IServices;
using Contract.DTOs.ScheduleDoctorService;
using Contract.DTOs.ScheduleService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly IScheduleDoctorService _scheduleDoctorService;
        public ScheduleController(IScheduleService scheduleService, IScheduleDoctorService scheduleDoctorService)
        {
            _scheduleService = scheduleService;
            _scheduleDoctorService = scheduleDoctorService;
        }


        [HttpGet("Doctor/{userId}/{date}")]
        public async Task<IActionResult> GetSchedulesOfDoctor(string userId, DateTime date)
        {

            var scheduleDto = await _scheduleDoctorService.GetSchedulesAsync(userId, date);
            return Ok(scheduleDto);
        }

        //[HttpPost("Doctor/{shiftsId}")]
        //public async Task<IActionResult> CreateSchedulesOfDoctor([FromBody]ScheduleDoctorCreateDto model,[FromQuery] int[] shiftsId)
        //{
        //    var scheduleDto = await _scheduleDoctorService.CreateScheduleAsync(model,shiftsId);
        //    return Ok(scheduleDto);
        //}

        [HttpPost("Doctor")]
        public async Task<IActionResult> CreateSchedulesOfDoctor([FromBody] ScheduleDoctorCreateDto model)
        {
            var scheduleDto = await _scheduleDoctorService.CreateScheduleAsync(model);
            return Ok(scheduleDto);
        }


        [HttpGet("Shifts")]
        public async Task<IActionResult> GetShifts()
        {
            var shifts = await _scheduleDoctorService.GetShiftsAsync();
            return Ok(shifts);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }
            var scheduleDto = await _scheduleService.GetSchedulesAsync(userId);
            return Ok(scheduleDto);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ScheduleCreateDto schedule)
        {
            var scheduleDto = await _scheduleService.CreateScheduleAsync(schedule);
            return Ok(scheduleDto);
        }


    }
}
