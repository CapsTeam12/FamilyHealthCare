using Business.IServices;
using Contract.DTOs.ScheduleDoctorService;
using Contract.DTOs.ScheduleService;
using Data.Entities;
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
using System.Text;
using System.Threading.Tasks;

namespace FamilyHealthCare.Web.Controllers
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

        public async Task<IActionResult> Calendar(string userId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri("http://localhost:5020"); //gateway url
            var response = await client.GetAsync($"/Schedule/{userId}");
            string jsonData = await response.Content.ReadAsStringAsync();
            List<ScheduleDto> data = JsonConvert.DeserializeObject<List<ScheduleDto>>(jsonData);
            ViewBag.UserId = userId;
            return View(data);
        }

        public async Task<List<Shift>> GetShifts()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri("http://localhost:5020"); //gateway url
            var response = await client.GetAsync("/Schedule/Shifts");
            string jsonData = await response.Content.ReadAsStringAsync();
            List<Shift> data = JsonConvert.DeserializeObject<List<Shift>>(jsonData);
            return data;
        }

        public async Task<IActionResult> AvailableTimings(string userId,DateTime date)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri("http://localhost:5020"); //gateway url
            var response = await client.GetAsync($"/Schedule/Doctor/{userId}/{date.ToString("yyyy-MM-dd")}");
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
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri("http://localhost:5020"); //gateway url
            var stringModel = JsonConvert.SerializeObject(model);
            var data = new StringContent(stringModel, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/Schedule/Doctor",data);
            if (response != null && response.IsSuccessStatusCode)
            {
                return RedirectToAction("AvailableTimings", "Schedule",new { userId = model.UserId, date = model.Date});
            }
            return View(model);

        }

    }
}
