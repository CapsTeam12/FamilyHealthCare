using Business.IServices;
using Contract.DTOs.ManagementService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly IManagementService _managementService;

        public ManagementController(IManagementService managementService)
        {
            _managementService = managementService;
        }
        // GET: api/<ManagementController>
        [HttpGet("doctors")]
        public async Task<List<DoctorDetailsDto>> GetDoctorsAsync()
        {
            var doctors = await _managementService.GetDoctorsAsync();
            return doctors;
        }

        [HttpGet("patients")]
        public async Task<List<PatientDetailsDto>> GetPatientsAsync()
        {
            var patients = await _managementService.GetPatientsAsync();
            return patients;
        }

        [HttpGet("pharmacies")]
        public async Task<List<PharmacyDetailsDto>> GetPharmacysAsync()
        {
            var pharmacies = await _managementService.GetPharmaciesAsync();
            return pharmacies;
        }

        [HttpGet("categories")]
        public async Task<List<CategoriesDetailsDto>> GetCategoriesAsync()
        {
            var categories = await _managementService.GetCategoriesAsync();
            return categories;
        }

        [HttpGet("specialities")]
        public async Task<List<SpecialitiesDetailsDto>> GetSpecialitiesAsync()
        {
            var specialities = await _managementService.GetSpecialitiesAsync();
            return specialities;
        }

        [HttpGet("doctors/{id}")]
        public async Task<DoctorDetailsDto> GetDoctorDetailsAsync(string id)
        {
            var doctors = await _managementService.GetDoctorDetailsAsync(id);
            return doctors;
        }

        [HttpGet("patients/{id}")]
        public async Task<PatientDetailsDto> GetPatientDetailsAsync(string id)
        {
            var patients = await _managementService.GetPatientDetailsAsync(id);
            return patients;
        }

        [HttpGet("pharmacies/{id}")]
        public async Task<PharmacyDetailsDto> GetPharmacyDetailssAsync(string id)
        {
            var pharmacies = await _managementService.GetPharmacyDetailsAsync(id);
            return pharmacies;
        }

        [HttpGet("categories/{id}")]
        public async Task<CategoriesDetailsDto> GetCategoryDetailsAsync(string id)
        {
            var categories = await _managementService.GetCategoryDetailsAsync(id);
            return categories;
        }

        [HttpGet("specialities/{id}")]
        public async Task<SpecialitiesDetailsDto> GetSpecializedDetailsAsync(string id)
        {
            var specialities = await _managementService.GetSpecializedDetailsAsync(id);
            return specialities;
        }
    }
}
