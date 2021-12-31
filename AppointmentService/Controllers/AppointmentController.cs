using Business.IServices;
using Contract.DTOs;
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAppointments()
        {
            var appointment = await _appointmentService.GetAppointmentsAsync(search);
            return Ok(appointment);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAppoinmentsAsync([FromForm] AppointmentCreateDto appointmentCreateDto)
        {
            var createAppointment = await _appointmentService.CreateAppointmentAsync(appointmentCreateDto);
            return Ok(createAppointment);
        }


        //[HttpPost("create")]
        //public async Task<IActionResult> CreateAppoinmentsAsync([FromBody] AppointmentCreateDto appointmentCreateDto)
        //{
        //    return Ok();
        //}
    }
}
