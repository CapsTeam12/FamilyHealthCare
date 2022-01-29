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

        public ActionResult Index() 
        {
            return View();
        }

        public async Task<IActionResult> GetData(int? page, int? pageSize) // Show list appointment of User 
        {
            var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier); //get userId of current user logged in
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.AppointmentService.LIST + userId);
            string jsonData = await response.Content.ReadAsStringAsync();
            List<AppointmentDetailsDto> data = JsonConvert.DeserializeObject<List<AppointmentDetailsDto>>(jsonData);
            ViewBag.UserId = userId;
            var _pageSize = pageSize ?? 8; // Số bản ghi trên 1 trang
            var pageIndex = page ?? 1; // page null thì bằng 1
            var totalPage = data.Count; //Tổng số trang
            var numberPage = Math.Ceiling((float)totalPage / _pageSize); // Số lượng trang
            var startPage = (pageIndex - 1) * _pageSize;
            data = data.OrderBy(x => x.Status).ToList();
            data = data.Skip(startPage).Take(_pageSize).ToList();
            return Json(new
            {
                data = data,
                totalItem = data.Count,
                CurrentPage = pageIndex,
                NumberPage = numberPage,
                PageSize = _pageSize
            });
        }

        public async Task<IActionResult> CancelAppointment(string id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.AppointmentService.CANCEL + id);
            string jsonData = await response.Content.ReadAsStringAsync();
            if(jsonData.Equals("Appointment completed!") || jsonData.Equals("Appointment is in progress!") || jsonData.Equals("Can only cancel the appointment at least two hour!") || jsonData.Equals("This appointment has been canceled before!"))
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



        public async Task<IActionResult> Reschedule(string doctor,DateTime date)
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
            return Json(new { dataSchedules = ScheduleView.ScheduleDoctors, dataShifts = ScheduleView.Shifts });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> RescheduleAppointment(AppointmentRescheduleDto model, string id)
        {
            if(model.StartTime.ToString() == "1/1/0001 12:00:00 AM")
            {
                return Json(new { errorMessage = "No appointment selected!" });
            }else if(model.Status == 4)
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
            }else
            {
                return Json(new { data = jsonData });
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


        public IActionResult BookingSummary(string Time, string userId, int therapistId, string doctorName, DateTime date, string doctor)
        {
            if (Time == null)
            {
                TempData["StatusMessage"] = "No appointment has been selected";
                return RedirectToAction("Booking", "Appointment", new { doctor = doctor, date = date, doctorName = doctorName, Id = therapistId });
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
