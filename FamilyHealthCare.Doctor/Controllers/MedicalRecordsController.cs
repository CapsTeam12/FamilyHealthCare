using Contract.DTOs.ManagementService;
using Contract.DTOs.MedicalRecordService;
using Contract.DTOs.MedicineService;
using Contract.DTOs.PrescriptionService;
using FamilyHealthCare.Doctor.Models;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateMedicalRecord(MedicalPrescriptionViewModel medicalDto)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var jsonContent = JsonConvert.SerializeObject(medicalDto.medicalRecordDto);
            var stringContent = new StringContent(jsonContent,Encoding.UTF8,"application/json");
            var response = await client.PostAsync(EndpointConstants.MedicalRecordService.CREATE,stringContent);
            if (response.IsSuccessStatusCode)
            {
                var medicalRecord = await response.Content.ReadAsAsync<MedicalRecordDto>();
                var FormContent = new MultipartFormDataContent();
                if(medicalDto.PrescriptionDto.Signature != null)
                {
                    var fileContent = new StreamContent(medicalDto.PrescriptionDto.Signature.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(medicalDto.PrescriptionDto.Signature.ContentType);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Signature",
                        FileName = medicalDto.PrescriptionDto.Signature.Name,
                        FileNameStar = medicalDto.PrescriptionDto.Signature.Name
                    };
                    FormContent.Add(fileContent);
                }
                FormContent.Add(new StringContent(medicalDto.PrescriptionDto.PrescriptionName), "PrescriptionName");
                FormContent.Add(new StringContent(medicalDto.PrescriptionDto.Date.ToString()), "Date");
                FormContent.Add(new StringContent(medicalDto.PrescriptionDto.Notes), "Notes");
                FormContent.Add(new StringContent(medicalDto.PrescriptionDto.PatientId.ToString()), "PatientId");
                FormContent.Add(new StringContent(medicalDto.PrescriptionDto.DoctorId.ToString()), "DoctorId");
                FormContent.Add(new StringContent(medicalRecord.Id.ToString()), "MedicalRecordId");
                for(int i=0;i< medicalDto.PrescriptionDto.PrescriptionDetailsDtos.Count; i++)
                {
                    var obj = medicalDto.PrescriptionDto.PrescriptionDetailsDtos[i];
                    FormContent.Add(new StringContent(obj.MedicineName), "PrescriptionDetailsDtos[" + i+"].MedicineName");
                    FormContent.Add(new StringContent(obj.Quantity.ToString()), "PrescriptionDetailsDtos[" + i + "].Quantity");
                    FormContent.Add(new StringContent(obj.Days.ToString()), "PrescriptionDetailsDtos[" + i + "].Days");
                    for(int j = 0; j< obj.Time.Length; j++)
                    {
                        FormContent.Add(new StringContent(obj.Time[j].ToString()), "PrescriptionDetailsDtos[" + i + "].Time");
                    }                   
                }
                var responsePrescription = await client.PostAsync(EndpointConstants.PrescriptionService.CREATE,FormContent);
                return Json(new { success = true, item = medicalRecord });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> GetMedicalRecord(int id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.PrescriptionService.DETAILS}/{id}");
            if(response != null && response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<PrescriptionDto>();
                return Json(new { success = true,item = model });
            }
            return Json(new { success = false });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMedicalRecord(MedicalPrescriptionViewModel medicalDto)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var jsonContent = JsonConvert.SerializeObject(medicalDto.medicalRecordDto);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{EndpointConstants.MedicalRecordService.UPDATE}/{medicalDto.medicalRecordDto.Id}",stringContent);
            if (response.IsSuccessStatusCode)
            {
                var medicalRecord = await response.Content.ReadAsAsync<MedicalRecordDto>();
                var FormContent = new MultipartFormDataContent();
                if (medicalDto.PrescriptionDto.Signature != null)
                {
                    var fileContent = new StreamContent(medicalDto.PrescriptionDto.Signature.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(medicalDto.PrescriptionDto.Signature.ContentType);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Signature",
                        FileName = medicalDto.PrescriptionDto.Signature.Name,
                        FileNameStar = medicalDto.PrescriptionDto.Signature.Name
                    };
                    FormContent.Add(fileContent);
                }
                FormContent.Add(new StringContent(medicalDto.PrescriptionDto.PrescriptionName), "PrescriptionName");
                FormContent.Add(new StringContent(medicalDto.PrescriptionDto.Date.ToString()), "Date");
                FormContent.Add(new StringContent(medicalDto.PrescriptionDto.Notes), "Notes");
                FormContent.Add(new StringContent(medicalDto.PrescriptionDto.PatientId.ToString()), "PatientId");
                FormContent.Add(new StringContent(medicalDto.PrescriptionDto.DoctorId.ToString()), "DoctorId");
                FormContent.Add(new StringContent(medicalRecord.Id.ToString()), "MedicalRecordId");
                for (int i = 0; i < medicalDto.PrescriptionDto.PrescriptionDetailsDtos.Count; i++)
                {
                    var obj = medicalDto.PrescriptionDto.PrescriptionDetailsDtos[i];
                    FormContent.Add(new StringContent(obj.MedicineName), "PrescriptionDetailsDtos[" + i + "].MedicineName");
                    FormContent.Add(new StringContent(obj.Quantity.ToString()), "PrescriptionDetailsDtos[" + i + "].Quantity");
                    FormContent.Add(new StringContent(obj.Days.ToString()), "PrescriptionDetailsDtos[" + i + "].Days");
                    for (int j = 0; j < obj.Time.Length; j++)
                    {
                        FormContent.Add(new StringContent(obj.Time[j].ToString()), "PrescriptionDetailsDtos[" + i + "].Time");
                    }
                }
                var responsePrescription = await client.PutAsync($"{EndpointConstants.PrescriptionService.UPDATE}/{medicalDto.medicalRecordDto.Id}", FormContent);
                return Json(new { success = true, item = medicalRecord });
            }
            return Json(new { success = false });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var responsePrescription = await client.DeleteAsync($"{EndpointConstants.PrescriptionService.DELETE}/{id}");
            var response = await client.DeleteAsync($"{EndpointConstants.MedicalRecordService.DELETE}/{id}");
            if(response != null && response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
