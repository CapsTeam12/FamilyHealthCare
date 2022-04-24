using Contract.DTOs;
using Contract.DTOs.AppoimentService;
using Contract.DTOs.ManagementService;
using Contract.DTOs.ScheduleDoctorService;
using Contract.DTOs.ScheduleService;
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
            }else if(response != null && response.IsSuccessStatusCode && jsonData.Equals("You had an appointment at the same time before!"))
            {
                return Json(new { content = jsonData });
            }
            else
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
        public async Task<IActionResult> Booking(BookingViewModel model)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ScheduleService.DOCTOR_SCHEDULES + "/" + model.accountDoctorId);
            string jsonData = await response.Content.ReadAsStringAsync();
            List<ScheduleDoctorDto> data = JsonConvert.DeserializeObject<List<ScheduleDoctorDto>>(jsonData);
            List<Shift> shiftdata = await GetShifts();
            var doctorInfo = await getDoctorDetails(model.accountDoctorId);
            var ScheduleView = new ScheduleViewModel
            {
                Shifts = shiftdata,
                doctorDetails = doctorInfo,
                ScheduleDoctors = data
            };
            ViewBag.doctorEmail = model.doctorEmail;
            ViewBag.doctorId = model.therapistId; // get doctorId
            ViewBag.doctor = model.accountDoctorId; // get userId of doctor
            ViewBag.CurrentDate = model.Date; //get currentDate
            ViewBag.doctorName = model.doctorName; // get doctor fullname
            return View(ScheduleView);
        }

        private async Task<DoctorDetailsDto> getDoctorDetails(string accountDoctorId)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.ManagementService.DOCTORDETAILS}/{accountDoctorId}");
            if (response.IsSuccessStatusCode)
            {
                var doctorInfo = await response.Content.ReadAsAsync<DoctorDetailsDto>();
                return doctorInfo;
            }
            return null;
        }


        public async Task<IActionResult> BookingSummary(string Time, BookingViewModel model)
        {
            if (Time == null)
            {
                TempData["StatusMessage"] = "No appointment has been selected";
                return RedirectToAction("Booking", "Appointment", new { accountDoctorId = model.accountDoctorId, Date = model.Date, doctorName = model.doctorName, therapistId = model.therapistId, doctorEmail = model.doctorEmail });
            }
            var bookingView = new BookingViewModel()
            {
                therapistId = model.therapistId,
                accountDoctorId = model.accountDoctorId,
                doctorName = model.doctorName,
                doctorEmail = model.doctorEmail,
                doctorDetails = await getDoctorDetails(model.accountDoctorId),
                StartTime = Convert.ToDateTime(model.Date.ToString("yyyy-MM-dd") + " " + Time.Split("-")[0]),
                EndTime = Convert.ToDateTime(model.Date.ToString("yyyy-MM-dd") + " " + Time.Split("-")[1]),
                Date = model.Date
            };
            return View(bookingView);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookingSummary(BookingViewModel model)
        {
            var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier); //get userId of current user logged in
            if (String.IsNullOrEmpty(model.Description)) model.Description = "No information";

            var stringModel = JsonConvert.SerializeObject(model);
            var data = new StringContent(stringModel, Encoding.UTF8, "application/json");
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.PostAsync(EndpointConstants.BookingAppointmentService.BOOKING + userId, data);
            if (response.IsSuccessStatusCode && response != null)
            {
                var inforAppointment = await response.Content.ReadAsAsync<AppointmentDetailsDto>();
                var meetingObj = new Meeting()
                {
                    Topic = "Meeting with Dr." + model.doctorName,
                    Agenda = model.Description,
                    StartTime = model.StartTime,
                    Duration = model.EndTime.Minute - model.StartTime.Minute,
                };
                var meetingModel = JsonConvert.SerializeObject(meetingObj);
                var dataMeeting = new StringContent(meetingModel, Encoding.UTF8, "application/json");
                var responseZoomCreate = await client.PostAsync(EndpointConstants.ZoomService.CREATE + model.doctorEmail, dataMeeting);
                if (responseZoomCreate != null && responseZoomCreate.IsSuccessStatusCode)
                {
                    var inforMeeting = await responseZoomCreate.Content.ReadAsAsync<ZoomMeeting>();

                    var scheduleObj = new ScheduleCreateDto()
                    {
                        AppointmentId = inforAppointment.Id,
                        Join_Url = inforMeeting.Join_Url,
                        Start_Url = inforMeeting.Start_Url,
                        MeetingId = inforMeeting.Id
                    };

                    var scheduleModel = JsonConvert.SerializeObject(scheduleObj);
                    var dataSchedule = new StringContent(scheduleModel, Encoding.UTF8, "application/json");
                    var responseSchedule = await client.PostAsync(EndpointConstants.ScheduleService.MEETING_SCHEDULES + model.accountDoctorId + "/" + userId, dataSchedule);
                    if (responseSchedule != null && responseSchedule.IsSuccessStatusCode)
                    {
                        //return RedirectToAction("BookingSuccess", "Appointment", new { doctorName = model.doctorName, StartTime = model.StartTime, EndTime = model.EndTime, date = model.Date });
                        return Json(new { success = true, successUrl = Url.Action("BookingSuccess", "Appointment", new { doctorName = model.doctorName, StartTime = model.StartTime, EndTime = model.EndTime, date = model.Date }) });
                    }
                }
            }
            else
            {
                return Json(new { success = false, message = "You had an appointment at the same time before!" });
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
