using Contract.DTOs.ManagementService;
using Contract.DTOs.MedicalRecordService;
using FamilyHealthCare.Doctor.Models;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyHealthCare.Doctor.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _httpContext;

        public HomeController(ILogger<HomeController> logger,IHttpClientFactory httpClient, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpContext = httpContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.AccountId = accountId;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> getPatients()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ManagementService.GET_PATIENTS);
            if (response.IsSuccessStatusCode)
            {
                var patients = await response.Content.ReadAsAsync<int>();
                return Json(new { success = true, item = patients});
            }
            return Json(new {success = false });
        }

        [HttpGet]
        public async Task<IActionResult> getMedicalRecords()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await client.GetAsync($"{EndpointConstants.ManagementService.GET_MEDICALS_DOCTOR}/{accountId}");
            if (response.IsSuccessStatusCode)
            {
                var medicalRecords = await response.Content.ReadAsAsync<int>();
                return Json(new {success = true,item = medicalRecords });
            }
            return Json(new {success = false });
        }

        [HttpGet]
        public async Task<IActionResult> getAppointments()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await client.GetAsync($"{EndpointConstants.AppointmentService.GET_APPOINTMENTS_DOCTOR}/{accountId}");
            if (response.IsSuccessStatusCode)
            {
                var appointments = await response.Content.ReadAsAsync<int>();
                return Json(new { success = true, item = appointments });
            }
            return Json(new { success = false });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
