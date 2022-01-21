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
using System.Net.Http.Headers;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileSetting([FromForm] PatientUpdateDto model)
        {
            if (ModelState.IsValid)
            {
                model.AccountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                var form = new MultipartFormDataContent();

                //Add data model to multiForm Content     

                form.Add(new StringContent(model.AccountId), "AccountId");
                form.Add(new StringContent(model.Address), "Address");
                form.Add(new StringContent(model.DateOfBirth.ToString()), "DateOfBirth");
                form.Add(new StringContent(model.PostalCode.ToString()), "PostalCode");
                form.Add(new StringContent(model.Phone), "Phone");
                form.Add(new StringContent(model.Languages.ToString()), "Languages");
                form.Add(new StringContent(model.Email), "Email");
                form.Add(new StringContent(model.FullName), "FullName");
                form.Add(new StringContent(model.Gender.ToString()), "Gender");

                //Add file StreamContent to multiForm Content 
                if (model.Avatar != null)
                {
                    var fileContent = new StreamContent(model.Avatar.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.Avatar.ContentType);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Avatar",
                        FileName = model.Avatar.Name,
                        FileNameStar = model.Avatar.Name
                    };
                    form.Add(fileContent);
                }

                //Add file StreamContent to multiForm Content 
                //if (model.Avatar != null)
                //{
                //    content.Add(new StreamContent(model.Avatar.OpenReadStream()), "Avatar", model.Avatar.FileName);
                //}

                //var json = JsonConvert.SerializeObject(model);
                //var data = new StringContent(json, Encoding.UTF8, "application/json");


                var client = _clientFactory.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
                var response = await client.PutAsync(EndpointConstants.AuthService.PATIENTPROFILE, form);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
