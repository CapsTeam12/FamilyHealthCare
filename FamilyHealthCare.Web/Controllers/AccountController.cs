using Contract.DTOs.AuthService;
using Contract.DTOs.ManagementService;
using FamilyHealthCare.Customer.Models;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<AppointmentController> _logger;
        private readonly IHttpContextAccessor _httpContext;
        public AccountController(ILogger<AppointmentController> logger, IHttpClientFactory clientFactory, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _httpContext = httpContext;
        }
        public IActionResult SignIn()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectDefaults.AuthenticationScheme);
        }

        public new IActionResult SignOut()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if(ModelState.IsValid)
            {
                model.UserId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                var json = JsonConvert.SerializeObject(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _clientFactory.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
                var response = await client.PutAsync(EndpointConstants.AuthService.CHANGEPASSWORD, data);
                if (response.StatusCode.ToString() == "NoContent")
                    ModelState.AddModelError("CurrentPassword", "Invalid Password");
                else if(response.StatusCode.ToString() == "NotFound")
                    ModelState.AddModelError("NewPassword", "Password must differ from old password");
                else if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ProfileSetting()
        {
            var id = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var httpClient = _clientFactory.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await httpClient.GetAsync($"{EndpointConstants.ManagementService.PATIENTDETAILS}/{id}");
            var model = new PatientDetailsDto();
            if (response.IsSuccessStatusCode)
            {
                model = await response.Content.ReadAsAsync<PatientDetailsDto>();
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProfileSetting(PatientDetailsDto model)
        {
            if (ModelState.IsValid)
            {
                model.AccountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                var json = JsonConvert.SerializeObject(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _clientFactory.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
                var response = await client.PutAsync(EndpointConstants.AuthService.PATIENTPROFILE, data);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
