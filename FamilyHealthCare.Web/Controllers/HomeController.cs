using Contract.DTOs.ManagementService;
using Contract.DTOs.SearchService;
using FamilyHealthCare.Customer.Models;
using FamilyHealthCare.SharedLibrary;
using FamilyHealthCare.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using Contract.DTOs.NotificationServiceDtos;

namespace FamilyHealthCare.Customer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContext;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _httpContext = httpContext;
        }


        public async Task<IActionResult> Index()
        {
            var httpClient = _clientFactory.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await httpClient.GetAsync(EndpointConstants.ManagementService.DOCTORS);
            var data = new List<DoctorDetailsDto>();
            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadAsAsync<List<DoctorDetailsDto>>();
            }

            GetNotification();

            HomeViewModel homeVM = new HomeViewModel
            {
                Doctors = data,
            };
            return View(homeVM);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Test()
        {
            var httpClient = _clientFactory.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await httpClient.GetAsync(EndpointConstants.TEST);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchCategoryDto searchCategoryDto)
        {
            var httpClient = _clientFactory.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var json = JsonConvert.SerializeObject(searchCategoryDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(EndpointConstants.SearchService.SEARCH, data);

            var respsoneData = new List<SearchMedicineDto>();
           
            //var result = await response.Content.ReadAsStringAsync();
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
                if(searchCategoryDto.FilterCates != null)
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
                FilterCates = filteredCates
            };
            return View(model);
        }

        private async void GetNotification()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                var httpClient = _clientFactory.CreateClient(ServiceConstants.NOTIFICATION_NAMED_CLIENT);
                var response = await httpClient.GetAsync($"{EndpointConstants.NotificationService.NOTIFICAITON}/{userId}");
                var data = new List<NotificationListDto>();
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsAsync<List<NotificationListDto>>();
                }
                HttpContext.Session.SetComplexData("notification", data);
            }
        }
    }
}
