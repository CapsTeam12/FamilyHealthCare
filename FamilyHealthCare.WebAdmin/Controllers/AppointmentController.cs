using Contract.DTOs;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FamilyHealthCare.WebAdmin.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _httpContext;

        public AppointmentController(IHttpClientFactory httpClient,IHttpContextAccessor httpContext)
        {
            _httpClient = httpClient;
            _httpContext = httpContext;
        }
        public async Task<IActionResult> Index()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.AppointmentService.GET_APPOINTMENTS);
            if (response.IsSuccessStatusCode)
            {
                var appointments = await response.Content.ReadAsAsync<IEnumerable<AppointmentDetailsDto>>();
                appointments = appointments.OrderBy(x => x.Status);
                return View(appointments);
            }
            return NotFound();
        }
    }
}
