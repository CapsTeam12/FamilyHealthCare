using Contract.DTOs;
using FamilyHealthCare.SharedLibrary;
using FamilyHealthCare.WebAdmin.Models;
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

namespace FamilyHealthCare.WebAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _httpContext;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClient, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpContext = httpContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ManagementService.GET_PATIENTS);
            if (response.IsSuccessStatusCode)
            {
                var patients = await response.Content.ReadAsAsync<int>();
                return Json(new { success = true, item = patients });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ManagementService.GET_DOCTORS);
            if (response.IsSuccessStatusCode)
            {
                var doctors = await response.Content.ReadAsAsync<int>();
                return Json(new { success = true, item = doctors });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> GetPharmacies()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ManagementService.GET_PHARMACIES);
            if (response.IsSuccessStatusCode)
            {
                var pharmacies = await response.Content.ReadAsAsync<int>();
                return Json(new { success = true, item = pharmacies });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> getAppointments()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await client.GetAsync($"{EndpointConstants.AppointmentService.GET_APPOINTMENTS}");
            if (response.IsSuccessStatusCode)
            {
                var appointments = await response.Content.ReadAsAsync<IEnumerable<AppointmentDetailsDto>>();
                return Json(new { success = true, item = appointments });
            }
            return Json(new { success = false });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
