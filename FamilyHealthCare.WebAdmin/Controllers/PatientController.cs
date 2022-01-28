using Contract.DTOs.ManagementService;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public IActionResult DetailsPatient()
        {
            return View();
        }
    }
}
