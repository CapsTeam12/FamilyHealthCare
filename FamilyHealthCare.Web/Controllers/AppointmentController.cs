using Contract.DTOs;
using Contract.DTOs.AppoimentService;
using Microsoft.AspNetCore.Authentication;
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

namespace FamilyHealthCare.Web.Controllers
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
            client.BaseAddress = new Uri("http://localhost:5020"); //gateway url
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
            client.BaseAddress = new Uri("http://localhost:5020"); //gateway url
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
            client.BaseAddress = new Uri("http://localhost:5020"); //gateway url
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
            client.BaseAddress = new Uri("http://localhost:5020"); //gateway url
            var stringModel = JsonConvert.SerializeObject(model);
            var data = new StringContent(stringModel,Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/Appointment/{id}",data);
            if (response != null && response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index),new { userId= model.UserId });
            }
            return View(model);
        }

    }
}
