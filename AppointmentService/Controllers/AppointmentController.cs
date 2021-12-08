using Business.IServices;
using Contract.DTOs;
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
        [HttpGet]
        public async Task<IActionResult> GetAppoinmentsAsync(string search)
        {
            return await _appointmentService.GetAppointmentsAsync(search);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAppoinmentsAsync([FromBody] AppointmentCreateDto appointmentCreateDto)
        {
            return Ok();
        }
    }
}
