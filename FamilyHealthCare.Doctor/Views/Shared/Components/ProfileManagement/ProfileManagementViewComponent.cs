using Contract.DTOs.ManagementService;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyHealthCare.Doctor.Views.Shared.Components.ProfileManagement
{
    public class ProfileManagementViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContext;

        public ProfileManagementViewComponent(IHttpClientFactory clientFactory, IHttpContextAccessor httpContext)
        {
            _clientFactory = clientFactory;
            _httpContext = httpContext;
        }

        public async Task<DoctorDetailsDto> GetProfileAsync()
        {
            var id = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var httpClient = _clientFactory.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await httpClient.GetAsync($"{EndpointConstants.ManagementService.DOCTORDETAILS}/{id}");
            var model = new DoctorDetailsDto();
            if (response.IsSuccessStatusCode)
            {
                model = await response.Content.ReadAsAsync<DoctorDetailsDto>();
            }
            return model;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var profile = await GetProfileAsync();
            return View("ProfileManagement", profile);
        }
    }
}
