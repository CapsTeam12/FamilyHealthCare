using Contract.DTOs.ScheduleDoctorService;
using Contract.DTOs.ScheduleService;
using Data.Entities;
using FamilyHealthCare.Doctor.Models;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHealthCare.Doctor.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<ScheduleController> _logger;
        private readonly IHttpContextAccessor _httpContext;
        public ScheduleController(ILogger<ScheduleController> logger, IHttpClientFactory httpClient, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpContext = httpContext;
        }

        public async Task<IActionResult> Calendar(string userId)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ScheduleService.CALENDAR + "/" + userId);
            string jsonData = await response.Content.ReadAsStringAsync();
            List<ScheduleDto> data = JsonConvert.DeserializeObject<List<ScheduleDto>>(jsonData);
            ViewBag.UserId = userId;
            return View(data);
        }

        public async Task<List<Shift>> GetShifts()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ScheduleService.SHIFTS);
            string jsonData = await response.Content.ReadAsStringAsync();
            List<Shift> data = JsonConvert.DeserializeObject<List<Shift>>(jsonData);
            return data;
        }

        public async Task<IActionResult> AvailableTimings(string userId, DateTime date)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ScheduleService.DOCTOR_SCHEDULES + "/" + userId + "/" + date.ToString("yyyy-MM-dd"));
            string jsonData = await response.Content.ReadAsStringAsync();
            List<ScheduleDoctorDto> data = JsonConvert.DeserializeObject<List<ScheduleDoctorDto>>(jsonData);
            List<Shift> shiftdata = await GetShifts();
            var ScheduleView = new ScheduleViewModel
            {
                Shifts = shiftdata,
                ScheduleDoctors = data
            };
            ViewBag.userId = userId;
            ViewBag.CurrentDate = date.ToString("yyyy-MM-dd");
            return View(ScheduleView);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateScheduleDoctor(ScheduleDoctorCreateDto model)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var stringModel = JsonConvert.SerializeObject(model);
            var data = new StringContent(stringModel, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(EndpointConstants.ScheduleService.DOCTOR_SCHEDULES, data);
            if (response != null && response.IsSuccessStatusCode)
            {
                return RedirectToAction("AvailableTimings", "Schedule", new { userId = model.AccountId, date = model.Date });
            }
            return View(model);

        }
    }
}
