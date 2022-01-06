using Business.IServices;
using Contract.DTOs;
using Contract.DTOs.AppoimentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppointmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // GET: api/<AppointmentController>
        //[HttpGet]
        //public async Task<IActionResult> GetAppoinmentsAsync(string search)
        //{
        //    return await _appointmentService.GetAppointmentsAsync(search);
        //}

        [HttpGet]
        [Route("List/{userId}")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get(string userId) // Get list appointment of user
        {
            var appointmentDto = await _appointmentService.GetAppointments(userId);
            return Ok(appointmentDto);
        }

        [HttpGet("{id}")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetById(string id)
        {
            var appointmentDto = await _appointmentService.GetAppointmentById(id);
            if(appointmentDto == null)
            {
                return NotFound();
            }
            return Ok(appointmentDto);
        }

        [HttpPost]
        [Route("Booking/{userId}")]
        public async Task<IActionResult> Post([FromBody] AppointmentCreateDto createDto,string userId)
        {
            var appointmentDto = await _appointmentService.BookingAppointment(createDto, userId);
            if(userId == null)
            {
                return BadRequest();
            }
            return Ok(appointmentDto);
        }

        [HttpPut("{id}")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Put([FromBody] AppointmentRescheduleDto appointmentDto,string id)
        {
            var model = await _appointmentService.RescheduleAppointment(appointmentDto, id);
            if(model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpDelete("{id}")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await _appointmentService.GetAppointmentById(id);
            if(model == null)
            {
                return NotFound();
            }
            await _appointmentService.CancelAppointment(id);
            return NoContent();
        }



        //[HttpPost("create")]
        //public async Task<IActionResult> CreateAppoinmentsAsync([FromBody] AppointmentCreateDto appointmentCreateDto)
        //{
        //    return Ok();
        //}
    }
}
