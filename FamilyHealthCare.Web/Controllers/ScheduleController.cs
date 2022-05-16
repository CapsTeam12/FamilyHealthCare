using Business.IServices;
using Contract.DTOs.ScheduleDoctorService;
using Contract.DTOs.ScheduleService;
using Data.Entities;
using FamilyHealthCare.SharedLibrary;
using FamilyHealthCare.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class ScheduleController : Controller
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IHttpClientFactory _httpClient;

        public ScheduleController(IHttpContextAccessor httpContext,IHttpClientFactory httpClient)
        {
            _httpContext = httpContext;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Calendar()
        {
            var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier); //get userId of current user logged in
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ScheduleService.CALENDAR + "/" + userId);
            string jsonData = await response.Content.ReadAsStringAsync();
            List<ScheduleDto> data = JsonConvert.DeserializeObject<List<ScheduleDto>>(jsonData);
            ViewBag.UserId = userId;
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(ScheduleCreateDto schedule)
        {
            var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier); //get userId of current user logged in
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            schedule.AccountId = userId;
            var scheduleModel = JsonConvert.SerializeObject(schedule);
            var dataSchedule = new StringContent(scheduleModel, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(EndpointConstants.ScheduleService.EVENT_CREATE, dataSchedule);
            if (response.IsSuccessStatusCode && response != null)
            {
                return Json(new { success = true });
            }
            return View();
        }


    }
}
