using Contract.DTOs.ManagementService;
using Data.Entities;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHealthCare.WebAdmin.Controllers
{
    public class PatientController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContext;
        public PatientController(ILogger<HomeController> logger, IHttpClientFactory clientFactory, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _httpContext = httpContext;
        }
        public async Task<IActionResult> Index()
        {
            var httpClient = _clientFactory.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await httpClient.GetAsync(EndpointConstants.ManagementService.PATIENTS);
            var data = new List<PatientDetailsDto>();

            if (response.IsSuccessStatusCode)
            {
                data= await response.Content.ReadAsAsync<List<PatientDetailsDto>>();
            }
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsPatient(string id)
        {
            //var id = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            //var id = patient;
            var httpClient = _clientFactory.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await httpClient.GetAsync($"{EndpointConstants.ManagementService.PATIENTDETAILS}/{id}");
            var model = new PatientDetailsDto();
            if (response.IsSuccessStatusCode)
            {
                model = await response.Content.ReadAsAsync<PatientDetailsDto>();
            }
            return View(model);
        }

        [HttpPut]
        public async Task<IActionResult> ActiveAndDeActivateAccount(string accountId,bool isActive)
        {
            var httpClient = _clientFactory.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var content = new StringContent(accountId, Encoding.UTF8, "application/json");
            if(isActive == true)
            {
                var response = await httpClient.PutAsync($"{EndpointConstants.ManagementService.DEACTIVATE_PATIENT}/{accountId}",content);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new {IsActive = false });

                }
            }
            else
            {
                var response = await httpClient.PutAsync($"{EndpointConstants.ManagementService.ACTIVE_PATIENT}/{accountId}",content);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { IsActive = true });
                }
            }
            return View();
        }
    }
}
