using Contract.DTOs.HealthCheck;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Controllers
{
    [Authorize]
    public class HealthCheckController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _httpContext;

        public HealthCheckController(IHttpClientFactory httpClient, IHttpContextAccessor httpContext)
        {
            _httpClient = httpClient;
            _httpContext = httpContext;
        }

        [HttpPost]
        public async Task<HealthCheckDto> HealthCheck(HealthCheckDto healthCheckDto)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            healthCheckDto.UserId = accountId;
            var stringModel = JsonConvert.SerializeObject(healthCheckDto);
            var stringContent = new StringContent(stringModel, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{EndpointConstants.HealthCheckService.HEALTHCHECK}", stringContent);
           // if (response.IsSuccessStatusCode)
            //{
                var healthChecks = await response.Content.ReadAsAsync<HealthCheckDto>();
                return healthChecks;
            //}
            //return Json(new { success = false });   `   
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
