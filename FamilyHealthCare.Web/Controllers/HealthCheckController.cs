using Contract.DTOs.HealthCheck;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Controllers
{
    public class HealthCheckController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _httpContext;

        public HealthCheckController(IHttpClientFactory httpClient, IHttpContextAccessor httpContext)
        {
            _httpClient = httpClient;
            _httpContext = httpContext;
        }

        [HttpGet]
        public IActionResult HealthCheck()
        {
            //var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            //var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            //var response = await client.GetAsync($"{EndpointConstants.HealthCheckService.HEALTHCHECKLIST}/{accountId}"); ;
            //if (response.IsSuccessStatusCode)
            //{
            //    var healthChecks = await response.Content.ReadAsAsync<List<HealthCheckDto>>();
            //    return Json(new { success = true, item = healthChecks });
            //}
            //return Json(new { success = false });
            return View();
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
