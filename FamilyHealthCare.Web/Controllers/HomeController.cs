using Contract.DTOs.ManagementService;
using Contract.DTOs.SearchService;
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
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
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

        public async Task<IActionResult> Search(string search)
        {
            var httpClient = _clientFactory.CreateClient(ServiceConstants.BACK_END_NAMED_CLIENT);
            var response = await httpClient.GetAsync(EndpointConstants.SearchService.SEARCH);
            var data = new List<SearchMedicineDto>();
            response.EnsureSuccessStatusCode();
            //var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                data = await response.Content.ReadAsAsync<List<SearchMedicineDto>>();
            ViewBag.search = search;
            return View(data);
        }
    }
}
