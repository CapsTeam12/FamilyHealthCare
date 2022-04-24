using Contract.DTOs.ManagementService;
using Contract.DTOs.MedicineService;
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
    public class MedicineController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _httpContext;

        public MedicineController(IHttpClientFactory httpClient, IHttpContextAccessor httpContext)
        {
            _httpClient = httpClient;
            _httpContext = httpContext;
        }

        private async Task<List<CategoriesDetailsDto>> GetCategories()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ManagementService.CATEGORIES);
            var categories = await response.Content.ReadAsAsync<List<CategoriesDetailsDto>>();
            return categories;
        }

        [HttpGet]
        public async Task<IActionResult> GetMedicinesByPharmacy()
        {
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.MedicineService.MEDICINES_PHARMACY}/{accountId}");
            if (response.IsSuccessStatusCode)
            {
                var medicines = await response.Content.ReadAsAsync<IEnumerable<MedicineDto>>();
                var medicineViewModel = new MedicineViewModel()
                {
                    Categories = await GetCategories(),
                    Medicines = medicines
                };
                return View("Medicines",medicineViewModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedicine(AddUpdateMedicineDto medicineDto)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            if (ModelState.IsValid)
            {
                var FormContent = new MultipartFormDataContent();
                if (medicineDto.Images != null)
                {
                    var fileContent = new StreamContent(medicineDto.Images.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(medicineDto.Images.ContentType);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Images",
                        FileName = medicineDto.Images.Name,
                        FileNameStar = medicineDto.Images.Name
                    };
                    FormContent.Add(fileContent);
                }

                FormContent.Add(new StringContent(medicineDto.MedicineName), "MedicineName");
                FormContent.Add(new StringContent(medicineDto.Description), "Description");
                FormContent.Add(new StringContent(medicineDto.Quantity.ToString()), "Quantity");
                FormContent.Add(new StringContent(medicineDto.MedicineName), "MedicineName");
                FormContent.Add(new StringContent(medicineDto.ImportDate.ToShortDateString()), "ImportDate");
                FormContent.Add(new StringContent(medicineDto.ExpirationDate.ToShortDateString()), "ExpirationDate");
                FormContent.Add(new StringContent(medicineDto.Ingredients), "Ingredients");
                FormContent.Add(new StringContent(medicineDto.Direction), "Direction");
                FormContent.Add(new StringContent(medicineDto.Status.ToString()), "Status");
                FormContent.Add(new StringContent(medicineDto.ClassificationID.ToString()), "ClassificationID");
                FormContent.Add(new StringContent(medicineDto.PharmacyId.ToString()), "PharmacyId");

                var response = await client.PostAsync(EndpointConstants.MedicineService.CREATE_MEDICINE, FormContent);
                if (response.IsSuccessStatusCode && response != null)
                {
                    var medicine = await response.Content.ReadAsAsync<MedicineDto>();
                    return Json(new { success = true, item = medicine });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            

            return View();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMedicine(AddUpdateMedicineDto medicineDto)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            if (ModelState.IsValid)
            {
                var FormContent = new MultipartFormDataContent();
                if (medicineDto.Images != null)
                {
                    var fileContent = new StreamContent(medicineDto.Images.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(medicineDto.Images.ContentType);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Images",
                        FileName = medicineDto.Images.Name,
                        FileNameStar = medicineDto.Images.Name
                    };
                    FormContent.Add(fileContent);
                }

                FormContent.Add(new StringContent(medicineDto.MedicineName), "MedicineName");
                FormContent.Add(new StringContent(medicineDto.Description), "Description");
                FormContent.Add(new StringContent(medicineDto.Quantity.ToString()), "Quantity");
                FormContent.Add(new StringContent(medicineDto.MedicineName), "MedicineName");
                FormContent.Add(new StringContent(medicineDto.ImportDate.ToShortDateString()), "ImportDate");
                FormContent.Add(new StringContent(medicineDto.ExpirationDate.ToShortDateString()), "ExpirationDate");
                FormContent.Add(new StringContent(medicineDto.Ingredients), "Ingredients");
                FormContent.Add(new StringContent(medicineDto.Direction), "Direction");
                FormContent.Add(new StringContent(medicineDto.Status.ToString()), "Status");
                FormContent.Add(new StringContent(medicineDto.ClassificationID.ToString()), "ClassificationID");
                FormContent.Add(new StringContent(medicineDto.PharmacyId.ToString()), "PharmacyId");

                var response = await client.PutAsync($"{EndpointConstants.MedicineService.UPDATE_MEDICINE}/{medicineDto.Id}", FormContent);
                if (response.IsSuccessStatusCode && response != null)
                {
                    var medicine = await response.Content.ReadAsAsync<MedicineDto>();
                    return Json(new { success = true,item = medicine });
                }
                else
                {
                    return Json(new { success = false });
                }
            }


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ReturnMedicine(int id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.MedicineService.RETURN_MEDICINE}/{id}");
            if (response.IsSuccessStatusCode && response != null)
            {
                var medicine = await response.Content.ReadAsAsync<MedicineDto>();
                return Json(new { success = true, item = medicine });
            }
            return Json(new { success = false });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.DeleteAsync($"{EndpointConstants.MedicineService.DELETE_MEDICINE}/{id}");
            if (response.IsSuccessStatusCode && response != null)
            {
                var medicine = await response.Content.ReadAsAsync<MedicineDto>();
                return Json(new { success = true});
            }
            return Json(new { success = false });
        }

    }
}
