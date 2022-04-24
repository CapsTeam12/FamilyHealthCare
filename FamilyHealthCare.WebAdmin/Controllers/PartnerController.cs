using Contract.DTOs.ManagementService;
using Contract.DTOs.PartnerService;
using Data.Entities;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHealthCare.WebAdmin.Controllers
{
    public class PartnerController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _httpContext;
        public PartnerController(IHttpClientFactory httpClient, IHttpContextAccessor httpContext)
        {
            _httpClient = httpClient;
            _httpContext = httpContext;
        }

        [HttpGet]
        public async Task<IActionResult> DoctorRequestList()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ManagementService.DOCTORS_REQUESTS);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<List<DoctorRequestDetailsDto>>();
                return View(data);
            }
            return NotFound();

        }

        [HttpGet]
        public async Task<IActionResult> PharmacyRequestList()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ManagementService.PHARMACIES_REQUESTS);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<List<PharmacyRequestDetailsDto>>();
                return View(data);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> AcceptDoctorRequest(int doctorId)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.ManagementService.ACCEPT_DOCTORS_REQUESTS}/{doctorId}");
            if (response.IsSuccessStatusCode)
            {
                var dataDoctorReq = await response.Content.ReadAsAsync<DoctorDetailsDto>();
                var userZoom = new UserZoomDetail()
                {
                    Email = dataDoctorReq.Email,
                    First_name = dataDoctorReq.FullName,
                    Type = 1
                };
                var dataJsonString = JsonConvert.SerializeObject(userZoom);
                var stringContent = new StringContent(dataJsonString, Encoding.UTF8, "application/json");
                var responseZoom = await client.PostAsync($"{EndpointConstants.ZoomService.CREATE_USER}", stringContent);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> AcceptPharmacyRequest(int pharmacyId)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.ManagementService.ACCEPT_PHARMACIES_REQUESTS}/{pharmacyId}");
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> DenyDoctorRequest(int doctorId)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.ManagementService.DENY_DOCTORS_REQUESTS}/{doctorId}");
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> DenyPharmacyRequest(int pharmacyId)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.ManagementService.DENY_PHARMACIES_REQUESTS}/{pharmacyId}");
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> GetDetailsDoctorRequest(int doctorId)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.ManagementService.DETAILS_DOCTORS_REQUESTS}/{doctorId}");
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> GetDetailsPharmacyRequest(int pharmacyId)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.ManagementService.DETAILS_PHARMACIES_REQUESTS}/{pharmacyId}");
            var pharmacyDetails = await response.Content.ReadAsAsync<PharmacyDetailsDto>();
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, item = pharmacyDetails });
            }
            return Json(new { success = false });
        }

      

    }
}
