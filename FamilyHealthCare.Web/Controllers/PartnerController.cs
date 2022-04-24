using Contract.DTOs.ManagementService;
using Contract.DTOs.PartnerService;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Controllers
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

        private async Task<List<SpecialitiesDetailsDto>> GetSpecialities()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ManagementService.SPECIALIST);
            var jsonData = await response.Content.ReadAsStringAsync();
            var specialities = JsonConvert.DeserializeObject<List<SpecialitiesDetailsDto>>(jsonData);
            return specialities;
        }

        public async Task<IActionResult> DoctorRegister()
        {
            //var specialities = await GetSpecialities();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DoctorRegister(DoctorRegisterDto doctorRegisterDto)
        {         
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            if (ModelState.IsValid)
            {
                var FormContent = new MultipartFormDataContent();

                if (doctorRegisterDto.Avatar != null)
                {
                    var fileContent = new StreamContent(doctorRegisterDto.Avatar.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(doctorRegisterDto.Avatar.ContentType);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Avatar",
                        FileName = doctorRegisterDto.Avatar.Name,
                        FileNameStar = doctorRegisterDto.Avatar.Name
                    };
                    FormContent.Add(fileContent);
                }

                if (doctorRegisterDto.Certifications != null)
                {
                    var fileContent = new StreamContent(doctorRegisterDto.Certifications.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(doctorRegisterDto.Certifications.ContentType);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Certifications",
                        FileName = doctorRegisterDto.Certifications.Name,
                        FileNameStar = doctorRegisterDto.Certifications.Name
                    };
                    FormContent.Add(fileContent);
                }

                FormContent.Add(new StringContent(doctorRegisterDto.FullName), "FullName");
                FormContent.Add(new StringContent(doctorRegisterDto.Address), "Address");
                FormContent.Add(new StringContent(doctorRegisterDto.Email), "Email");
                FormContent.Add(new StringContent(doctorRegisterDto.Phone), "Phone");
                FormContent.Add(new StringContent(doctorRegisterDto.Gender.ToString()), "Gender");
                FormContent.Add(new StringContent(doctorRegisterDto.DateOfBirth.ToString()), "DateOfBirth");
                FormContent.Add(new StringContent(doctorRegisterDto.Biography), "Biography");
                for(int i = 0;i< doctorRegisterDto.Experiences.Count; i++)
                {
                    var obj = doctorRegisterDto.Experiences[i];
                    FormContent.Add(new StringContent(obj.HospitalName), "Experiences["+i+"].HospitalName");
                    FormContent.Add(new StringContent(obj.From.ToString()), "Experiences[" + i + "].From");
                    FormContent.Add(new StringContent(obj.To.ToString()), "Experiences[" + i + "].To");
                    FormContent.Add(new StringContent(obj.Designation), "Experiences[" + i + "].Designation");
                }
                
                FormContent.Add(new StringContent(doctorRegisterDto.SpecializedId.ToString()), "SpecializedId");


                var response = await client.PostAsync(EndpointConstants.PatientService.DOCTOR_REGISTER, FormContent);
                if (response.IsSuccessStatusCode && response != null)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }

            return View();
        }

        public async Task<IActionResult> PharmacyRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PharmacyRegister(PharmacyRegisterDto pharmacyRegisterDto)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            if (ModelState.IsValid)
            {
                var FormContent = new MultipartFormDataContent();

                if (pharmacyRegisterDto.Avatar != null)
                {
                    var fileContent = new StreamContent(pharmacyRegisterDto.Avatar.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(pharmacyRegisterDto.Avatar.ContentType);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Avatar",
                        FileName = pharmacyRegisterDto.Avatar.Name,
                        FileNameStar = pharmacyRegisterDto.Avatar.Name
                    };
                    FormContent.Add(fileContent);
                }

                if (pharmacyRegisterDto.Certifications != null)
                {
                    var fileContent = new StreamContent(pharmacyRegisterDto.Certifications.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(pharmacyRegisterDto.Certifications.ContentType);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Certifications",
                        FileName = pharmacyRegisterDto.Certifications.Name,
                        FileNameStar = pharmacyRegisterDto.Certifications.Name
                    };
                    FormContent.Add(fileContent);
                }

                FormContent.Add(new StringContent(pharmacyRegisterDto.PharmacyName), "PharmacyName");
                FormContent.Add(new StringContent(pharmacyRegisterDto.Address), "Address");
                FormContent.Add(new StringContent(pharmacyRegisterDto.Email), "Email");
                FormContent.Add(new StringContent(pharmacyRegisterDto.Phone), "Phone");
                FormContent.Add(new StringContent(pharmacyRegisterDto.PostalCode.ToString()), "PostalCode");
                FormContent.Add(new StringContent(pharmacyRegisterDto.Biography), "Biography");
                for (int i = 0; i < pharmacyRegisterDto.Awards.Count; i++)
                {
                    var obj = pharmacyRegisterDto.Awards[i];
                    FormContent.Add(new StringContent(obj.Award), "Awards[" + i + "].Award");
                    FormContent.Add(new StringContent(obj.Year), "Awards[" + i + "].Year");
                }
           
                var response = await client.PostAsync(EndpointConstants.PatientService.PHARMACY_REGISTER, FormContent);
                if (response.IsSuccessStatusCode && response != null)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckEmailExist(string email)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("email", email));
            var content = new FormUrlEncodedContent(parameters);
            var response = await client.PostAsync($"{EndpointConstants.PatientService.CHECK_EMAIL_EXIST}", content);
            var data = await response.Content.ReadAsAsync<bool>();
            var jsonString = JsonConvert.SerializeObject(data);
            if (response.IsSuccessStatusCode)
            {
                return Json(data);
            }
            return Json(data);
        }
    }
}
