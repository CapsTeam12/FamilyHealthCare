using Contract.DTOs.ManagementService;
using Contract.DTOs.SearchService;
using Data.Entities;
using FamilyHealthCare.Customer.Models;
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

namespace FamilyHealthCare.Customer.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _httpContext;

        public DoctorController(IHttpClientFactory httpClient, IHttpContextAccessor httpContext)
        {
            _httpClient = httpClient;
            _httpContext = httpContext;
        }

        private async Task<DoctorDetailsDto> GetDoctorDetails(int id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.SearchService.DOCTORDETAILS}/{id}");
            var doctor = await response.Content.ReadAsAsync<DoctorDetailsDto>();
            return doctor;
        }

        private async Task<List<SpecialitiesDetailsDto>> GetSpecialities()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ManagementService.SPECIALIST);
            var jsonData = await response.Content.ReadAsStringAsync();
            var specialities = JsonConvert.DeserializeObject<List<SpecialitiesDetailsDto>>(jsonData);
            return specialities;
        }

        private async Task<List<DoctorDetailsDto>> GetDoctors()
        {
            var httpClient = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await httpClient.GetAsync(EndpointConstants.ManagementService.DOCTORS);
            var doctors = new List<DoctorDetailsDto>();
            if (response.IsSuccessStatusCode)
            {
                doctors = await response.Content.ReadAsAsync<List<DoctorDetailsDto>>();
            }
            return doctors;
        }

        [HttpGet]
        public async Task<IActionResult> Search()
        {
            var specialities = await GetSpecialities();
            var doctors = await GetDoctors();
            var doctorSearchViewModel = new DoctorSearchViewModel()
            {
                Specialities = specialities,
                Doctors = doctors
            };
            return View(doctorSearchViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SearchByFilters(SearchDoctorDto searchDoctorDto)
        {
            if(searchDoctorDto.Gender == null && searchDoctorDto.Specialities == null)
            {
                return RedirectToAction("Search","Doctor");
            }
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);            
            var stringModel = JsonConvert.SerializeObject(searchDoctorDto);
            var stringContent = new StringContent(stringModel, Encoding.UTF8, "application/json");            
            var response = await client.PostAsync(EndpointConstants.SearchService.DOCTORSEARCH,stringContent);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<List<DoctorDetailsDto>>();
                var doctorSearchViewModel = new DoctorSearchViewModel()
                {
                    Specialities = await GetSpecialities(),
                    Doctors = data
                };
                ViewBag.resultCount = data.Count;
                ViewBag.filterGender = searchDoctorDto.Gender;
                ViewBag.filterSpecialize = searchDoctorDto.Specialities;
                return View("Search",doctorSearchViewModel);
            }
            return View("Search");
        }

        
        public async Task<IActionResult> DoctorMapList(double lat,double lng,string name)
        {
            var doctors = new DoctorSearchViewModel()
            {
                Specialities = await GetSpecialities(),
                Doctors = await GetDoctors(),
            };
            ViewBag.Lat = lat;
            ViewBag.Lng = lng;
            ViewBag.Name = name;
            return View("MapList",doctors);
        }

        [HttpGet]
        public async Task<IActionResult> DoctorDetails(int id)
        {
            var doctor = await GetDoctorDetails(id);
            return View(doctor);
        }
    }
}
