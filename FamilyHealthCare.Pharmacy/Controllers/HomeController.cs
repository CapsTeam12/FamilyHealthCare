using Contract.DTOs.MedicineService;
using Contract.DTOs.PrescriptionService;
using FamilyHealthCare.Pharmacy.Models;
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

namespace FamilyHealthCare.Pharmacy.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> getMedicines()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await client.GetAsync($"{EndpointConstants.MedicineService.MEDICINES_PHARMACY}/{accountId}");
            if (response.IsSuccessStatusCode)
            {
                var medicines = await response.Content.ReadAsAsync<IEnumerable<MedicineDto>>();
                return Json(new { success = true, item = medicines });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> getPrescriptions()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await client.GetAsync($"{EndpointConstants.PrescriptionService.LIST_BY_PHARMACY}/{accountId}");
            if (response.IsSuccessStatusCode)
            {
                var prescriptions = await response.Content.ReadAsAsync<IEnumerable<PrescriptionDto>>();
                return Json(new { success = true, item = prescriptions });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> getPrescriptionsAndMedicines()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var responsePrescription = await client.GetAsync($"{EndpointConstants.PrescriptionService.LIST_BY_PHARMACY}/{accountId}");
            var responseMedicine = await client.GetAsync($"{EndpointConstants.MedicineService.MEDICINES_PHARMACY}/{accountId}");
            if (responsePrescription.IsSuccessStatusCode && responseMedicine.IsSuccessStatusCode)
            {
                var prescriptions = await responsePrescription.Content.ReadAsAsync<IEnumerable<PrescriptionDto>>();
                var medicines = await responseMedicine.Content.ReadAsAsync<IEnumerable<MedicineDto>>();
                return Json(new { success = true, prescriptions = prescriptions,medicines = medicines });
            }
            return Json(new { success = false });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Setting()
        {
            return View();
        }
    }
}
