using Contract.DTOs;
using Contract.DTOs.AppoimentService;
using Contract.DTOs.ScheduleDoctorService;
using Data.Entities;
using FamilyHealthCare.Customer.Models;
using FamilyHealthCare.SharedLibrary;
using FamilyHealthCare.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Controllers
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

        public async Task<IActionResult> Index(string userId) // Show list appointment of User 
        {
            var userCurrent = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier); //get userId of current user logged in
            if (userCurrent != userId) // Check if the currently logged in user is the same as the userId
            {
                return Forbid();
            }
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.AppointmentService.LIST + userId);
            string jsonData = await response.Content.ReadAsStringAsync();
            List<AppointmentDetailsDto> data = JsonConvert.DeserializeObject<List<AppointmentDetailsDto>>(jsonData);
            ViewBag.UserId = userId;
            return View(data);
        }

        public async Task<IActionResult> Details(string id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.AppointmentService.DETAILS + id);
            string jsonData = await response.Content.ReadAsStringAsync();
            AppointmentDetailsDto data = JsonConvert.DeserializeObject<AppointmentDetailsDto>(jsonData);
            ViewBag.UserId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(data);
        }



        public async Task<IActionResult> Reschedule(string id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.AppointmentService.RESCHEDULE + id);
            string jsonData = await response.Content.ReadAsStringAsync();
            AppointmentDetailsDto data = JsonConvert.DeserializeObject<AppointmentDetailsDto>(jsonData);
            ViewBag.UserId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier); //get userId of current user logged in
            ViewBag.AppointmentId = data; // get AppointmentId
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reschedule(AppointmentRescheduleDto model, string id)
        {
            var stringModel = JsonConvert.SerializeObject(model);
            var data = new StringContent(stringModel, Encoding.UTF8, "application/json");
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.PutAsync(EndpointConstants.AppointmentService.DETAILS + id, data);
            if (response != null && response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index), new { userId = model.UserId });
            }
            return View(model);
        }

        public async Task<List<Shift>> GetShifts()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ScheduleService.SHIFTS);
            string jsonData = await response.Content.ReadAsStringAsync();
            List<Shift> data = JsonConvert.DeserializeObject<List<Shift>>(jsonData);
            return data;
        }

        [Authorize]
        public async Task<IActionResult> Booking(string doctor, DateTime date, string doctorName, int Id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ScheduleService.DOCTOR_SCHEDULES + "/" + doctor + "/" + date.ToString("yyyy-MM-dd"));
            string jsonData = await response.Content.ReadAsStringAsync();
            List<ScheduleDoctorDto> data = JsonConvert.DeserializeObject<List<ScheduleDoctorDto>>(jsonData);
            List<Shift> shiftdata = await GetShifts();
            var ScheduleView = new ScheduleViewModel
            {
                Shifts = shiftdata,
                ScheduleDoctors = data
            };
            ViewBag.doctorId = Id; // get doctorId
            ViewBag.doctor = doctor; // get userId of doctor
            ViewBag.userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier); //get userId of current user logged in
            ViewBag.CurrentDate = date; //get currentDate
            ViewBag.doctorName = doctorName; // get doctor fullname
            return View(ScheduleView);
        }


        public IActionResult BookingSummary(string Time, string userId, int therapistId, string doctorName, DateTime date,string doctor)
        {
            if (Time == null)
            {
                TempData["StatusMessage"] = "No appointment has been selected";
                return RedirectToAction("Booking", "Appointment", new { doctor = doctor, date = date, doctorName = doctorName, Id = therapistId  });
            }
            var bookingView = new BookingViewModel()
            {
                therapistId = therapistId,
                userId = userId,
                doctorName = doctorName,
                StartTime = Convert.ToDateTime(date.ToString("yyyy-MM-dd") + " " + Time.Split("-")[0]),
                EndTime = Convert.ToDateTime(date.ToString("yyyy-MM-dd") + " " + Time.Split("-")[1]),
                Date = date
            };
            return View(bookingView);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookingSummary(BookingViewModel model, string userId)
        {
            if (String.IsNullOrEmpty(model.Description)) model.Description = "No information";
            var stringModel = JsonConvert.SerializeObject(model);
            var data = new StringContent(stringModel, Encoding.UTF8, "application/json");
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.PostAsync(EndpointConstants.AppointmentService.BOOKING + userId, data);
            string jsonData = await response.Content.ReadAsStringAsync();
            if (response != null && response.IsSuccessStatusCode)
            {
                return RedirectToAction("BookingSuccess", "Appointment", new { doctorName = model.doctorName, StartTime = model.StartTime, EndTime = model.EndTime, date = model.Date });
            }
            return View(model);
        }

        public IActionResult BookingSuccess(string doctorName, DateTime StartTime, DateTime EndTime, DateTime date)
        {
            ViewBag.doctorName = doctorName;
            ViewBag.StartTime = StartTime;
            ViewBag.EndTime = EndTime;
            ViewBag.Date = date;
            return View();
        }

    }
}
