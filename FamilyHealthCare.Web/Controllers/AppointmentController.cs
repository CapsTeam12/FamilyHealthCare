using Contract.DTOs;
using Contract.DTOs.AppoimentService;
using Contract.DTOs.ScheduleDoctorService;
using Data.Entities;
using FamilyHealthCare.Customer.Models;
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

        public async Task<IActionResult> Index(string userId) // Show list appointment of current User has logged in
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri("https://localhost:44316"); //gateway url
            var response = await client.GetAsync($"/Appointment/List/{userId}");
            string jsonData = await response.Content.ReadAsStringAsync();
            List<AppointmentDetailsDto> data = JsonConvert.DeserializeObject<List<AppointmentDetailsDto>>(jsonData);
            ViewBag.UserId = userId;
            return View(data);
        }

        public async Task<IActionResult> Details(string id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri("https://localhost:44316"); //gateway url
            var response = await client.GetAsync("/Appointment/" + id);
            string jsonData = await response.Content.ReadAsStringAsync();
            AppointmentDetailsDto data = JsonConvert.DeserializeObject<AppointmentDetailsDto>(jsonData);
            ViewBag.UserId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(data);
        }



        public async Task<IActionResult> Reschedule(string id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri("https://localhost:44316"); //gateway url
            var response = await client.GetAsync("/Appointment/" + id);
            string jsonData = await response.Content.ReadAsStringAsync();
            AppointmentDetailsDto data = JsonConvert.DeserializeObject<AppointmentDetailsDto>(jsonData);
            ViewBag.UserId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier); //get userId of current user logged in
            ViewBag.AppointmentId = data; // get AppointmentId
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reschedule(AppointmentRescheduleDto model,string id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri("https://localhost:44316"); //gateway url
            var stringModel = JsonConvert.SerializeObject(model);
            var data = new StringContent(stringModel,Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/Appointment/{id}",data);
            if (response != null && response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index),new { userId= model.UserId });
            }
            return View(model);
        }

        public async Task<List<Shift>> GetShifts()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri("https://localhost:44316"); //gateway url
            var response = await client.GetAsync("/Schedule/Shifts");
            string jsonData = await response.Content.ReadAsStringAsync();
            List<Shift> data = JsonConvert.DeserializeObject<List<Shift>>(jsonData);
            return data;
        }

        [Authorize]
        public async Task<IActionResult> Booking(string doctor, DateTime date,string doctorName,int Id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri("https://localhost:44316"); //gateway url
            var response = await client.GetAsync($"/Schedule/Doctor/{doctor}/{date.ToString("yyyy-MM-dd")}");
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


        public IActionResult BookingSummary(string Time,string userId,int therapistId,string doctorName,DateTime date)
        {
            var bookingView = new BookingViewModel()
            {
                therapistId = therapistId,
                userId = userId,
                doctorName = doctorName,
                StartTime = Convert.ToDateTime(Time.Split("-")[0]),
                EndTime = Convert.ToDateTime(Time.Split("-")[1]),
                Date = date
            };
            return View(bookingView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookingSummary(BookingViewModel model,string userId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri("https://localhost:44316"); //gateway url
            var stringModel = JsonConvert.SerializeObject(model);
            var data = new StringContent(stringModel, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/Appointment/Booking/{userId}", data);
            string jsonData = await response.Content.ReadAsStringAsync();
            if (response != null && response.IsSuccessStatusCode)
            {
                return RedirectToAction("BookingSuccess","Appointment",new { doctorName = model.doctorName,StartTime = model.StartTime,EndTime = model.EndTime,date = model.Date });
            }
            return View(model);
        }

        public IActionResult BookingSuccess(string doctorName,DateTime StartTime,DateTime EndTime,DateTime date)
        {
            ViewBag.doctorName = doctorName;
            ViewBag.StartTime = StartTime;
            ViewBag.EndTime = EndTime;
            ViewBag.Date = date;
            return View();
        }

    }
}
