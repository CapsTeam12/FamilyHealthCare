using Business.IServices;
using Contract.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookingAppoimentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingAppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public BookingAppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        [Route("Booking/{userId}")]
        public async Task<IActionResult> Post([FromBody] AppointmentCreateDto createDto, string userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            var appointmentDto = await _appointmentService.BookingAppointment(createDto, userId);
            if (appointmentDto == null)
            {
                return NotFound();
            }
            return Ok(appointmentDto);
        }
    }
}
