using Contract.DTOs.ManagementService;
using Contract.DTOs.PrescriptionService;
using FamilyHealthCare.Pharmacy.Models;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyHealthCare.Pharmacy.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _httpContext;

        public PrescriptionController(IHttpClientFactory httpClient,IHttpContextAccessor httpContext)
        {
            _httpClient = httpClient;
            _httpContext = httpContext;
        }

        private async Task<PharmacyDetailsDto> GetPharmacyDetails()
        {
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.ManagementService.PHARMACYDETAILS}/{accountId}");
            var pharmacy = await response.Content.ReadAsAsync<PharmacyDetailsDto>();
            return pharmacy;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.PrescriptionService.LIST_BY_PHARMACY}/{accountId}");
            var prescriptionViewModel = new PrescriptionViewModel();
            prescriptionViewModel.PharmacyDetails = await GetPharmacyDetails();
            if(response.IsSuccessStatusCode && response != null)
            {
                var prescriptionDto = await response.Content.ReadAsAsync<IEnumerable<PrescriptionDto>>();
                prescriptionViewModel.Prescriptions = prescriptionDto;
                return View(prescriptionViewModel);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.PrescriptionService.DETAILS}/{id}");
            if (response != null && response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<PrescriptionDto>();
                return Json(new { success = true, item = model });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrescription(AddUpdatePrescriptionPharmacyDto PrescriptionDto)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var FormContent = new MultipartFormDataContent();
            if (PrescriptionDto.Signature != null)
            {
                var fileContent = new StreamContent(PrescriptionDto.Signature.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(PrescriptionDto.Signature.ContentType);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "Signature",
                    FileName = PrescriptionDto.Signature.Name,
                    FileNameStar = PrescriptionDto.Signature.Name
                };
                FormContent.Add(fileContent);
            }
            FormContent.Add(new StringContent(PrescriptionDto.PrescriptionName), "PrescriptionName");
            FormContent.Add(new StringContent(PrescriptionDto.Date.ToString()), "Date");
            FormContent.Add(new StringContent(PrescriptionDto.Notes), "Notes");
            FormContent.Add(new StringContent(PrescriptionDto.PharmacyId.ToString()), "PharmacyId");
            for (int i = 0; i < PrescriptionDto.PrescriptionDetailsDtos.Count; i++)
            {
                var obj = PrescriptionDto.PrescriptionDetailsDtos[i];
                FormContent.Add(new StringContent(obj.MedicineName), "PrescriptionDetailsDtos[" + i + "].MedicineName");
                FormContent.Add(new StringContent(obj.Quantity.ToString()), "PrescriptionDetailsDtos[" + i + "].Quantity");
                FormContent.Add(new StringContent(obj.Days.ToString()), "PrescriptionDetailsDtos[" + i + "].Days");
                for (int j = 0; j < obj.Time.Length; j++)
                {
                    FormContent.Add(new StringContent(obj.Time[j].ToString()), "PrescriptionDetailsDtos[" + i + "].Time");
                }
            }
            var responsePrescription = await client.PostAsync(EndpointConstants.PrescriptionService.CREATE_BY_PHARMACY, FormContent);
            if(responsePrescription != null && responsePrescription.IsSuccessStatusCode)
            {
                var prescriptionDto = await responsePrescription.Content.ReadAsAsync<PrescriptionDto>();
                return Json(new { success = true, item = prescriptionDto });
            }
            return Json(new { success = false });
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePrescription(AddUpdatePrescriptionPharmacyDto PrescriptionDto)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var FormContent = new MultipartFormDataContent();
            if (PrescriptionDto.Signature != null)
            {
                var fileContent = new StreamContent(PrescriptionDto.Signature.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(PrescriptionDto.Signature.ContentType);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "Signature",
                    FileName = PrescriptionDto.Signature.Name,
                    FileNameStar = PrescriptionDto.Signature.Name
                };
                FormContent.Add(fileContent);
            }
            FormContent.Add(new StringContent(PrescriptionDto.PrescriptionName), "PrescriptionName");
            FormContent.Add(new StringContent(PrescriptionDto.Date.ToString()), "Date");
            FormContent.Add(new StringContent(PrescriptionDto.Notes), "Notes");
            FormContent.Add(new StringContent(PrescriptionDto.PharmacyId.ToString()), "PharmacyId");
            FormContent.Add(new StringContent(PrescriptionDto.Id.ToString()), "Id");
            for (int i = 0; i < PrescriptionDto.PrescriptionDetailsDtos.Count; i++)
            {
                var obj = PrescriptionDto.PrescriptionDetailsDtos[i];
                FormContent.Add(new StringContent(obj.MedicineName), "PrescriptionDetailsDtos[" + i + "].MedicineName");
                FormContent.Add(new StringContent(obj.Quantity.ToString()), "PrescriptionDetailsDtos[" + i + "].Quantity");
                FormContent.Add(new StringContent(obj.Days.ToString()), "PrescriptionDetailsDtos[" + i + "].Days");
                for (int j = 0; j < obj.Time.Length; j++)
                {
                    FormContent.Add(new StringContent(obj.Time[j].ToString()), "PrescriptionDetailsDtos[" + i + "].Time");
                }
            }
            var responsePrescription = await client.PutAsync($"{EndpointConstants.PrescriptionService.UPDATE_BY_PHARMACY}/{PrescriptionDto.Id}", FormContent);
            if (responsePrescription != null && responsePrescription.IsSuccessStatusCode)
            {
                var prescriptionDto = await responsePrescription.Content.ReadAsAsync<PrescriptionDto>();
                return Json(new { success = true, item = prescriptionDto });
            }
            return Json(new { success = false });
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.DeleteAsync($"{EndpointConstants.PrescriptionService.DELETE}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
