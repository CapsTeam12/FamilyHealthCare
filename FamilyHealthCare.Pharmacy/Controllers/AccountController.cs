using Contract.DTOs.AuthService;
using Contract.DTOs.ManagementService;
using FamilyHealthCare.Pharmacy.Models;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHealthCare.Pharmacy.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContext;

        public AccountController(IHttpClientFactory clientFactory, IHttpContextAccessor httpContext)
        {
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
            if (ModelState.IsValid)
            {
                model.UserId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                var json = JsonConvert.SerializeObject(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _clientFactory.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
                var response = await client.PutAsync(EndpointConstants.AuthService.CHANGEPASSWORD, data);
                if (response.StatusCode.ToString() == "NoContent")
                    ModelState.AddModelError("CurrentPassword", "Invalid Password");
                else if (response.StatusCode.ToString() == "NotFound")
                    ModelState.AddModelError("NewPassword", "Password must differ from old password");
                else if (response.StatusCode.ToString() == "BadRequest")
                    ModelState.AddModelError("NewPassword", "Password requires at least 8 characters including a number, a lowercase and a uppercase letter");
                else if (response.IsSuccessStatusCode)
                {
                    ViewBag.Success = "Successfully!";
                    //return RedirectToAction("Index", "Home");
                }

            }
            return View();
        }

        private async Task<ProfileViewModel> GetPharmacyDetails()
        {
            var id = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var httpClient = _clientFactory.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await httpClient.GetAsync($"{EndpointConstants.ManagementService.PHARMACYDETAILS}/{id}");
            var model = new ProfileViewModel();
            if (response.IsSuccessStatusCode)
            {
                model.PharmacyDetails = await response.Content.ReadAsAsync<PharmacyDetailsDto>();
            }
            return model;
        }

        [HttpGet]
        public async Task<IActionResult> ProfileSetting()
        {
            var model = await GetPharmacyDetails();
            ViewBag.Pharmacy = model.PharmacyDetails;
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileSetting(ChangePasswordDto model)
        {
            var pharmacy = await GetPharmacyDetails();
            ViewBag.Pharmacy = pharmacy.PharmacyDetails;            
            if (ModelState.IsValid)
            {
                model.UserId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                var json = JsonConvert.SerializeObject(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _clientFactory.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
                var response = await client.PutAsync(EndpointConstants.AuthService.CHANGEPASSWORD, data);
                if (response.StatusCode.ToString() == "NoContent")
                    ModelState.AddModelError("CurrentPassword", "Invalid Password");
                else if (response.StatusCode.ToString() == "NotFound")
                    ModelState.AddModelError("NewPassword", "Password must differ from old password");
                else if (response.StatusCode.ToString() == "BadRequest")
                    ModelState.AddModelError("NewPassword", "Password requires at least 8 characters including a number, a lowercase and a uppercase letter");
                else if (response.IsSuccessStatusCode)
                {
                    ViewBag.Success = "Successfully!";
                    //return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.PasswordTab = "password_tab";
            return View();
        }

    }
}
