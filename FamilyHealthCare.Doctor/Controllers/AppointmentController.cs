using Contract.DTOs;
using Contract.DTOs.AppoimentService;
using Contract.DTOs.ScheduleDoctorService;
using Data.Entities;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHealthCare.Doctor.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<AppointmentController> _logger;
        private readonly IHttpContextAccessor _httpContext;

        public AppointmentController(ILogger<AppointmentController> logger, IHttpClientFactory httpClient, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpContext = httpContext;
        }

        public async Task<IActionResult> CancelAppointment(string id)
        {
            var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(string.Format(EndpointConstants.AppointmentService.CANCEL, id, userId));
            string jsonData = await response.Content.ReadAsStringAsync();
            if (jsonData.Equals("Appointment completed!") || jsonData.Equals("Appointment is in progress!") || jsonData.Equals("Can only cancel the appointment at least two hour!") || jsonData.Equals("This appointment has been canceled before!"))
            {
                return Json(new { content = jsonData });
            }
            AppointmentDetailsDto data = JsonConvert.DeserializeObject<AppointmentDetailsDto>(jsonData);
            return Json(new { data = data });
        }

        public async Task<IActionResult> Details(string id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.AppointmentService.DETAILS + id);
            string jsonData = await response.Content.ReadAsStringAsync();
            AppointmentDetailsDto data = JsonConvert.DeserializeObject<AppointmentDetailsDto>(jsonData);
            ViewBag.UserId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Json(new { data = data });
        }



        public async Task<IActionResult> Reschedule(string doctor, DateTime date)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ScheduleService.DOCTOR_SCHEDULES + "/" + doctor + "/" + date.ToString("yyyy-MM-dd"));
            string jsonData = await response.Content.ReadAsStringAsync();
            List<ScheduleDoctorDto> data = JsonConvert.DeserializeObject<List<ScheduleDoctorDto>>(jsonData);
            List<Shift> shiftdata = await GetShifts();
            return Json(new { dataSchedules = data, dataShifts = shiftdata });
        }

        [HttpPut]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> RescheduleAppointment(AppointmentRescheduleDto model, string id)
        {
            if (model.StartTime.ToString() == "1/1/0001 12:00:00 AM")
            {
                return Json(new { errorMessage = "No appointment selected!" });
            }
            else if (model.Status == 4)
            {
                return Json(new { errorMessage = "This appointment has been canceled before!" });
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                model.Description = "No information";
            }
            var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            model.AccountId = userId;
            var stringModel = JsonConvert.SerializeObject(model);
            var data = new StringContent(stringModel, Encoding.UTF8, "application/json");
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.PutAsync(EndpointConstants.AppointmentService.RESCHEDULE + id, data);
            string jsonData = await response.Content.ReadAsStringAsync();
            if (response != null && response.IsSuccessStatusCode && jsonData.Equals("Can only reschedule the appointment at least two hour!"))
            {
                return Json(new { content = jsonData });
            }
            else if (response != null && response.IsSuccessStatusCode && jsonData.Equals("You had an appointment at the same time before!"))
            {
                return Json(new { content = jsonData });
            }
            else if (response != null && response.IsSuccessStatusCode && jsonData.Equals("This appointment has been canceled before!"))
            {
                return Json(new { errorMessage = jsonData });
            }
            else
            {
                var appointmentDetails = JsonConvert.DeserializeObject<AppointmentDetailsDto>(jsonData);
                return Json(new { data = appointmentDetails });
            }
        }

        public async Task<List<Shift>> GetShifts()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ScheduleService.SHIFTS);
            string jsonData = await response.Content.ReadAsStringAsync();
            List<Shift> data = JsonConvert.DeserializeObject<List<Shift>>(jsonData);
            return data;
        }
    }
}
