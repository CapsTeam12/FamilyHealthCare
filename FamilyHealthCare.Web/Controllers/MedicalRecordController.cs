using Contract.DTOs.MedicalRecordService;
using Contract.DTOs.PrescriptionService;
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
    public class MedicalRecordController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _httpContext;

        public MedicalRecordController(IHttpClientFactory httpClient,IHttpContextAccessor httpContext)
        {
            _httpClient = httpClient;
            _httpContext = httpContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await client.GetAsync($"{EndpointConstants.MedicalRecordService.LIST_BY_DOCTOR}/{accountId}");;
            if (response.IsSuccessStatusCode)
            {
                var medicalRecords = await response.Content.ReadAsAsync<IEnumerable<MedicalRecordDto>>();
                return Json(new {success= true,item = medicalRecords });
            }
            return Json(new {success = false });
        }

        [HttpGet]
        public async Task<IActionResult> GetMedicalRecord(int id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.PrescriptionService.DETAILS_BY_MEDICAL}/{id}");
            if (response != null && response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<PrescriptionDto>();
                return Json(new { success = true, item = model });
            }
            return Json(new { success = false });
        }
    }
}
