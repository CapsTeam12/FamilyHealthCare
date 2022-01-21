using Business.IServices;
using Contract.DTOs.SearchService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost("search/medicine")]
        public async Task<IEnumerable<SearchMedicineDto>> GetMedicinesAsync(SearchCategoryDto searchCategoryDto)
        {
            var medicines = await _searchService.GetSearchMedicineResultAsync(searchCategoryDto);
            return medicines;
        }

        [HttpGet("search/medicine/{id}")]
        public async Task<IActionResult> GetDetailsMedicinesAsync(int id)
        {
            var medicine = await _searchService.GetDetailsSearchMedicineAsync(id);
            return Ok(medicine);
        }

        [HttpGet("search/doctor")]
        public async Task<IActionResult> GetDoctorsAsync(string search)
        {
            var doctors = await _searchService.GetSearchDoctorResultAsync(search);
            return Ok(doctors);
        }

        [HttpGet("search/doctor/{id}")]
        public async Task<IActionResult> GetDetailsDoctorAsync(int id)
        {
            var doctor = await _searchService.GetDetailsSearchDoctorAsync(id);
            return Ok(doctor);
        }

        [HttpGet("search/pharmacy")]
        public async Task<IActionResult> GetPharmacysAsync(string search)
        {
            var pharmacies = await _searchService.GetSearchPharmacyResultAsync(search);
            return Ok(pharmacies);
        }

        [HttpGet("search/pharmacy/{id}")]
        public async Task<IActionResult> GetDetailsPharmacyAsync(int id)
        {
            var pharmacy = await _searchService.GetDetailsSearchPharmacyAsync(id);
            return Ok(pharmacy);
        }
    }
}
