using Contract.DTOs;
using Data;
using Data.Entities;
using FHC.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace FHC.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContext;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClient, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpContext = httpContext;
        }

        
        public async Task<IActionResult> Index()
        {                   
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri("http://localhost:5020"); //gateway url
            var response = await client.GetAsync("/Appointment");
            string jsonData = await response.Content.ReadAsStringAsync();
            List<AppointmentDetailsDto> data = JsonConvert.DeserializeObject<List<AppointmentDetailsDto>>(jsonData);
            ViewBag.UserId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(data);
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


        [Authorize]
        public async Task<IActionResult> Login()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}
