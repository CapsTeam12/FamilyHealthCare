using Contract.DTOs.ManagementService;
using Contract.DTOs.MedicineService;
using Contract.DTOs.SearchService;
using FamilyHealthCare.Customer.Models;
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
    public class PharmacyController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _httpContext;

        public PharmacyController(IHttpClientFactory httpClient, IHttpContextAccessor httpContext)
        {
            _httpClient = httpClient;
            _httpContext = httpContext;
        }

        private async Task<PharmacyDetailsDto> GetPharmacyDetails(int id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.SearchService.PHARMACYSEARCH}/{id}");
            var pharmacy = await response.Content.ReadAsAsync<PharmacyDetailsDto>();
            return pharmacy;
        }


        [HttpGet]
        public async Task<IActionResult> PharmacyDetails(int id)
        {
            var pharmacy = await GetPharmacyDetails(id);
            return View(pharmacy);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync(EndpointConstants.ManagementService.PHARMACIES);
            if (response.IsSuccessStatusCode)
            {
                var pharmaciesDto = await response.Content.ReadAsAsync<IEnumerable<PharmacyDetailsDto>>();
                return View(pharmaciesDto);
            }
            return NotFound();
        }


        [HttpGet]
        public async Task<IActionResult> Medicines(int id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.MedicineService.MEDICINES_PHARMACY_VIEW}/{id}");
            var model = new SearchViewModel();
            model.Pharmacy = await GetPharmacyDetails(id);
            if (response.IsSuccessStatusCode)
            {
                var medicinesDto = await response.Content.ReadAsAsync<IEnumerable<SearchMedicineDto>>();
                model.SearchMedicineDtos = medicinesDto.ToList();
                
            }
            ViewBag.resultCount = model.SearchMedicineDtos.Count();
            var responseCate = await client.GetAsync(EndpointConstants.ManagementService.CATEGORIES);
            var dataCate = new List<CategoriesDetailsDto>();
            if (responseCate.IsSuccessStatusCode)
            {
                dataCate = await responseCate.Content.ReadAsAsync<List<CategoriesDetailsDto>>();
                var filteredCates = new List<FilterCate>();
                for (int i = 0;i< dataCate.Count; i++)
                {
                    filteredCates.Add(new FilterCate { 
                    Name = dataCate[i].CateName,
                    IsSelected = false
                    });
                }
                model.FilterCates = filteredCates;
            }
            return View(model);
        }

        [HttpGet]
        [Route("Pharmacy/Medicines/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await client.GetAsync($"{EndpointConstants.MedicineService.DETAILS_MEDICINE}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var medicineDto = await response.Content.ReadAsAsync<MedicineDto>();
                return View(medicineDto);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Search(int id,SearchCategoryDto searchCategoryDto)
        {
            if(searchCategoryDto.Search == null && searchCategoryDto.FilterCates == null)
            {
                return RedirectToAction("Medicines", "Pharmacy",new { id = id });
            }
            var httpClient = _httpClient.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var json = JsonConvert.SerializeObject(searchCategoryDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{EndpointConstants.SearchService.SEARCH}/{id}", data);

            var respsoneData = new List<SearchMedicineDto>();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsAsync<IEnumerable<SearchMedicineDto>>();
                respsoneData = content.ToList();
            }
            ViewBag.resultCount = respsoneData.Count();
            ViewBag.keyword = searchCategoryDto.Search;

            var responseCate = await httpClient.GetAsync(EndpointConstants.ManagementService.CATEGORIES);
            var dataCate = new List<CategoriesDetailsDto>();
            if (responseCate.IsSuccessStatusCode)
            {
                dataCate = await responseCate.Content.ReadAsAsync<List<CategoriesDetailsDto>>();
            }

            var filteredCates = new List<FilterCate>();
            for (int i = 0; i < dataCate.Count; i++)
            {
                if (searchCategoryDto.FilterCates != null)
                {
                    if (searchCategoryDto.FilterCates.Contains(dataCate[i].CateName))
                    {
                        filteredCates.Add(new FilterCate
                        {
                            Name = dataCate[i].CateName,
                            IsSelected = true
                        });
                    }
                    else
                    {
                        filteredCates.Add(new FilterCate
                        {
                            Name = dataCate[i].CateName,
                            IsSelected = false
                        });
                    }
                }
                else
                {
                    filteredCates.Add(new FilterCate
                    {
                        Name = dataCate[i].CateName,
                        IsSelected = false
                    });
                }
            }

            var model = new SearchViewModel
            {
                SearchMedicineDtos = respsoneData,
                FilterCates = filteredCates,
                Pharmacy = await GetPharmacyDetails(id)               
            };
            return View("Medicines",model);
        }


    }
}
