using Business.IServices;
using Contract.DTOs;
using Contract.DTOs.AppoimentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
        public async Task<IActionResult> Post([FromBody] AppointmentCreateDto createDto, string userId)
        {
            if(userId == null)
            {
                return BadRequest();
            }
            var appointmentDto = await _appointmentService.BookingAppointment(createDto, userId);
            if(appointmentDto == null)
            {
                return NotFound();
            }
            return Ok(appointmentDto);
        }

        [HttpPut("Reschedule/{id}")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Put([FromBody] AppointmentRescheduleDto appointmentDto,string id)
        {
            var appointment = await _appointmentService.GetAppointmentById(id);
            DateTime startTime = appointment.StartTime.ToLocalTime(); // Thời gian bắt đầu của cuộc hẹn
            DateTime currentTime = DateTime.Now; // Thời gian hiện tại 
            var HourDistance = (startTime - currentTime).TotalHours; // Khoảng cách giờ giữa thời gian bắt đầu cuộc hẹn và thời gian hiện tại 
            if(currentTime == startTime && currentTime <= appointment.EndTime)
            {
                return Content("Appointment is in progress!");
            }
            if(appointment.Status == 4)
            {
                return Content("This appointment has been canceled before!");
            }
            if(HourDistance < 2)
            {
                return Content("Can only reschedule the appointment at least two hour!");
            }
            var model = await _appointmentService.RescheduleAppointment(appointmentDto, id);
            if (model == null)
            {
                return Content("You had an appointment at the same time before!");
            }
            return Ok(model);
        }

        [HttpGet("Cancel/{id}")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Cancel(string id)
        {
            var model = await _appointmentService.GetAppointmentById(id);
            if(model == null)
            {
                return NotFound();
            }
            if(model.Status == 4)
            {
                return Content("This appointment has been canceled before!");
            }
            var appointment = await _appointmentService.CancelAppointment(id);
            DateTime startTime = appointment.StartTime.ToLocalTime(); // Thời gian bắt đầu của cuộc hẹn
            DateTime currentTime = DateTime.Now; // Thời gian hiện tại 
            var HourDistance = (startTime - currentTime).TotalHours; // Khoảng cách giờ giữa thời gian bắt đầu cuộc hẹn và thời gian hiện tại 
            if (appointment.Status == 3)
            {
                return Content("Appointment completed!");
            }
            else if(appointment.Status == 2)
            {
                return Content("Appointment is in progress!");
            }else if(appointment.Status == 1 && HourDistance < 2)
            {
                return Content("Can only cancel the appointment at least two hour!");
            }
            return Ok(appointment);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetTotalAppointments()
        {
            return Ok( await _appointmentService.GetTotalAppointments());
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetTotalAppointmentsByDoctor(string id)
        {
            return Ok(_appointmentService.GetTotalAppointmentsByDoctor(id));
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetTotalAppointmentsByPatient(string id)
        {
            return Ok(_appointmentService.GetTotalAppointmentsByPatient(id));
        }

    }
}
