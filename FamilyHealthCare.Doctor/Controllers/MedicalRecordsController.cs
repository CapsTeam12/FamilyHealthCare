using Contract.DTOs.ManagementService;
using Contract.DTOs.MedicalRecordService;
using FamilyHealthCare.Doctor.Models;
using FamilyHealthCare.SharedLibrary;
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
    public class MedicalRecordsController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _httpContext;
        public MedicalRecordsController(IHttpClientFactory httpClient,IHttpContextAccessor httpContext)
        {
            _httpClient = httpClient;
            _httpContext = httpContext;
        }

        private async Task<IEnumerable<PatientDetailsDto>> getPatients()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ManagementService.PATIENTDETAILS);
            if (response.IsSuccessStatusCode)
            {
                var patients = await response.Content.ReadAsAsync<IEnumerable<PatientDetailsDto>>();
                return patients;
            }
            return null;
        }

        private async Task<DoctorDetailsDto> getDoctorProfile()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await client.GetAsync($"{EndpointConstants.ManagementService.DOCTORDETAILS}/{accountId}");
            if (response.IsSuccessStatusCode)
            {
                var doctorInfo = await response.Content.ReadAsAsync<DoctorDetailsDto>();
                return doctorInfo;
            }
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> MedicalRecord()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await client.GetAsync($"{EndpointConstants.MedicalRecordService.LIST_BY_DOCTOR}/{accountId}");
            var medicalRecordVM = new MedicalRecordViewModel();
            medicalRecordVM.doctorDetails = await getDoctorProfile();
            medicalRecordVM.Patients = await getPatients();
            if (response.IsSuccessStatusCode)
            {
                var medicalRecords = await response.Content.ReadAsAsync<IEnumerable<MedicalRecordDto>>();

                medicalRecordVM.MedicalRecords = medicalRecords;
                return View(medicalRecordVM);
            }
            return View(medicalRecordVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedicalRecord(AddUpdateMedicalRecordDto medicalDto)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var jsonContent = JsonConvert.SerializeObject(medicalDto);
            var stringContent = new StringContent(jsonContent,Encoding.UTF8,"application/json");
            var response = await client.PostAsync(EndpointConstants.MedicalRecordService.CREATE,stringContent);
            if (response.IsSuccessStatusCode)
            {
                var medicalRecord = await response.Content.ReadAsAsync<MedicalRecordDto>();
                return Json(new { success = true, item = medicalRecord });
            }
            return Json(new { success = false });
        }


        public IActionResult MedicalRecordDetails()
        {
            return View();
        }
    }
}
