using Business.IServices;
using Contract.DTOs.ManagementService;
using Contract.DTOs.PartnerService;
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
        private readonly IParnerService _partnerService;
        private readonly IDashboardService _dashboardService;

        public ManagementController(IManagementService managementService,IParnerService parnerService, IDashboardService dashboardService)
        {
            _managementService = managementService;
            _partnerService = parnerService;
            _dashboardService = dashboardService;
        }

        
        [HttpGet("[action]")]
        public async Task<IActionResult> DoctorRequestList()
        {
            var doctorListDto = await _partnerService.DoctorRequestList();
            if (doctorListDto != null)
                return Ok(doctorListDto);
            return NoContent();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> PharmacyRequestList()
        {
            var pharmacyListDto = await _partnerService.PharmacyRequestList();
            if (pharmacyListDto != null)
                return Ok(pharmacyListDto);
            return NoContent();
        }

        [HttpGet("[action]/{doctorId}")]
        public async Task<IActionResult> AcceptDoctorRequest(int doctorId)
        {
            var DoctorDto = await _partnerService.AcceptDoctorRequest(doctorId);
            if (DoctorDto != null)
                return Ok(DoctorDto);
            return NoContent();
        }

        [HttpGet("[action]/{pharmacyId}")]
        public async Task<IActionResult> AcceptPharmacyRequest(int pharmacyId)
        {
            var PharmacyDto = await _partnerService.AcceptPharmacyRequest(pharmacyId);
            if (PharmacyDto != null)
                return Ok(PharmacyDto);
            return NoContent();
        }

        [HttpGet("[action]/{doctorId}")]
        public async Task<IActionResult> DenyDoctorRequest(int doctorId)
        {
            var doctorDto = await _partnerService.DenyDoctorRequest(doctorId);
            if (doctorDto == null)
            {
                return NotFound($"Doctor Id = {doctorId} not found.");
            }
            return Ok(doctorDto);
        }

        [HttpGet("[action]/{pharmacyId}")]
        public async Task<IActionResult> DenyPharmacyRequest(int pharmacyId)
        {
            var pharmacyDto = await _partnerService.DenyPharmacyRequest(pharmacyId);
            if(pharmacyDto == null)
            {
                return NotFound($"Pharmacy Id = {pharmacyId} not found.");
            }
            return Ok(pharmacyDto);
        }

        [HttpGet("[action]/{doctorId}")]
        public async Task<IActionResult> GetDetailsDoctorRequest(int doctorId)
        {
            var doctorDto = await _partnerService.GetDetailsDoctorRequest(doctorId);
            if(doctorDto == null)
            {
                return NotFound($"Doctor Id = {doctorId} not found.");
            }
            return Ok(doctorDto);
        }

        [HttpGet("[action]/{pharmacyId}")]
        public async Task<IActionResult> GetDetailsPharmacyRequest(int pharmacyId)
        {
            var pharmacyDto = await _partnerService.GetDetailsPharmacyRequest(pharmacyId);
            if(pharmacyDto == null)
            {
                return NotFound($"Pharmacy Id = {pharmacyId} not found.");
            }
            return Ok(pharmacyDto);
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
        public async Task<IActionResult> GetCategoriesAsync()
        {
            var categories = await _managementService.GetCategoriesAsync();
            return Ok(categories);
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
        [HttpPut("specialities/update")]
        public async Task<IActionResult> UpdateSpecialities([FromForm] SpecialitiesUpdateDto specialitiesUpdateDto)
        {
            var specializeDto = await _managementService.UpdateSpecialities(specialitiesUpdateDto);
            return Ok(specializeDto);
        }

        [HttpGet("[action]")]
        public IActionResult GetTotalPatients()
        {
            return Ok(_dashboardService.GetTotalPatients());
        }

        [HttpGet("[action]")]
        public IActionResult GetTotalDoctors()
        {
            return Ok(_dashboardService.GetTotalDoctors());
        }

        [HttpGet("[action]")]
        public IActionResult GetTotalPharmacies()
        {
            return Ok(_dashboardService.GetTotalPharmacies());
        }

       
        [HttpGet("[action]/{id}")]
        public IActionResult GetTotalMedicalsByDoctor(string id)
        {
            return Ok(_dashboardService.GetTotalMedicalRecordsByDoctor(id));
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetTotalMedicalsByPatient(string id)
        {
            return Ok(_dashboardService.GetTotalMedicalRecordsByPatient(id));
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetTotalMedicines(string id)
        {
            return Ok(_dashboardService.GetTotalMedicines(id));
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetTotalPrescriptions(string id)
        {
            return Ok(_dashboardService.GetTotalPrescriptions(id));
        }
    }
}
